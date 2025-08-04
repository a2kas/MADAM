using Fluxor;
using Microsoft.AspNetCore.Components;
using Tamro.Madam.Ui.Store.Actions.Commerce.ItemAssortmentSalesChannels;
using Tamro.Madam.Ui.Store.State.Commerce.ItemAssortmentSalesChannels;

namespace Tamro.Madam.Ui.Pages.Commerce.ItemAssortmentSalesChannels;

public partial class ItemAssortmentSalesChannels
{
    [Inject]
    private IState<ItemAssortmentSalesChannelState> _state { get; set; }
    [Inject]
    private IActionSubscriber _actionSubscriber { get; set; }

    protected override async Task OnInitializedAsync()
    {
        _actionSubscriber?.SubscribeToAction<SetActiveFormAction>(this, _ => StateHasChanged());
    }
}
