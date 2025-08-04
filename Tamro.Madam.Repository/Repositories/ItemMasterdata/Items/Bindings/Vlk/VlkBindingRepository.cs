using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using Tamro.Madam.Repository.Common;
using Tamro.Madam.Repository.Entities.ItemMasterdata.Items.Bindings.Vlk;
using Tamro.Madam.Repository.UnitOfWork;

namespace Tamro.Madam.Repository.Repositories.ItemMasterdata.Items.Bindings.Vlk;

public class VlkBindingRepository : IVlkBindingRepository
{
    private readonly IMadamUnitOfWork _uow;

    public VlkBindingRepository(IMadamUnitOfWork uow)
    {
        _uow = uow;
    }

    public async Task<IEnumerable<VlkBinding>> GetList(Expression<Func<VlkBinding, bool>> filter, List<IncludeOperation<VlkBinding>>? includes = null, bool track = true, CancellationToken cancellationToken = default)
    {
        IQueryable<VlkBinding> query = _uow.GetRepository<VlkBinding>().AsQueryable();

        if (includes != null)
        {
            foreach (var include in includes)
            {
                query = include.ApplyInclude(query);
            }
        }

        query = query.Where(filter);

        if (!track)
        {
            query = query.AsNoTracking();
        }

        return await query.ToListAsync(cancellationToken);
    }
}
