using System.ComponentModel;
using Tamro.Madam.Models.General;

namespace Tamro.Madam.Models.Commerce.ItemAssortmentSalesChannels;

public class ItemAssortmentSalesChannelDetailsModel
{
    public int Id { get; set; }
    [DisplayName("Name")]
    public string Name { get; set; }
    public BalticCountry Country { get; set; }
    public IEnumerable<ItemAssortmentGridModel> Assortment { get; set; } = new List<ItemAssortmentGridModel>();
}
