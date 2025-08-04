using Tamro.Madam.Models.General;
using Tamro.Madam.Models.Sales.HeldOrders;

namespace Tamro.Madam.Application.Services.Sales.HeldOrders.Resolvers;
public interface IE1HeldOrdersResolver
{
    Task Resolve(IEnumerable<E1HeldOrderModel> orders, BalticCountry country);
}
