using Tamro.Madam.Models.General;
using Tamro.Madam.Models.Sales.HeldOrders;

namespace Tamro.Madam.Application.Services.Sales.HeldOrders;
public interface IE1HeldOrderService
{
    Task<IEnumerable<E1HeldOrderModel>> GetHeldOrders(BalticCountry country, List<E1HeldNotificationStatusModel> statuses);
    Task<E1HeldOrderModel> Update(E1HeldOrderModel order);
    Task<IEnumerable<E1HeldOrderModel>> UpdateMany(IEnumerable<E1HeldOrderModel> orders);
}
