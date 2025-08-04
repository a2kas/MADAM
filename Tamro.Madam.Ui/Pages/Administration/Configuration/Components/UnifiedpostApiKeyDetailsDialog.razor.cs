using Fluxor;
using MediatR;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using MudBlazor;
using Tamro.Madam.Application.Commands.Administration.Configuration.Ubl;
using Tamro.Madam.Application.Queries.Customers.Wholesale.Clsf;
using Tamro.Madam.Application.Validation;
using Tamro.Madam.Models.Administration.Configuration.Ubl;
using Tamro.Madam.Models.Common.Dialog;
using Tamro.Madam.Models.Customers.Wholesale;
using Tamro.Madam.Models.Customers.Wholesale.Clsf;
using Tamro.Madam.Models.General;
using Tamro.Madam.Ui.Store.State;

namespace Tamro.Madam.Ui.Pages.Administration.Configuration.Components;

public partial class UnifiedpostApiKeyDetailsDialog
{
    [EditorRequired]
    [Parameter]
    public UblApiKeyEditModel Model { get; set; } = new();
    [EditorRequired]
    [Parameter]
    public DialogState State { get; set; }
    [Inject]
    private IValidationService _validator { get; set; }
    [Inject]
    private IMediator _mediator { get; set; }
    [Inject]
    private IState<UserProfileState> _userProfileState { get; set; }
    [CascadingParameter]
    private IMudDialogInstance _mudDialog { get; set; } = default!;

    private bool _saving;
    private MudForm? _form;

    private async Task OnKeyDown(KeyboardEventArgs e)
    {
        if (e.Key == "Enter")
        {
            await Submit();
        }
    }

    private async Task<IEnumerable<WholesaleCustomerClsfModel>> SearchCustomers(string value, CancellationToken token)
    {
        try
        {
            var query = new WholesaleCustomerClsfQuery()
            {
                SearchTerm = value,
                Country = _userProfileState.Value.UserProfile.Country ?? BalticCountry.LV,
                CustomerType = WholesaleCustomerType.LegalEntity,
            };

            var result = await _mediator.Send(query, token);

            return result.Items;
        }
        catch (Exception)
        {
            Snackbar.Add("Failed to load customers", Severity.Error);
            return new List<WholesaleCustomerClsfModel>();
        }
    }

    private async Task Submit()
    {
        try
        {
            _saving = true;
            await _form!.Validate();
            if (!_form.IsValid)
            {
                return;
            }
            Model.Country = _userProfileState.Value.UserProfile.Country ?? BalticCountry.LV;
            var result = await _mediator.Send(new UpsertUblApiKeyCommand(Model));
            if (result.Succeeded)
            {
                Model.Customer.AddressNumber = result.Data;
                _mudDialog.Close(DialogResult.Ok(Model));
                Snackbar.Add(GetSuccessMessage(), Severity.Success);
            }
            else
            {
                Snackbar.Add(result.ErrorMessage, Severity.Error);
            }
        }
        finally
        {
            _saving = false;
        }
    }

    private void Cancel() => _mudDialog.Cancel();

    private string GetTitle()
    {
        return State switch
        {
            DialogState.Create => $"Create new UnifiedPost API key",
            DialogState.View => $"UnifiedPost API key",
            _ => "",
        };
    }

    private string GetSuccessMessage()
    {
        return State switch
        {
            DialogState.Create => $"UnifiedPost API key for {Model.Customer.DisplayName} created",
            DialogState.View => $"UnifiedPost API key for {Model.Customer.DisplayName} updated",
            _ => "",
        };
    }
}
