using System.ComponentModel;

namespace Tamro.Madam.Models.Commerce.ItemAssortmentSalesChannels;

public class ItemAssortmentGridModel
{
    public int Id { get; set; }
    public int ItemBindingId { get; set; }
    public int ItemAssortmentSalesChannelId { get; set; }
    [DisplayName("Item code")]
    public string ItemCode { get; set; }
    [DisplayName("Item name")]
    public string ItemName { get; set; }
}
