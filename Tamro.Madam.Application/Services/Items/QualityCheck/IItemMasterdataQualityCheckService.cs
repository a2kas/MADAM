using Tamro.Madam.Models.ItemMasterdata.Items.QualityCheck;
using Tamro.Madam.Repository.Entities.ItemMasterdata.Items;
using Tamro.Madam.Repository.Entities.ItemMasterdata.Items.QualityCheck;

namespace Tamro.Madam.Application.Services.Items.QualityCheck;

public interface IItemMasterdataQualityCheckService
{
    Task<IEnumerable<Item>> GetItems();
    ItemQualityCheck PerformQualityCheck(ItemQualityCheckReferenceModel reference, ItemQualityCheckApiResponseModel response);
}
