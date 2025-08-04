using Fluxor;
using MediatR;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using MudBlazor;
using Tamro.Madam.Application.Commands.ItemMasterdata.Nicks;
using Tamro.Madam.Application.Validation;
using Tamro.Madam.Models.Common.Dialog;
using Tamro.Madam.Models.ItemMasterdata.Nicks;
using Tamro.Madam.Ui.Store.State;

namespace Tamro.Madam.Ui.Pages.ItemMasterdata.Nicks.Components;

public partial class NickDetailsDialog
{
    [EditorRequired]
    [Parameter]
    public NickModel Model { get; set; }
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

            var result = await _mediator.Send(new UpsertNickCommand(Model));
            Model.Id = result.Data;

            if (result.Succeeded)
            {
                Model.Id = result.Data;
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

    private string GetTitle()
    {
        return State switch
        {
            DialogState.Create => $"Create new baltic nick",
            DialogState.View => $"Baltic nick",
            DialogState.Copy => $"Copy baltic nick",
            _ => "",
        };
    }

    private string GetSuccessMessage()
    {
        return State switch
        {
            DialogState.Create or DialogState.Copy => $"Baltic nick {Model.Name} created",
            DialogState.View => $"Baltic nick {Model.Name} updated",
            _ => "",
        };
    }

    private void Cancel() => _mudDialog.Cancel();
}
