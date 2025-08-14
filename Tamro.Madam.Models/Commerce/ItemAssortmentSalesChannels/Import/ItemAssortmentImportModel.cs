using Tamro.Madam.Models.Commerce.ItemAssortmentSalesChannels.Import;
using Tamro.Madam.Models.General;

namespace Tamro.Madam.Models.Commerce.ItemAssortmentSalesChannels;

public class ItemAssortmentImportModel
{
    public BalticCountry Country { get; set; }
    private ItemAssortmentImportAction _action;
    public ItemAssortmentImportAction Action 
    { 
        get { return _action; }
        set 
        { 
            _action = value; 
        }
    }
    public string ItemNos { get; set; }
}
