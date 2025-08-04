using Tamro.Madam.Models.Commerce.ItemAssortmentSalesChannels.Import;
using Tamro.Madam.Models.General;

namespace Tamro.Madam.Models.Commerce.ItemAssortmentSalesChannels;

public class ItemAssortmentImportModel
{
    public BalticCountry Country { get; set; }
    public ItemAssortmentImportAction Action { get; set; }
    public string ItemNos { get; set; }
}
