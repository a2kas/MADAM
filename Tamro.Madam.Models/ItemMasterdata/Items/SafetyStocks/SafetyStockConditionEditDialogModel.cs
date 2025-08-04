using System.ComponentModel;

namespace Tamro.Madam.Models.ItemMasterdata.Items.SafetyStocks;

public class SafetyStockConditionEditDialogModel : SafetyStockGridDataModel
{
    [DisplayName("Item")]
    public string Item { get { return $"{ItemNo} - {ItemName}"; } }
}
