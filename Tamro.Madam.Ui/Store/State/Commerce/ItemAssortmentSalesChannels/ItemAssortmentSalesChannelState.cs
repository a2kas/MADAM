using Fluxor;
using Tamro.Madam.Models.Commerce.ItemAssortmentSalesChannels;

namespace Tamro.Madam.Ui.Store.State.Commerce.ItemAssortmentSalesChannels;

[FeatureState]
public class ItemAssortmentSalesChannelState
{
    public ItemAssortmentSalesChannelState()
    {
        ActiveForm = ItemAssortmentSalesChannelForm.Grid;
    }

    public ItemAssortmentSalesChannelState(ItemAssortmentSalesChannelForm form, ItemAssortmentSalesChannelDetailsModel model)
    {
        ActiveForm = form;
        Model = model;
    }

    public ItemAssortmentSalesChannelForm ActiveForm { get; set; }
    public ItemAssortmentSalesChannelDetailsModel Model { get; set; } = new();
}