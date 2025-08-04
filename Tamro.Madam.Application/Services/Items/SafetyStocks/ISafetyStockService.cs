using Tamro.Madam.Models.General;
using Tamro.Madam.Models.ItemMasterdata.Items.SafetyStocks;

namespace Tamro.Madam.Application.Services.Items.SafetyStocks;

public interface ISafetyStockService
{
    Task<SafetyStockModel> GetSafetyStock(string itemNo2, int checkDays, BalticCountry country);
}
