using Fluxor;
using Tamro.Madam.Ui.Store.Actions.Suppliers;
using Tamro.Madam.Ui.Store.State.Suppliers;

namespace Tamro.Madam.Ui.Store.Reducers.Suppliers;

public class SupplierReducers
{
    [ReducerMethod]
    public static SupplierState SetActiveFormAction(SupplierState state, SetActiveFormAction action)
    {
        var newState = new SupplierState(state.ActiveForm, state.Model)
        {
            ActiveForm = action.ActiveForm,
            Model = action.Model,
        };

        return newState;
    }

    [ReducerMethod]
    public static SupplierState RefreshGridAction(SupplierState state, RefreshGridAction action)
    {
        return state;
    }
}
