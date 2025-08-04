using Fluxor;
using MediatR;
using Microsoft.AspNetCore.Components;
using MudBlazor;
using Tamro.Madam.Application.Services.Sales.CanceledOrderLines.Decorators;
using Tamro.Madam.Models.General;
using Tamro.Madam.Models.Sales.CanceledOrderLines;
using Tamro.Madam.Ui.Store.State;

namespace Tamro.Madam.Ui.Pages.Sales.CanceledOrderLines.Components;

public partial class CanceledOrderHeaderDetailsGrid
{
    [EditorRequired]
    [Parameter]
    public List<CanceledOrderLineGridModel> Lines { get; set; }

    [Inject]
    private IMediator _mediator { get; set; }
    [Inject]
    private IState<UserProfileState> _userProfileState { get; set; }
    [Inject]
    private ICanceledOrderLineItemDecorator _canceledOrderLineItemDecorator { get; set; }

    protected override async Task OnInitializedAsync()
    {
        try
        {
            await _canceledOrderLineItemDecorator.Decorate(Lines, _userProfileState.Value.UserProfile.Country ?? BalticCountry.LV);
        }
        catch (Exception)
        {
            Snackbar.Add("Failed to load items", Severity.Error);
        }
    }
}
