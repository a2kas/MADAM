using Tamro.Madam.Models.Common.Dialog;
using Tamro.Madam.Models.ItemMasterdata.Items;
using Tamro.Madam.Ui.Store.State;

namespace Tamro.Madam.Ui.Store.Actions.ItemMasterdata.Items;

public class SetCurrentItemAction
{
    public ItemModel? Item { get; set; }
    public DialogState State { get; set; }
    public UserProfileState UserProfileState { get; set; }
}
