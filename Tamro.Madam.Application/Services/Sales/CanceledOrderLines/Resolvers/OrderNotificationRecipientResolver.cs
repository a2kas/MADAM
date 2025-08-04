using Tamro.Madam.Application.Services.Customers.Factories;
using Tamro.Madam.Models.Customers.Wholesale;
using Tamro.Madam.Models.General;
using Tamro.Madam.Models.Sales.CanceledOrderLines;
using Tamroutilities.Email.Utils;

namespace Tamro.Madam.Application.Services.Sales.CanceledOrderLines.Resolvers;

public class OrderNotificationRecipientResolver : ICanceledOrderLinesResolver
{
    public int Priority => 1;

    private readonly IWholesaleCustomerRepositoryFactory _wholesaleCustomerRepositoryFactory;
    private readonly TimeProvider _timeProvider;

    public OrderNotificationRecipientResolver(IWholesaleCustomerRepositoryFactory wholesaleCustomerRepositoryFactory, TimeProvider timeProvider)
    {
        _wholesaleCustomerRepositoryFactory = wholesaleCustomerRepositoryFactory ?? throw new ArgumentNullException(nameof(wholesaleCustomerRepositoryFactory));
        _timeProvider = timeProvider ?? throw new ArgumentNullException(nameof(timeProvider));
    }

    public async Task Resolve(IEnumerable<CanceledOrderHeaderModel> orders, BalticCountry country)
    {
        var utcnow = _timeProvider.GetUtcNow().DateTime;

        var addressNumbers = orders.Select(x => x.E1ShipTo).Distinct().ToList();

        var searchModel = new WholesaleCustomerSearchModel
        {
            AddressNumbers = addressNumbers
        };

        var notificationRecipients = await _wholesaleCustomerRepositoryFactory.Get(country).GetMany(searchModel);

        var notificationRecipientsDictionary = notificationRecipients.ToDictionary(c => c.AddressNumber, c => new { c.EmailAddress, c.LegalEntityNumber, });

        foreach (var order in orders)
        {
            if (notificationRecipientsDictionary.TryGetValue(order.E1ShipTo, out var notificationRecipient))
            {
                order.SoldTo = notificationRecipient.LegalEntityNumber;

                foreach (var line in order.Lines)
                {
                    line.EmailAddress = SanitizeEmailAddresses(notificationRecipient.EmailAddress);

                    if (IsValidEmail(notificationRecipient.EmailAddress))
                    {
                        line.EmailStatus = CanceledOrderLineEmailStatus.NotSent;
                    }
                    else
                    {
                        line.EmailStatus = DetermineEmailStatusBasedOnCreatedDate(utcnow, line);
                    }
                }
            }
            else
            {
                foreach (var line in order.Lines)
                {
                    line.EmailStatus = DetermineEmailStatusBasedOnCreatedDate(utcnow, line);
                }
            }
        }
    }

    private static bool IsValidEmail(string email)
    {
        var emailAddresses = email.Split(',').Select(e => e.Trim()).ToList();

        foreach (var emailAddress in emailAddresses)
        {
            if (!EmailUtils.IsValidEmail(emailAddress))
            {
                return false;
            }
        }

        return true;
    }

    private static CanceledOrderLineEmailStatus DetermineEmailStatusBasedOnCreatedDate(DateTime utcNow, CanceledOrderLineModel line)
    {
        if (line.CreatedDate < utcNow.AddDays(-3) && line.EmailStatus == CanceledOrderLineEmailStatus.FailureSending)
        {
            return CanceledOrderLineEmailStatus.WillNotBeSent;
        }
        else
        {
            return CanceledOrderLineEmailStatus.FailureSending;
        }
    }

    private string SanitizeEmailAddresses(string email)
    {
        var emailAddresses = email.Split(',')
            .Select(x => !x.Contains('@') ? string.Empty : x.Trim())
            .Where(x => !string.IsNullOrEmpty(x))
            .ToList();

        return string.Join(",", emailAddresses);
    }
}
