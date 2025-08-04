using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Tamro.Madam.Models.General;
using Tamro.Madam.Models.ItemMasterdata.Items.SafetyStocks;
using Tamro.Madam.Repository.Context.Madam;
using TamroUtilities.EFCore.Models;

namespace Tamro.Madam.Repository.Entities.ItemMasterdata.Items.SafetyStocks;

[Table("SafetyStockCondition")]
public class SafetyStockCondition : IMadamEntity<int>, IAuditable, IBaseEntity
{
    [Key]
    public int Id { get; set; }
    public int SafetyStockItemId { get; set; }
    public int? SafetyStockPharmacyChainId { get; set; }
    public bool CanBuy { get; set; }
    public string? Comment { get; set; }
    public string User { get; set; }
    public SafetyStockRestrictionLevel? RestrictionLevel { get; set; }
    public PharmacyGroup? SafetyStockPharmacyChainGroup { get; set; }
    public DateTime CreatedDate { get; set; }
    public DateTime RowVer { get; set; }

    public SafetyStockItem SafetyStockItem { get; set; }
    public SafetyStockPharmacyChain SafetyStockPharmacyChain { get; set; }
}
