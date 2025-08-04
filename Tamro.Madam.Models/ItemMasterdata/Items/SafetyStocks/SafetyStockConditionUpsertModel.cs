using Tamro.Madam.Models.General;

namespace Tamro.Madam.Models.ItemMasterdata.Items.SafetyStocks;

public class SafetyStockConditionUpsertModel
{
    public int Id { get; set; }
    public bool? CanBuy { get; set; }
    public int? CheckDays { get; set; }
    public string? Comment { get; set; }
    public PharmacyGroup? PharmacyGroup { get; set; }
    public int? PharmacyChainId { get; set; }
    public string PharmacyChainName { get; set; }
}
