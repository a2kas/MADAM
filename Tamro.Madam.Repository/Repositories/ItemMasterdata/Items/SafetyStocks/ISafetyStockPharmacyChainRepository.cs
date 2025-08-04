using Tamro.Madam.Models.ItemMasterdata.Items.SafetyStocks.PharmacyChains;

namespace Tamro.Madam.Repository.Repositories.ItemMasterdata.Items.SafetyStocks;

public interface ISafetyStockPharmacyChainRepository
{
    Task<IEnumerable<PharmacyChainModel>> GetAll();
}
