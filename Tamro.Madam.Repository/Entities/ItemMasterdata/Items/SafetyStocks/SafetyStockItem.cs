using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Tamro.Madam.Models.General;
using Tamro.Madam.Repository.Context.Madam;
using TamroUtilities.EFCore.Models;

namespace Tamro.Madam.Repository.Entities.ItemMasterdata.Items.SafetyStocks;

[Table("SafetyStockItem")]
public class SafetyStockItem : IMadamEntity<int>, IAuditable
{
    [Key]
    public int Id { get; set; }
    public BalticCountry Country { get; set; }
    public string ItemNo { get; set; }
    public string? ItemName { get; set; }
    public string? ItemGroup { get; set; }
    public string? ProductClass { get; set; }
    public string? Brand { get; set; }
    public int? SupplierNumber { get; set; }
    public string? SupplierNick { get; set; }
    public string? Cn3 { get; set; }
    public string? Cn1 { get; set; }
    public string Substance { get; set; }
    public int CheckDays { get; set; }

    public IEnumerable<SafetyStockCondition> SafetyStockConditions { get; set; }
    public SafetyStock SafetyStock { get; set; }
}
