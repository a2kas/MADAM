using Tamro.Madam.Models.General;
using Tamro.Madam.Models.Sales.HeldOrders;
using Tamro.Madam.Repository.Repositories.Sales.HeldOrders;

namespace Tamro.Madam.Application.Services.Sales.HeldOrders;
public class E1HeldOrderService : IE1HeldOrderService
{
    private readonly IE1HeldOrderRepository _e1HeldOrderRepository;

    public E1HeldOrderService(IE1HeldOrderRepository e1HeldOrderRepository)
    {
        _e1HeldOrderRepository = e1HeldOrderRepository ?? throw new ArgumentNullException(nameof(e1HeldOrderRepository));
    }

    public async Task<IEnumerable<E1HeldOrderModel>> GetHeldOrders(BalticCountry country, List<E1HeldNotificationStatusModel> statuses)
    {
        return await _e1HeldOrderRepository.GetHeldOrders(country, statuses);
    }

    public async Task<E1HeldOrderModel> Update(E1HeldOrderModel order)
    {
        return await _e1HeldOrderRepository.Update(order);
    }

    public async Task<IEnumerable<E1HeldOrderModel>> UpdateMany(IEnumerable<E1HeldOrderModel> orders)
    {
        var result = new List<E1HeldOrderModel>();

        foreach (var order in orders)
        {
            result.Add(await Update(order));
        }

        return result;
    }
}
