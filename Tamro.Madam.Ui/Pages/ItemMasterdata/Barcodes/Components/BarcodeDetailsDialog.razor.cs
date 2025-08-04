using Fluxor;
using MediatR;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using MudBlazor;
using Tamro.Madam.Application.Commands.ItemMasterdata.Barcodes;
using Tamro.Madam.Application.Queries.Items.Clsf;
using Tamro.Madam.Application.Validation;
using Tamro.Madam.Models.Common.Dialog;
using Tamro.Madam.Models.ItemMasterdata.Barcodes;
using Tamro.Madam.Models.ItemMasterdata.Items;
using Tamro.Madam.Ui.Store.State;

namespace Tamro.Madam.Ui.Pages.ItemMasterdata.Barcodes.Components;

public partial class BarcodeDetailsDialog
{
    [EditorRequired]
    [Parameter]
    public BarcodeModel Model { get; set; }
    [EditorRequired]
    [Parameter]
    public DialogState State { get; set; }
    [Parameter]
    public bool IsItemSelectionDisabled { get; set; }

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

            var result = await _mediator.Send(new UpsertBarcodeCommand(Model));

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

    private async Task<IEnumerable<ItemClsfModel>> SearchItems(string value, CancellationToken token)
    {
        try
        {
            var query = new ItemsClsfQuery()
            {
                SearchTerm = value,
            };

            var result = await _mediator.Send(query);

            return result.Items;
        }
        catch (Exception)
        {
            Snackbar.Add("Failed to load items", Severity.Error);
            return new List<ItemClsfModel>();
        }
    }

    private string GetTitle()
    {
        return State switch
        {
            DialogState.Create => $"Create new barcode",
            DialogState.View => $"Barcode",
            DialogState.Copy => $"Copy barcode",
            _ => "",
        };
    }

    private string GetSuccessMessage()
    {
        return State switch
        {
            DialogState.Create or DialogState.Copy => $"Barcode {Model.Ean} created",
            DialogState.View => $"Barcode {Model.Ean} updated",
            _ => "",
        };
    }

    private void Cancel() => _mudDialog.Cancel();
}
