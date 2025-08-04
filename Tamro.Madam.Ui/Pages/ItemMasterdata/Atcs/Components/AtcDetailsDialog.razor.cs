using Fluxor;
using MediatR;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using MudBlazor;
using Tamro.Madam.Application.Commands.ItemMasterdata.Atcs;
using Tamro.Madam.Application.Validation;
using Tamro.Madam.Models.Common.Dialog;
using Tamro.Madam.Models.ItemMasterdata.Atcs;
using Tamro.Madam.Ui.Store.State;

namespace Tamro.Madam.Ui.Pages.ItemMasterdata.Atcs.Components;

public partial class AtcDetailsDialog
{
    [EditorRequired]
    [Parameter]
    public AtcModel Model { get; set; }
    [EditorRequired]
    [Parameter]
    public DialogState State { get; set; }

    [Inject]
    private IValidationService _validator { get; set; }
    [Inject]
    private IMediator _mediator { get; set; }
    [CascadingParameter]
    private IMudDialogInstance _mudDialog { get; set; } = default!;
    [Inject]
    private IState<UserProfileState> _userProfileState { get; set; }

    private bool _saving;
    private MudForm? _form;

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
            _saving = true;
            await _form!.Validate();

            if (!_form.IsValid)
            {
                return;
            }

            var result = await _mediator.Send(new UpsertAtcCommand(Model));

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
            DialogState.Create => $"Create new ATC",
            DialogState.View => $"ATC",
            DialogState.Copy => $"Copy ATC",
            _ => "",
        };
    }

    private string GetSuccessMessage()
    {
        return State switch
        {
            DialogState.Create or DialogState.Copy => $"ATC {Model.Name} created",
            DialogState.View => $"ATC {Model.Name} updated",
            _ => "",
        };
    }

    private void Cancel() => _mudDialog.Cancel();
}
