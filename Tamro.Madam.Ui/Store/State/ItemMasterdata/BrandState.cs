using Fluxor;
using Tamro.Madam.Models.ItemMasterdata.Brands;

namespace Tamro.Madam.Ui.Store.State.ItemMasterdata;

[FeatureState]
public class BrandState
{
    public BrandState() : this(new())
    {
    }

    public BrandState(BrandDeleteDialogModel deleteDialogState)
    {
        DeleteDialogState = deleteDialogState;
    }

    public BrandDeleteDialogModel DeleteDialogState { get; }
}
