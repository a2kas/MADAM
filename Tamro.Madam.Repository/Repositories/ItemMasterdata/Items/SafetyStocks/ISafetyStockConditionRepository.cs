using Tamro.Madam.Models.ItemMasterdata.Items.SafetyStocks;

namespace Tamro.Madam.Repository.Repositories.ItemMasterdata.Items.SafetyStocks;

public interface ISafetyStockConditionRepository
{
    Task<SafetyStockConditionModel> Get(int id);
    Task<SafetyStockConditionModel> Upsert(SafetyStockConditionModel model);
    Task<int> DeleteMany(int[] ids, CancellationToken cancellationToken);
}
