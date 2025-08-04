using Tamro.Madam.Application.Services.Customers.Factories;
using Tamro.Madam.Models.Customers.Wholesale;
using Tamro.Madam.Models.General;
using Tamro.Madam.Models.Sales;

namespace Tamro.Madam.Application.Services.Sales.Decorators;
public class SalesOrderCustomerDecorator : ISalesOrderCustomerDecorator
{
    private readonly IWholesaleCustomerRepositoryFactory _wholesaleCustomerRepositoryFactory;

    public SalesOrderCustomerDecorator(IWholesaleCustomerRepositoryFactory wholesaleCustomerRepositoryFactory)
    {
        _wholesaleCustomerRepositoryFactory = wholesaleCustomerRepositoryFactory;
    }

    public async Task Decorate(IEnumerable<ISalesOrderHeader> orders, BalticCountry country)
    {
        var customerAddressNumbers = orders.Select(x => x.E1ShipTo).Distinct();
        var customers = (await _wholesaleCustomerRepositoryFactory.Get(country).GetClsf(customerAddressNumbers, WholesaleCustomerType.All, 1, int.MaxValue)).Items;
        var customerDictionary = customers.ToDictionary(c => c.AddressNumber, c => c.Name);

        foreach (var order in orders)
        {
            if (customerDictionary.TryGetValue(order.E1ShipTo, out var customerName))
            {
                order.CustomerName = customerName;
            }
        }
    }
}
