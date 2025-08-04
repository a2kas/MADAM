using System.Linq.Expressions;
using Tamro.Madam.Repository.Common;
using Tamro.Madam.Repository.Entities.ItemMasterdata.Items;

namespace Tamro.Madam.Repository.Repositories.ItemMasterdata.Items;

public interface IItemRepository
{
    Task<Item?> Get(int id, List<IncludeOperation<Item>>? includes = null);
    Task<List<Item>> GetList(Expression<Func<Item, bool>> filter, List<IncludeOperation<Item>>? includes = null, bool track = true, int take = 0, CancellationToken cancellationToken = default);
    Task<List<Item>> UpdateMany(List<Item> items, CancellationToken cancellationToken);
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}
