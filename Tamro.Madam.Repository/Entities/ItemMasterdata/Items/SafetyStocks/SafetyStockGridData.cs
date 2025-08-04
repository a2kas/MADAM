using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Tamro.Madam.Models.General;
using Tamro.Madam.Repository.Context.Madam;

namespace Tamro.Madam.Repository.Entities.ItemMasterdata.Items.SafetyStocks;

[Table("v_SafetyStockGrid")]
public class SafetyStockGridData : IMadamEntity
{
    [Key]
    public int Id { get; set; }
    public bool CanBuy { get; set; }
    public string? SafetyStockPharmacyChainGroup { get; set; }
    public string? PharmacyChainDisplayName { get; set; }
    public int? PharmacyChainId { get; set; }
    public string? ItemNo { get; set; }
    public string? ItemName { get; set; }
    public int CheckDays { get; set; }
    public int? WholesaleQuantity { get; set; }
    public decimal? RetailQuantity { get; set; }
    public int? QuantityToBuy { get; set; }
    public string? ItemGroup { get; set; }
    public string? ProductClass { get; set; }
    public string Brand { get; set; }
    public int? SupplierNumber { get; set; }
    public string? SupplierNick { get; set; }
    public string? Cn3 { get; set; }
    public string? Cn1 { get; set; }
    public string? Substance { get; set; }
    public string? Comment { get; set; }
    public BalticCountry Country { get; set; }
}
