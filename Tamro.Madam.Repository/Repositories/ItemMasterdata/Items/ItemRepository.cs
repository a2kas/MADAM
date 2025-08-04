using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using Tamro.Madam.Repository.Common;
using Tamro.Madam.Repository.Entities.ItemMasterdata.Items;
using Tamro.Madam.Repository.UnitOfWork;

namespace Tamro.Madam.Repository.Repositories.ItemMasterdata.Items;

public class ItemRepository : IItemRepository
{
    private readonly IMadamUnitOfWork _uow;
    public ItemRepository(IMadamUnitOfWork uow)
    {
        _uow = uow;
    }

    public async Task<Item?> Get(int id, List<IncludeOperation<Item>>? includes = null)
    {
        var query = _uow.GetRepository<Item>().AsReadOnlyQueryable();

        if (includes != null)
        {
            foreach (var include in includes)
            {
                query = include.ApplyInclude(query);
            }
        }

        return await query
            .SingleOrDefaultAsync(x => x.Id == id);
    }

    public async Task<List<Item>> GetList(Expression<Func<Item, bool>> filter, List<IncludeOperation<Item>>? includes = null, bool track = true, int take = 0, CancellationToken cancellationToken = default)
    {
        IQueryable<Item> query = _uow.GetRepository<Item>().AsQueryable();

        if (includes != null)
        {
            foreach (var include in includes)
            {
                query = include.ApplyInclude(query);
            }
        }

        query = query.Where(filter);

        if (take != default)
        {
            query = query.Take(take);
        }

        if (!track)
        {
            query = query.AsNoTracking();
        }

        return await query.ToListAsync(cancellationToken);
    }

    public async Task<List<Item>> UpdateMany(List<Item> items, CancellationToken cancellationToken)
    {
        await _uow.SaveChangesAsync(cancellationToken);
        return items;
    }

    public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        return await _uow.SaveChangesAsync(cancellationToken);
    }
}
