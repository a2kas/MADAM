using Fluxor;
using MediatR;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using MudBlazor;
using Tamro.Madam.Application.Commands.ItemMasterdata.Items.Bindings.Vlk;
using Tamro.Madam.Application.Queries.Items.Bindings.Clsf;
using Tamro.Madam.Application.Validation;
using Tamro.Madam.Models.Common.Dialog;
using Tamro.Madam.Models.General;
using Tamro.Madam.Models.ItemMasterdata.Items.Bindings;
using Tamro.Madam.Models.ItemMasterdata.Items.Bindings.Vlk;
using Tamro.Madam.Ui.Store.State;

namespace Tamro.Madam.Ui.Pages.ItemMasterdata.Items.Components;

public partial class VlkBindingDetailsDialog
{
    [EditorRequired]
    [Parameter]
    public VlkBindingDetailsModel Model { get; set; }
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

            var result = await _mediator.Send(new UpsertVlkBindingCommand(Model));

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

    private async Task<IEnumerable<ItemBindingClsfModel>> SearchItemBindings(string value, CancellationToken token)
    {
        try
        {
            var query = new ItemBindingClsfQuery()
            {
                Companies = [Classifiers.LtWholesaleCode,],
                SearchTerm = value,
            };

            var result = await _mediator.Send(query);

            return result.Items;
        }
        catch (Exception)
        {
            Snackbar.Add("Failed to load items", Severity.Error);
            return new List<ItemBindingClsfModel>();
        }
    }

    private string GetTitle()
    {
        return State switch
        {
            DialogState.Create => $"Create new vlk binding",
            DialogState.View => $"Vlk binding",
            _ => "",
        };
    }

    private string GetSuccessMessage()
    {
        return State switch
        {
            DialogState.Create or DialogState.Copy => $"Vlk binding created",
            DialogState.View => $"Vlk binding updated",
            _ => "",
        };
    }

    private void Cancel() => _mudDialog.Cancel();
}
