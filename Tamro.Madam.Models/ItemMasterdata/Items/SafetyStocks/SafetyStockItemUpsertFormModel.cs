using Tamro.Madam.Models.General;
using Tamro.Madam.Models.ItemMasterdata.Items.SafetyStocks.PharmacyChains;
using Tamro.Madam.Models.ItemMasterdata.Items.Wholesale;
using Tamro.Madam.Models.ItemMasterdata.Items.Wholesale.Clsf;
using Tamro.Madam.Models.State.General;

namespace Tamro.Madam.Models.ItemMasterdata.Items.SafetyStocks;

public class SafetyStockItemUpsertFormModel
{
    public BalticCountry Country { get; set; }
    public string? Comment { get; set; }
    public WholesaleItemClsfModel Item { get; set; } = new();
    public WholesaleSafetyStockItemModel ItemInfo { get; set; } = new();
    public SafetyStockRestrictionLevel RestrictionLevel { get; set; } = SafetyStockRestrictionLevel.PharmacyChainGroup;
    public List<PharmacyGroup> PharmacyGroups { get; set; } = [];
    public List<PharmacyChainModel> PharmacyChains { get; set; } = [];
    public bool IsSaveAttempted { get; set; }
    public UserProfileStateModel User { get; set; }
}
