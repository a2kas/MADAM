using Tamro.Madam.Models.Sales.HeldOrders;

namespace Tamro.Madam.Application.Services.Sales.HeldOrders.Resolvers;
public interface IE1HeldOrdersEmailStatusResolver
{
    void ResolveNotifcationStatus(IEnumerable<E1HeldOrderModel> orders);
}
