using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Tamro.Madam.Models.General;
using Tamro.Madam.Models.Sales.CanceledOrderLines;
using Tamro.Madam.Repository.Context.E1Gateway;
using Tamro.Madam.Repository.Entities.Sales.CanceledOrderLines;
using TamroUtilities.EFCore.Repository;

namespace Tamro.Madam.Repository.Repositories.Sales;

public class E1CanceledOrderRepository : BaseRepository<CanceledOrderHeaderModel, E1CanceledOrderHeader, E1GatewayDbContext>, IE1CanceledOrderRepository
{
    public E1CanceledOrderRepository(E1GatewayDbContext e1DatabaseContext, IMapper mapper) : base(e1DatabaseContext, mapper)
    {
    }

    public async Task<IEnumerable<CanceledOrderHeaderModel>> GetCanceledOrders(BalticCountry country, List<CanceledOrderLineEmailStatus> statuses)
    {
        var query = DbContext.E1CanceledOrderHeaders
            .AsNoTracking()
            .Where(x => x.Country == country && x.Lines.Any(y => statuses.Contains(y.EmailStatus.Value)))
            .Select(order => new E1CanceledOrderHeader
            {
                Id = order.Id,
                Country = order.Country,
                OrderDate = order.OrderDate,
                E1ShipTo = order.E1ShipTo,
                CustomerOrderNo = order.CustomerOrderNo,
                DocumentNo = order.DocumentNo,
                CreatedDate = order.CreatedDate,
                RowVer = order.RowVer,
                Lines = order.Lines.Where(line => statuses.Contains(line.EmailStatus.Value)).ToList()
            });

        var result = await query.ToListAsync();

        return _mapper.Map<IEnumerable<CanceledOrderHeaderModel>>(result);
    }

    protected override Task<E1CanceledOrderHeader> GetEntity(CanceledOrderHeaderModel model)
    {
        return DbContext.E1CanceledOrderHeaders
            .Include(x => x.Lines)
            .SingleOrDefaultAsync(x => x.Id == model.Id);
    }
}
