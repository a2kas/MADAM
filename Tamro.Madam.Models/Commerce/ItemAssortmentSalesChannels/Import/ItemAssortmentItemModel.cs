using System.ComponentModel;

namespace Tamro.Madam.Models.Commerce.ItemAssortmentSalesChannels.Import;

public class ItemAssortmentItemModel
{
    [DisplayName("Item No.")]
    public string ItemNo { get; set; }
    [DisplayName("Item name")]
    public string ItemName { get; set; }
    public int ItemBindingId { get; set; }
}
