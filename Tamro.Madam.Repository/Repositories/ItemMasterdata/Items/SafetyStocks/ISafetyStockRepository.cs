using Tamro.Madam.Models.ItemMasterdata.Items.SafetyStocks;

namespace Tamro.Madam.Repository.Repositories.ItemMasterdata.Items.SafetyStocks;

public interface ISafetyStockRepository
{
    Task<IEnumerable<SafetyStockModel>> UpsertBulkRange(IEnumerable<SafetyStockModel> models);
}
