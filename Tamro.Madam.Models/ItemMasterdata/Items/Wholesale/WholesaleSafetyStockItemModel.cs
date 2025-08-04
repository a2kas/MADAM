using System.ComponentModel;

namespace Tamro.Madam.Models.ItemMasterdata.Items.Wholesale;

public class WholesaleSafetyStockItemModel
{
    [DisplayName("Item No.")]
    public string ItemNo { get; set; }
    [DisplayName("Item name")]
    public string ItemName { get; set; }
    [DisplayName("Item group")]
    public string ItemGroup { get; set; }
    [DisplayName("Product class")]
    public string ProductClass { get; set; }
    public string Brand { get; set; }
    [DisplayName("Supplier number")]
    public int SupplierNumber { get; set; }
    [DisplayName("Supplier nick")]
    public string SupplierNick { get; set; }
    [DisplayName("BCat3")]
    public string? Cn3 { get; set; }
    [DisplayName("BCat1")]
    public string? Cn1 { get; set; }
    public string Substance { get; set; }
    public decimal? RtlTransQty { get; set; }
}
