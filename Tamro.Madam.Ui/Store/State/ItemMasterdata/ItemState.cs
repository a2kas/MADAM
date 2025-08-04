using Fluxor;
using Tamro.Madam.Application.Access;
using Tamro.Madam.Models.Common.Dialog;
using Tamro.Madam.Models.ItemMasterdata.Items;

namespace Tamro.Madam.Ui.Store.State.ItemMasterdata;

[FeatureState]
public class ItemState
{
    public ItemState()
    {
        Item = new ItemModel();
    }

    public ItemState(ItemModel item, DialogState dialogState) : this(item, dialogState, new UserProfileState())
    {
    }

    public ItemState(ItemModel item, UserProfileState userProfileState) : this(item, DialogState.View, userProfileState)
    {
    }

    public ItemState(ItemModel item, DialogState dialogState, UserProfileState userProfileState)
    {
        Item = item;
        DialogState = dialogState;
        IsEditable = userProfileState.HasPermission(Permissions.CanEditCoreMasterdata) &&
                     (dialogState == DialogState.View ||
                      dialogState == DialogState.Copy ||
                      dialogState == DialogState.Create && !item.IsParallel);
        IsDescriptionEditable = userProfileState.HasPermission(Permissions.CanEditCoreMasterdata) && (item.IsParallel ? dialogState == DialogState.Copy : true);
        IsTabsActive = dialogState == DialogState.View;
    }

    public ItemModel Item { get; }
    public DialogState DialogState { get; }
    public bool IsEditable { get; }
    public bool IsTabsActive { get; }
    public bool IsDescriptionEditable { get; }
}
