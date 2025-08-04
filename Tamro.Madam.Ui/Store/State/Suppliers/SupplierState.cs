using Fluxor;
using Tamro.Madam.Models.Suppliers;

namespace Tamro.Madam.Ui.Store.State.Suppliers;

[FeatureState]
public class SupplierState
{
    public SupplierState()
    {
        ActiveForm = SupplierForm.Grid;
    }

    public SupplierState(SupplierForm form, SupplierDetailsModel model)
    {
        ActiveForm = form;
        Model = model; 
    }

    public SupplierForm ActiveForm { get; set; }
    public SupplierDetailsModel Model { get; set; }
}
