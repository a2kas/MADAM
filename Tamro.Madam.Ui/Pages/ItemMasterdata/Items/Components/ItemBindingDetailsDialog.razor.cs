using Fluxor;
using MediatR;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using MudBlazor;
using Tamro.Madam.Application.Commands.ItemMasterdata.Items.Bindings;
using Tamro.Madam.Application.Validation;
using Tamro.Madam.Models.Common.Dialog;
using Tamro.Madam.Models.ItemMasterdata.Items.Bindings;
using Tamro.Madam.Ui.Store.State;

namespace Tamro.Madam.Ui.Pages.ItemMasterdata.Items.Components;

public partial class ItemBindingDetailsDialog
{
    [EditorRequired]
    [Parameter]
    public ItemBindingModel Model { get; set; }
    [EditorRequired]
    [Parameter]
    public DialogState State { get; set; }

    [Inject]
    private IState<UserProfileState> _userProfileState { get; set; }
    [Inject]
    private IValidationService _validator { get; set; }
    [Inject]
    private IMediator _mediator { get; set; }
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

            var result = await _mediator.Send(new UpsertItemBindingCommand(Model, _userProfileState.Value.UserProfile));

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
            DialogState.Create => $"Create new binding",
            DialogState.View => $"Binding",
            DialogState.Copy => $"Copy binding",
            _ => "",
        };
    }

    private string GetSuccessMessage()
    {
        return State switch
        {
            DialogState.Create or DialogState.Copy => $"Binding {Model.LocalId} created",
            DialogState.View => $"Binding {Model.LocalId} updated",
            _ => "",
        };
    }

    private void Cancel() => _mudDialog.Cancel();
}
