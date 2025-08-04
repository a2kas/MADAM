using Fluxor;
using Tamro.Madam.Ui.Store.Actions.Commerce.ItemAssortmentSalesChannels;
using Tamro.Madam.Ui.Store.State.Commerce.ItemAssortmentSalesChannels;

namespace Tamro.Madam.Ui.Store.Reducers.Commerce.ItemAssortmentSalesChannels;

public class ItemAssortmentSalesChannelReducers
{
    [ReducerMethod]
    public static ItemAssortmentSalesChannelState SetActiveFormAction(ItemAssortmentSalesChannelState state, SetActiveFormAction action)
    {
        var newState = new ItemAssortmentSalesChannelState(state.ActiveForm, state.Model)
        {
            ActiveForm = action.ActiveForm,
            Model = action.Model,
        };

        return newState;
    }

    [ReducerMethod]
    public static ItemAssortmentSalesChannelState RefreshGridAction(ItemAssortmentSalesChannelState state, RefreshGridAction action)
    {
        return state;
    }
}
