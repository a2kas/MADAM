using Tamro.Madam.Application.Services.Customers.Factories;
using Tamro.Madam.Models.Customers.Wholesale;
using Tamro.Madam.Models.General;
using Tamro.Madam.Models.Sales.HeldOrders;

namespace Tamro.Madam.Application.Services.Sales.HeldOrders.Resolvers;
public class E1HeldOrderCustomerEmailResolver : IE1HeldOrdersResolver
{
    private readonly IWholesaleCustomerRepositoryFactory _wholesaleCustomerRepositoryFactory;

    public E1HeldOrderCustomerEmailResolver(IWholesaleCustomerRepositoryFactory wholesaleCustomerRepositoryFactory)
    {
        _wholesaleCustomerRepositoryFactory = wholesaleCustomerRepositoryFactory ?? throw new ArgumentNullException(nameof(wholesaleCustomerRepositoryFactory));
    }

    public async Task Resolve(IEnumerable<E1HeldOrderModel> orders, BalticCountry country)
    {
        var addressNumbers = orders.Select(x => x.E1ShipTo).Distinct().ToList();

        var searchModel = new WholesaleCustomerSearchModel
        {
            AddressNumbers = addressNumbers
        };

        var notificationRecipients = await _wholesaleCustomerRepositoryFactory.Get(country).GetMany(searchModel);

        var notificationRecipientsDictionary = notificationRecipients.ToDictionary(c => c.AddressNumber, c => new { c.EmailAddress, c.ResponsibleEmployeeNumber, c.MailingName });

        foreach (var order in orders)
        {
            if (notificationRecipientsDictionary.TryGetValue(order.E1ShipTo, out var notificationRecipient))
            {
                order.Email = notificationRecipient.EmailAddress;
                order.ResponsibleEmployeeNumber = notificationRecipient.ResponsibleEmployeeNumber;
                order.MailingName = notificationRecipient.MailingName;
            }
        }
    }
}
