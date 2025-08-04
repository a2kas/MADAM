using Tamro.Madam.Models.Suppliers;

namespace Tamro.Madam.Ui.Store.Actions.Suppliers;

public class SetActiveFormAction
{
    public SupplierForm ActiveForm { get; }
    public SupplierDetailsModel Model { get; set; }

    public SetActiveFormAction(SupplierForm activeForm, SupplierDetailsModel detailsModel = null)
    {
        ActiveForm = activeForm;

        if (detailsModel == null)
        {
            Model = new SupplierDetailsModel();
        }
        else
        {
            Model = detailsModel;
        }
    }
}
