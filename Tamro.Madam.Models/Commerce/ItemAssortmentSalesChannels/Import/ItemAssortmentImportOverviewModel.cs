using System.ComponentModel;

namespace Tamro.Madam.Models.Commerce.ItemAssortmentSalesChannels.Import;

public class ItemAssortmentImportOverviewModel
{
    [DisplayName("Item No.")]
    public string ItemNo { get; set; }
    [DisplayName("Item Name")]
    public string ItemName { get; set; }
    [DisplayName("Result")]
    public string Comment { get; set; }
    public bool IsSuccess { get; set; }
}
