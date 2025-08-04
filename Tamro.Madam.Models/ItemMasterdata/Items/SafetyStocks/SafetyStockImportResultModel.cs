using System.ComponentModel;

namespace Tamro.Madam.Models.ItemMasterdata.Items.SafetyStocks;

public class SafetyStockImportResultModel
{
    public SafetyStockGridDataModel SafetyStock { get; set; }
    [DisplayName("Result")]
    public bool IsImported { get; set; }
    public string Message { get; set; }
}
