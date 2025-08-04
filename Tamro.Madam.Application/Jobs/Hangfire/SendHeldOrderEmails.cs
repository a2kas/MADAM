using Hangfire;
using Hangfire.Server;
using Microsoft.Extensions.Logging;
using Tamro.Madam.Application.Models.Common;
using Tamro.Madam.Application.Services.Sales.HeldOrders;
using Tamro.Madam.Application.Services.Sales.HeldOrders.Resolvers;
using Tamro.Madam.Models.General;
using Tamro.Madam.Models.Sales.HeldOrders;
using Tamro.Madam.Repository.Context.E1Gateway;
using TamroUtilities.EFCore.UnitOfWork;

namespace Tamro.Madam.Application.Jobs.Hangfire;
public class SendHeldOrderEmails : SendHeldOrderEmailsBase, IOneTimeJob
{
    private readonly List<BalticCountry> _countries = [BalticCountry.LV];

    private readonly IEnumerable<IE1HeldOrdersResolver> _resolvers;
    private readonly IE1HeldOrderService _e1HeldOrderService;
    private readonly IE1HeldOrderEmailService _e1HeldOrderEmailService;
    private readonly IE1HeldOrdersEmailStatusResolver _e1HeldOrdersEmailStatusResolver;
    private readonly TimeProvider _timeProvider;
    private readonly IUnitOfWork<E1GatewayDbContext> _unitOfWork;
    private readonly ILogger<SendHeldOrderEmails> _logger;

    public string Name => "Send emails about held orders";

    public string Description => "Send emails about held orders";

    public bool Processing { get; set; }

    public SendHeldOrderEmails()
    {

    }

    public SendHeldOrderEmails(
        IEnumerable<IE1HeldOrdersResolver> resolvers,
        IE1HeldOrderService e1HeldOrderService,
        IE1HeldOrderEmailService heldOrderEmailService,
        IE1HeldOrdersEmailStatusResolver e1HeldOrdersEmailStatusResolver,
        TimeProvider timeProvider,
        IUnitOfWork<E1GatewayDbContext> unitOfWork,
        ILogger<SendHeldOrderEmails> logger)
    {
        _resolvers = resolvers ?? throw new ArgumentNullException(nameof(resolvers));
        _e1HeldOrderService = e1HeldOrderService ?? throw new ArgumentNullException(nameof(e1HeldOrderService));
        _e1HeldOrderEmailService = heldOrderEmailService ?? throw new ArgumentNullException(nameof(heldOrderEmailService));
        _e1HeldOrdersEmailStatusResolver = e1HeldOrdersEmailStatusResolver ?? throw new ArgumentNullException(nameof(e1HeldOrdersEmailStatusResolver));
        _timeProvider = timeProvider ?? throw new ArgumentNullException(nameof(timeProvider));
        _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public override async Task JobToRun(IJobCancellationToken token, PerformContext context)
    {
        await SendHeldEmails();
    }

    public async Task<Result<int>> Execute()
    {
        try
        {
            await SendHeldEmails();
        }
        catch (Exception ex)
        {
            const string errorMessage = "Failed to send emails about held orders";
            _logger.LogError(ex, errorMessage);
            return Result<int>.Failure(errorMessage);
        }

        return Result<int>.Success(1);
    }

    private async Task SendHeldEmails()
    {
        foreach (var country in _countries)
        {
            var orders = await GetHeldOrders(country);

            if (!orders.Any())
            {
                continue;
            }

            await ResolveOrders(orders, country);

            _e1HeldOrderEmailService.ResolveEmailValidity(orders);

            _e1HeldOrdersEmailStatusResolver.ResolveNotifcationStatus(orders);

            await SendEmailsToCustomersAndUpdateStatuses(orders.Where(x => x.HasValidCustomerEmails), country);

            await NotifyAboutInvalidEmails(orders.Where(x => x.NotificationStatus == E1HeldNotificationStatusModel.FailureSending &&
                                                        x.OldNotificationStatus != E1HeldNotificationStatusModel.FailureSending)
                                            , country);

            await _e1HeldOrderService.UpdateMany(orders.Where(x => x.NotificationStatus != E1HeldNotificationStatusModel.Sent));
        }
    }

    private async Task NotifyAboutInvalidEmails(IEnumerable<E1HeldOrderModel> orders, BalticCountry country)
    {
        _logger.LogInformation($"{nameof(SendHeldOrderEmails)}: notifying employees about orders with invalid emails, ordes with invalid emails count: '{orders.Count()}'");

        var ordersWithInvalidEmployee = orders.Where(x => !x.HasValidEmployeeEmails).ToList();
        foreach (var orderWithInvalidEmployee in ordersWithInvalidEmployee)
        {
            _logger.LogError($"{nameof(SendHeldOrderEmails)}: employee for order with E1ShipTo: '{orderWithInvalidEmployee.E1ShipTo}' has no valid email");
        }

        //TODO: revert OTHERS-2703 when Held orders functionality ready to start to work
        //await _e1HeldOrderEmailService.SendEmployeeEmails(orders, country);
    }

    private async Task SendEmailsToCustomersAndUpdateStatuses(IEnumerable<E1HeldOrderModel> ordersToBeSent, BalticCountry country)
    {
        _logger.LogInformation($"{nameof(SendHeldOrderEmails)}: Preparing and sending emails about '{ordersToBeSent.Count()}' held orders");

        var now = _timeProvider.GetUtcNow().DateTime;

        foreach (var orderToBeSent in ordersToBeSent)
        {
            try
            {
                await _unitOfWork.BeginTransactionAsync();

                orderToBeSent.NotificationStatus = E1HeldNotificationStatusModel.Sent;
                orderToBeSent.NotificationSendDate = now;

                await _e1HeldOrderService.Update(orderToBeSent);

                //TODO: revert OTHERS-2703 when Held orders functionality ready to start to work
                //await _e1HeldOrderEmailService.SendCustomerEmail(orderToBeSent, country);
                await _unitOfWork.CommitAsync();
            }
            catch (Exception ex)
            {
                orderToBeSent.NotificationStatus = orderToBeSent.OldNotificationStatus;
                orderToBeSent.NotificationSendDate = orderToBeSent.OldNotificationSendDate;

                await _unitOfWork.RollbackAsync();
                _logger.LogError(ex, $"{nameof(SendHeldOrderEmails)}: ({country}) Unable to update held order or send email with id: {orderToBeSent.Id}");
            }
        }
    }

    private async Task ResolveOrders(IEnumerable<E1HeldOrderModel> orders, BalticCountry country)
    {
        foreach (var resolver in _resolvers)
        {
            await resolver.Resolve(orders, country);
        }
    }

    private async Task<IEnumerable<E1HeldOrderModel>> GetHeldOrders(BalticCountry country)
    {
        var orders = await _e1HeldOrderService.GetHeldOrders(country, [E1HeldNotificationStatusModel.NotSent, E1HeldNotificationStatusModel.FailureSending]);

        _logger.LogInformation($"{nameof(SendHeldOrderEmails)}: ({country}) found '{orders?.Count() ?? 0}' held orders");
        return orders ?? [];
    }
}
