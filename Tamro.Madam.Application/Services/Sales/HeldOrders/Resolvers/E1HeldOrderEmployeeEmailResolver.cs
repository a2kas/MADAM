using Tamro.Madam.Application.Services.Customers.Factories;
using Tamro.Madam.Models.Employees.Wholesale;
using Tamro.Madam.Models.General;
using Tamro.Madam.Models.Sales.HeldOrders;

namespace Tamro.Madam.Application.Services.Sales.HeldOrders.Resolvers;
public class E1HeldOrderEmployeeEmailResolver : IE1HeldOrdersResolver
{
    private readonly IWholesaleEmployeeRepositoryFactory _wholesaleEmployeeRepositoryFactory;

    public E1HeldOrderEmployeeEmailResolver(IWholesaleEmployeeRepositoryFactory wholesaleEmployeeRepositoryFactory)
    {
        _wholesaleEmployeeRepositoryFactory = wholesaleEmployeeRepositoryFactory ?? throw new ArgumentNullException(nameof(wholesaleEmployeeRepositoryFactory));
    }

    public async Task Resolve(IEnumerable<E1HeldOrderModel> orders, BalticCountry country)
    {
        var responsibleEmployeeNumbers = orders.Select(x => x.ResponsibleEmployeeNumber).Distinct().ToList();

        var searchModel = new WholesaleEmployeeSearchModel
        {
            AddressNumbers = responsibleEmployeeNumbers
        };

        var notificationRecipients = await _wholesaleEmployeeRepositoryFactory.Get(country).GetMany(searchModel);

        var notificationRecipientsDictionary = notificationRecipients.ToDictionary(c => c.AddressNumber, c => new { c.Email });

        foreach (var order in orders)
        {
            if (notificationRecipientsDictionary.TryGetValue(order.ResponsibleEmployeeNumber, out var notificationRecipient))
            {
                order.EmployeesEmail = notificationRecipient.Email;
            }
        }
    }
}
