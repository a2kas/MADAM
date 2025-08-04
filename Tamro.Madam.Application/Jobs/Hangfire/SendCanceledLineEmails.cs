using Hangfire;
using Hangfire.Server;
using Microsoft.Extensions.Logging;
using MimeKit;
using Tamro.Madam.Application.Models.Common;
using Tamro.Madam.Application.Services.Sales.CanceledOrderLines.Factories;
using Tamro.Madam.Application.Services.Sales.CanceledOrderLines.Resolvers;
using Tamro.Madam.Application.Services.Sales.Decorators;
using Tamro.Madam.Models.Configuration;
using Tamro.Madam.Models.General;
using Tamro.Madam.Models.Sales.CanceledOrderLines;
using Tamro.Madam.Repository.Context.E1Gateway;
using Tamro.Madam.Repository.Repositories.Sales;
using Tamroutilities.Email.Sender;
using TamroUtilities.EFCore.UnitOfWork;

namespace Tamro.Madam.Application.Jobs.Hangfire;

public class SendCanceledLineEmails : SendCanceledLineEmailsBase, IOneTimeJob
{
    private readonly List<BalticCountry> _countries = [BalticCountry.LV, BalticCountry.LT, BalticCountry.EE];

    private readonly IEnumerable<ICanceledOrderLinesResolver> _resolvers;
    private readonly IE1CanceledOrderRepository _canceledOrderRepository;
    private readonly IUnitOfWork<E1GatewayDbContext> _unitOfWork;
    private readonly ICanceledOrderLinesEmailGeneratorFactory _canceledOrderLinesEmailGeneratorFactory;
    private readonly IEmailSender _emailSender;
    private readonly ISalesOrderCustomerDecorator _canceledOrderCustomerDecorator;
    private readonly List<CanceledLineSetting> _canceledLineSettings;
    private readonly ILogger<SendCanceledLineEmails> _logger;

    public string Name => "Send emails about canceled orders lines";

    public string Description => "Send emails about canceled orders lines";

    public bool Processing { get; set; }

    public SendCanceledLineEmails()
    {
    }

    public SendCanceledLineEmails(
        IEnumerable<ICanceledOrderLinesResolver> resolvers,
        IE1CanceledOrderRepository canceledOrderRepository,
        IUnitOfWork<E1GatewayDbContext> unitOfWork,
        ICanceledOrderLinesEmailGeneratorFactory canceledOrderLinesEmailGeneratorFactory,
        IEmailSender emailSender,
        ISalesOrderCustomerDecorator canceledOrderCustomerDecorator,
        List<CanceledLineSetting> canceledLineSettings,
        ILogger<SendCanceledLineEmails> logger)
    {
        _resolvers = resolvers ?? throw new ArgumentNullException(nameof(resolvers));
        _canceledOrderRepository = canceledOrderRepository ?? throw new ArgumentNullException(nameof(canceledOrderRepository));
        _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        _canceledOrderLinesEmailGeneratorFactory = canceledOrderLinesEmailGeneratorFactory ?? throw new ArgumentNullException(nameof(canceledOrderLinesEmailGeneratorFactory));
        _emailSender = emailSender ?? throw new ArgumentNullException(nameof(emailSender));
        _canceledOrderCustomerDecorator = canceledOrderCustomerDecorator ?? throw new ArgumentNullException(nameof(canceledOrderCustomerDecorator));
        _canceledLineSettings = canceledLineSettings ?? throw new ArgumentNullException(nameof(canceledLineSettings));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public async Task<Result<int>> Execute()
    {
        try
        {
            await SendCanceledEmails();
        }
        catch (Exception ex)
        {
            const string errorMessage = "Failed to send emails about canceled orders lines";
            _logger.LogError(ex, errorMessage);
            return Result<int>.Failure(errorMessage);
        }

        return Result<int>.Success(1);
    }

    public override async Task JobToRun(IJobCancellationToken token, PerformContext context)
    {
        await SendCanceledEmails();
    }

    private async Task SendCanceledEmails()
    {
        foreach (var country in _countries)
        {
            var orders = await GetCanceledOrders(country);
            if (!orders.Any())
            {
                continue;
            }

            var historicalFailureSendingOrdersIds = SelectFailureSendingOrdersIds(orders);

            await ResolveOrders(orders, country);

            MarkOrdersThatWillNotBeSent(orders);

            var ordersToBeSent = FilterOrdersToBeSent(orders);
            var ordersNotToBeSent = FilterOrdersNotToBeSent(orders, ordersToBeSent);
            var ordersWithMissingEmail = FilterNotSentOrdersWithMissingEmail(orders, historicalFailureSendingOrdersIds);

            await UpdateOrdersNotToBeSent(ordersNotToBeSent, country);
            await SendEmailsToRecipients(ordersToBeSent, country);
            await NotifyAboutRecipientsWithMissingEmails(ordersWithMissingEmail, country);
        }
    }

    private async Task<IEnumerable<CanceledOrderHeaderModel>> GetCanceledOrders(BalticCountry country)
    {
        var orders = await _canceledOrderRepository.GetCanceledOrders(country, [CanceledOrderLineEmailStatus.NotSent, CanceledOrderLineEmailStatus.FailureSending]);

        _logger.LogInformation($"{nameof(SendCanceledEmails)}: ({country}) Found {orders?.Count() ?? 0} orders with canceled lines.");
        return orders ?? [];
    }

    private async Task ResolveOrders(IEnumerable<CanceledOrderHeaderModel> orders, BalticCountry country)
    {
        foreach (var resolver in _resolvers.OrderBy(x => x.Priority))
        {
            await resolver.Resolve(orders, country);
        }
    }

    private void MarkOrdersThatWillNotBeSent(IEnumerable<CanceledOrderHeaderModel> orders)
    {
        foreach (var order in orders.Where(x => !x.SendCanceledOrderNotification))
        {
            foreach (var line in order.Lines)
            {
                line.EmailStatus = CanceledOrderLineEmailStatus.WillNotBeSent;
            }
        }
    }

    private static IEnumerable<CanceledOrderHeaderModel> FilterOrdersToBeSent(IEnumerable<CanceledOrderHeaderModel> orders)
    {
        return orders.Where(x => x.Lines.Any(line => line.EmailStatus == CanceledOrderLineEmailStatus.NotSent) && x.SendCanceledOrderNotification);
    }

    private static IEnumerable<CanceledOrderHeaderModel> FilterOrdersNotToBeSent(IEnumerable<CanceledOrderHeaderModel> orders, IEnumerable<CanceledOrderHeaderModel> ordersToBeSent)
    {
        return orders.Where(x => !ordersToBeSent.Any(y => y.Id == x.Id));
    }

    private static IEnumerable<CanceledOrderHeaderModel> FilterNotSentOrdersWithMissingEmail(IEnumerable<CanceledOrderHeaderModel> orders, IEnumerable<int> notSentOrdersIds)
    {
        return orders.Where(x => (notSentOrdersIds == null || !notSentOrdersIds.Contains(x.Id)) && x.Lines.Any(line => line.EmailStatus == CanceledOrderLineEmailStatus.FailureSending));
    }

    private static List<int> SelectFailureSendingOrdersIds(IEnumerable<CanceledOrderHeaderModel> orders)
    {
        return orders.Where(x => x.Lines.Any(line => line.EmailStatus == CanceledOrderLineEmailStatus.FailureSending)).Select(x => x.Id).ToList();
    }

    private async Task UpdateOrdersNotToBeSent(IEnumerable<CanceledOrderHeaderModel> ordersNotToBeSent, BalticCountry country)
    {
        foreach (var order in ordersNotToBeSent)
        {
            try
            {
                await _canceledOrderRepository.Update(order);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"{nameof(SendCanceledEmails)}: ({country}) Unable to update canceled order with Id = {order.Id}");
            }
        }
    }

