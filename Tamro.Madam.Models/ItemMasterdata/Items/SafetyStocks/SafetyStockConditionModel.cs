using Tamro.Madam.Models.General;

namespace Tamro.Madam.Models.ItemMasterdata.Items.SafetyStocks;

public class SafetyStockConditionModel
{
    public int Id { get; set; }
    public int SafetyStockItemId { get; set; }
    public int? SafetyStockPharmacyChainId { get; set; }
    public bool CanBuy { get; set; }
    public int CheckDays { get; set; }
    public string Comment { get; set; }
    public string User { get; set; }
    public SafetyStockRestrictionLevel? RestrictionLevel { get; set; }
    public PharmacyGroup? SafetyStockPharmacyChainGroup { get; set; }
    public DateTime CreatedDate { get; set; }
    public DateTime RowVer { get; set; }
}
