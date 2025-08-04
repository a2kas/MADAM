using System.ComponentModel;
using Tamro.Madam.Models.Common;
using Tamro.Madam.Models.General;

namespace Tamro.Madam.Models.ItemMasterdata.Items.SafetyStocks.PharmacyChains;

public class PharmacyChainModel : BaseDataGridModel<PharmacyChainModel>
{
    public int Id { get; set; }
    public BalticCountry Country { get; set; }
    [DisplayName("Display name")]
    public string DisplayName { get; set; }
    [DisplayName("E1 sold to")]
    public int E1SoldTo { get; set; }
    public PharmacyGroup Group { get; set; }
    [DisplayName("Active")]
    public bool IsActive { get; set; } = true;
}