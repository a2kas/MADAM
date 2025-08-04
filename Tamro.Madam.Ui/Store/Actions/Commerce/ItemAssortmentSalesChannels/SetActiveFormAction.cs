using Tamro.Madam.Models.Commerce.ItemAssortmentSalesChannels;

namespace Tamro.Madam.Ui.Store.Actions.Commerce.ItemAssortmentSalesChannels;

public class SetActiveFormAction
{
    public ItemAssortmentSalesChannelForm ActiveForm { get; }
    public ItemAssortmentSalesChannelDetailsModel Model { get; set; }

    public SetActiveFormAction(ItemAssortmentSalesChannelForm activeForm, ItemAssortmentSalesChannelDetailsModel detailsModel = null)
    {
        ActiveForm = activeForm;

        if (detailsModel == null)
        {
            Model = new ItemAssortmentSalesChannelDetailsModel();
        }
        else
        {
            Model = detailsModel;
        }
    }
}
