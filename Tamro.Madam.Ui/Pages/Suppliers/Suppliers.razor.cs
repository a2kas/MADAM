using Fluxor;
using Microsoft.AspNetCore.Components;
using Tamro.Madam.Ui.Store.Actions.Suppliers;
using Tamro.Madam.Ui.Store.State.Suppliers;

namespace Tamro.Madam.Ui.Pages.Suppliers;

public partial class Suppliers
{
    [Inject]
    private IState<SupplierState> _state { get; set; }
    [Inject]
    private IActionSubscriber _actionSubscriber { get; set; }

    protected override async Task OnInitializedAsync()
    {
        _actionSubscriber?.SubscribeToAction<SetActiveFormAction>(this, _ => StateHasChanged());
    }
}
