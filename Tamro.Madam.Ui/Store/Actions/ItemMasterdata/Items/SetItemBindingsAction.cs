using Tamro.Madam.Models.ItemMasterdata.Items.Bindings;
using Tamro.Madam.Ui.Store.State;

namespace Tamro.Madam.Ui.Store.Actions.ItemMasterdata.Items;

public class SetItemBindingsAction
{
    public IEnumerable<ItemBindingModel> Bindings { get; set; }
    public UserProfileState UserProfileState { get; set; }
}
