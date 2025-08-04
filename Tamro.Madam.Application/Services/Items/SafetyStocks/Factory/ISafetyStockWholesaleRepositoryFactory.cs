using Tamro.Madam.Models.General;
using Tamro.Madam.Repository.Repositories.ItemMasterdata.Items.SafetyStocks;

namespace Tamro.Madam.Application.Services.Items.SafetyStocks.Factory;

public interface ISafetyStockWholesaleRepositoryFactory
{
    ISafetyStockWholesaleRepository Get(BalticCountry country);
}