    private async Task SendEmailsToRecipients(IEnumerable<CanceledOrderHeaderModel> ordersToBeSent, BalticCountry country)
    {
        var ordersByRecipients = ordersToBeSent.GroupBy(x => x.E1ShipTo);
        _logger.LogInformation($"{nameof(SendCanceledEmails)}: Preparing and sending emails to {ordersByRecipients.Count()} recipients.");

        foreach (var ordersByRecipient in ordersByRecipients)
        {
            var canceledLinesEmail = _canceledOrderLinesEmailGeneratorFactory.Get(country).GenerateCanceledLinesEmail(ordersByRecipient);

            try
            {
                await _unitOfWork.BeginTransactionAsync();

                foreach (var orderByRecipient in ordersByRecipient)
                {
                    foreach (var line in orderByRecipient.Lines)
                    {
                        line.EmailStatus = CanceledOrderLineEmailStatus.Sent;
                        line.NotificationSendDate = DateTime.UtcNow;
                    }

                    await _canceledOrderRepository.Update(orderByRecipient);
                }

                if (canceledLinesEmail.Receivers.Any(x => !string.IsNullOrEmpty(x)))
                {
                    try
                    {
                        await _emailSender.SendEmailAsync(canceledLinesEmail);
                    }
                    catch (ParseException ex)
                    {
                        _logger.LogWarning(ex, $"{nameof(SendCanceledLineEmails)}: ({country}) Could not parse e-mail address as it is not valid: {string.Join(",", canceledLinesEmail.Receivers)}");
                    }
                }
                await _unitOfWork.CommitAsync();
            }
            catch (Exception ex)
            {
                await _unitOfWork.RollbackAsync();
                _logger.LogError(ex, $"{nameof(SendCanceledEmails)}: ({country}) Unable to update canceled orders or send email with ids: {string.Join(", ", ordersByRecipient.Select(order => order.Id))}");
            }
        }
    }

    private async Task NotifyAboutRecipientsWithMissingEmails(IEnumerable<CanceledOrderHeaderModel> ordersWithMissingEmail, BalticCountry country)
    {
        await _canceledOrderCustomerDecorator.Decorate(ordersWithMissingEmail, country);
        var canceledLineSettings = _canceledLineSettings.FirstOrDefault(x => x.Country.ToLower() == country.ToString().ToLower());

        if (canceledLineSettings == null)
        {
            _logger.LogError($"{nameof(SendCanceledEmails)}: ({country}) is missing responsible person email settings");
            return;
        }

        if (!ordersWithMissingEmail.Any())
        {
            return;
        }
        var missingEmailsNotification = _canceledOrderLinesEmailGeneratorFactory.Get(country).GenerateMissingEmailsNotification(canceledLineSettings.ResponsiblePerson, ordersWithMissingEmail);

        try
        {
            await _emailSender.SendEmailAsync(missingEmailsNotification);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"{nameof(SendCanceledEmails)}: ({country}) Unable to send email to {canceledLineSettings.ResponsiblePerson}");
        }
    }
}
