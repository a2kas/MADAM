using Tamro.Madam.Models.General;

namespace Tamro.Madam.Models.ItemMasterdata.Items.SafetyStocks;

public class SafetyStockItemModel
{
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

    public IEnumerable<SafetyStockConditionModel> SafetyStockConditions { get; set; } = [];
    public SafetyStockModel SafetyStock { get; set; } = new();
}
