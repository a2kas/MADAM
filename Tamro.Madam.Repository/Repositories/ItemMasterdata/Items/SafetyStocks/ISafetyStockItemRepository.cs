using EFCore.BulkExtensions;
using System.Linq.Expressions;
using Tamro.Madam.Models.ItemMasterdata.Items.SafetyStocks;
using Tamro.Madam.Repository.Common;
using Tamro.Madam.Repository.Entities.ItemMasterdata.Items.SafetyStocks;

namespace Tamro.Madam.Repository.Repositories.ItemMasterdata.Items.SafetyStocks;

public interface ISafetyStockItemRepository
{
    Task<IEnumerable<SafetyStockItemModel>> GetMany(Expression<Func<SafetyStockItem, bool>> filter, List<IncludeOperation<SafetyStockItem>>? includes = null, bool track = true, CancellationToken cancellationToken = default);
    Task<SafetyStockItemModel> Get(Expression<Func<SafetyStockItem, bool>> filter, List<IncludeOperation<SafetyStockItem>>? includes = null, bool track = true, CancellationToken cancellationToken = default);
    Task<IEnumerable<SafetyStockItemModel>> CreateRange(IEnumerable<SafetyStockItemModel> models);
    Task<SafetyStockItemModel> UpsertGraph(SafetyStockItemModel model);
    Task<IEnumerable<SafetyStockItemModel>> UpsertBulkRange(IEnumerable<SafetyStockItemModel> models, BulkConfig config);
    Task<int> DeleteMany(Expression<Func<SafetyStockItem, bool>> filter, CancellationToken cancellationToken);
}
