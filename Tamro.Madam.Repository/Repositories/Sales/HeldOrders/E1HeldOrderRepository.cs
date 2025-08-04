using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Tamro.Madam.Models.General;
using Tamro.Madam.Models.Sales.HeldOrders;
using Tamro.Madam.Repository.Context.E1Gateway;
using Tamro.Madam.Repository.Entities.Sales.HeldOrders;
using TamroUtilities.EFCore.Repository;

namespace Tamro.Madam.Repository.Repositories.Sales.HeldOrders;
public class E1HeldOrderRepository : BaseRepository<E1HeldOrderModel, E1HeldOrder, E1GatewayDbContext>, IE1HeldOrderRepository
{
    public E1HeldOrderRepository(E1GatewayDbContext context, IMapper mapper) : base(context, mapper)
    {
    }

    public async Task<IEnumerable<E1HeldOrderModel>> GetHeldOrders(BalticCountry country, List<E1HeldNotificationStatusModel> statuses)
    {
        var query = DbContext.E1HeldOrder
            .AsNoTracking()
            .Where(x => x.Country == country && statuses.Contains(x.NotificationStatus));

        var result = await query.ToListAsync();

        return _mapper.Map<IEnumerable<E1HeldOrderModel>>(result);
    }

    protected override Task<E1HeldOrder> GetEntity(E1HeldOrderModel model)
    {
        return DbContext.E1HeldOrder.SingleOrDefaultAsync(x => x.Id == model.Id);
    }
}
