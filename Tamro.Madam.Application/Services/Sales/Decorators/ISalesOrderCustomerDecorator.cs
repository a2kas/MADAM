using Tamro.Madam.Models.General;
using Tamro.Madam.Models.Sales;

namespace Tamro.Madam.Application.Services.Sales.Decorators;
public interface ISalesOrderCustomerDecorator
{
    Task Decorate(IEnumerable<ISalesOrderHeader> orders, BalticCountry country);
}
