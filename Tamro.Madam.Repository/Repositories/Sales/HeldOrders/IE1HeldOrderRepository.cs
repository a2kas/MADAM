using Tamro.Madam.Models.General;
using Tamro.Madam.Models.Sales.HeldOrders;

namespace Tamro.Madam.Repository.Repositories.Sales.HeldOrders;
public interface IE1HeldOrderRepository
{
    Task<IEnumerable<E1HeldOrderModel>> GetHeldOrders(BalticCountry country, List<E1HeldNotificationStatusModel> statuses);
    Task<E1HeldOrderModel> Update(E1HeldOrderModel order);
}
