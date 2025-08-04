using MediatR;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components;
using MudBlazor;
using Tamro.Madam.Application.Validation;
using Tamro.Madam.Models.Common.Dialog;
using Tamro.Madam.Application.Commands.ItemMasterdata.Items.SafetyStocks.PharmacyChains;
using Fluxor;
using Tamro.Madam.Ui.Store.State;
using Tamro.Madam.Models.General;
using Tamro.Madam.Models.ItemMasterdata.Items.SafetyStocks.PharmacyChains;

namespace Tamro.Madam.Ui.Pages.SafetyStock.PharmacyChains.Components;

public partial class PharmacyChainsDetailsDialog
{
    [EditorRequired]
    [Parameter]
    public PharmacyChainModel Model { get; set; }
    [EditorRequired]
    [Parameter]
    public DialogState State { get; set; }

    [Inject]
    private IValidationService _validator { get; set; }
    [Inject]
    private IState<UserProfileState> _userProfileState { get; set; }
    [Inject]
    private IMediator _mediator { get; set; }
    [CascadingParameter]
    private IMudDialogInstance _mudDialog { get; set; } = default!;

    private bool _saving;
    private MudForm? _form;
    private List<PharmacyGroup> _pharmacyGroups = Enum.GetValues(typeof(PharmacyGroup)).Cast<PharmacyGroup>().Where(x => x != PharmacyGroup.All).ToList();

    private async Task OnKeyDown(KeyboardEventArgs e)
    {
        if (e.Key == "Enter")
        {
            await Submit();
        }
    }

    private async Task Submit()
    {
        try
        {
            Model.Country = _userProfileState.Value.UserProfile.Country ?? default;
            _saving = true;
            await _form!.Validate();

            if (!_form.IsValid)
            {
                return;
            }

            var result = await _mediator.Send(new UpsertPharmacyChainCommand(Model));

            if (result.Succeeded)
            {
                _mudDialog.Close(DialogResult.Ok(true));
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

    private string GetTitle()
    {
        return State switch
        {
            DialogState.Create => $"Create new pharmacy chain",
            DialogState.View => $"Pharmacy chain",
            DialogState.Copy => $"Copy pharmacy chain",
            _ => "",
        };
    }

    private string GetSuccessMessage()
    {
        return State switch
        {
            DialogState.Create or DialogState.Copy => $"Pharmacy chain {Model.DisplayName} created",
            DialogState.View => $"Pharmacy chain {Model.DisplayName} updated",
            _ => "",
        };
    }

    private void Cancel() => _mudDialog.Cancel();
}

