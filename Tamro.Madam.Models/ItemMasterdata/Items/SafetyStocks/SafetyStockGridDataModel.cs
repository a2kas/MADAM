using System.ComponentModel;
using Tamro.Madam.Models.Common;
using Tamro.Madam.Models.General;

namespace Tamro.Madam.Models.ItemMasterdata.Items.SafetyStocks;

public class SafetyStockGridDataModel : BaseDataGridModel<SafetyStockGridDataModel>
{
    public int Id { get; set; }
    [DisplayName("Can buy")]
    public bool CanBuy { get; set; }
    [DisplayName("Pharmacy group")]
    public string? SafetyStockPharmacyChainGroup { get; set; }
    [DisplayName("Pharmacy chain")]
    public string? PharmacyChainDisplayName { get; set; }
    [DisplayName("Pharmacy chain")]
    public int? PharmacyChainId { get; set; }
    [DisplayName("Item No.")]
    public string? ItemNo { get; set; }
    [DisplayName("Item name")]
    public string? ItemName { get; set; }
    [DisplayName("Check days")]
    public int CheckDays { get; set; }
    [DisplayName("Wholesale stock")]
    public int? WholesaleQuantity { get; set; }
    [DisplayName("Benu sales")]
    public decimal? RetailQuantity { get; set; }
    [DisplayName("Quantity to buy")]
    public int? QuantityToBuy { get; set; }
    [DisplayName("Item group")]
    public string? ItemGroup { get; set; }
    [DisplayName("Product class")]
    public string? ProductClass { get; set; }
    public string Brand { get; set; }
    [DisplayName("Supplier number")]
    public int? SupplierNumber { get; set; }
    [DisplayName("Supplier nick")]
    public string? SupplierNick { get; set; }
    [DisplayName("BCat3")]
    public string? Cn3 { get; set; }
    [DisplayName("BCat1")]
    public string? Cn1 { get; set; }
    public string? Substance { get; set; }
    public string? Comment { get; set; }
    public BalticCountry Country { get; set; }
}
