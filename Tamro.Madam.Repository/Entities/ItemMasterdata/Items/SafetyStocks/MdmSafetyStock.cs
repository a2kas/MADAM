using System.ComponentModel.DataAnnotations.Schema;

namespace Tamro.Madam.Repository.Entities.ItemMasterdata.Items.SafetyStocks;

[Obsolete("To be removed after SafetyStock is moved to new system")]
public class MdmSafetyStock
{
    [Column("CanBuy", TypeName = "int")]
    public bool CanBuy { get; set; }
    public string PharmacyChain { get; set; }
    public string? ItemNo { get; set; }
    public string? ItemName { get; set; }
    public decimal RtlTransQty { get; set; }
    public int WhslAvailQty { get; set; }
    public int FinalAvailQty { get; set; }
    public string? ItemGroup { get; set; }
    public string? ProductClass { get; set; }
    public string Brand { get; set; }
    public int? SupplierNumber { get; set; }
    public string? SupplierNick { get; set; }
    public string? Cn3 { get; set; }
    public string? Cn1 { get; set; }
    public string? Substance { get; set; }
    public string Comment { get; set; }
    public string WindowsUser { get; set; }
    public DateTime DStamp { get; set; }
}
