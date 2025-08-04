using Tamro.Madam.Models.ItemMasterdata.Barcodes;
using Tamro.Madam.Ui.Store.State;

namespace Tamro.Madam.Ui.Store.Actions.ItemMasterdata.Items;

public class SetItemBarcodesAction
{
    public IEnumerable<BarcodeModel> Barcodes { get; set; }
    public UserProfileState UserProfileState { get; set; }
}
