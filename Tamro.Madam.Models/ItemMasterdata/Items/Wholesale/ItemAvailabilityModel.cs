using Tamro.Madam.Models.General;

namespace Tamro.Madam.Models.ItemMasterdata.Items.Wholesale;

public class ItemAvailabilityModel
{
    public BalticCountry Country { get; set; }
    public string ItemNo { get; set; }
    public int AvailableQuantity { get; set; }
}
