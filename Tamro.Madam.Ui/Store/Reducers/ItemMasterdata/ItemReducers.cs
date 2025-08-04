using Fluxor;
using Tamro.Madam.Ui.Store.Actions.ItemMasterdata.Items;
using Tamro.Madam.Ui.Store.State.ItemMasterdata;

namespace Tamro.Madam.Ui.Store.Reducers;

public class ItemReducers
{
    [ReducerMethod]
    public static ItemState ReduceSetCurrentItemAction(ItemState itemState, SetCurrentItemAction action)
    {
        return new ItemState(action.Item, action.State, action.UserProfileState);
    }

    [ReducerMethod]
    public static ItemState ReduceSetItemBarcodesAction(ItemState itemState, SetItemBarcodesAction action)
    {
        var newItemState = new ItemState(itemState.Item, action.UserProfileState);
        newItemState.Item.Barcodes = action.Barcodes;

        return newItemState;
    }

    [ReducerMethod]
    public static ItemState ReduceSetItemBindingsAction(ItemState itemState, SetItemBindingsAction action)
    {
        var newItemState = new ItemState(itemState.Item, action.UserProfileState);
        newItemState.Item.Bindings = action.Bindings;

        return newItemState;
    }
}
