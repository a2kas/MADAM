using System.ComponentModel;
using Tamro.Madam.Models.Common;

namespace Tamro.Madam.Models.Commerce.ItemAssortmentSalesChannels;

public class ItemAssortmentSalesChannelGridModel : BaseDataGridModel<ItemAssortmentSalesChannelGridModel>
{
    public int Id { get; set; }
    [DisplayName("Name")]
    public string Name { get; set; }
    [DisplayName("Amount of items")]
    public int ItemsCount { get; set; }
}
