using Fluxor;
using MediatR;
using Microsoft.AspNetCore.Components;
using MudBlazor;
using Tamro.Madam.Application.Commands.ItemMasterdata.Barcodes;
using Tamro.Madam.Models.Common.Dialog;
using Tamro.Madam.Models.ItemMasterdata.Barcodes;
using Tamro.Madam.Models.ItemMasterdata.Items;
using Tamro.Madam.Ui.Components.Dialogs;
using Tamro.Madam.Ui.Pages.ItemMasterdata.Barcodes.Components;
using Tamro.Madam.Ui.Store.Actions.ItemMasterdata.Items;
using Tamro.Madam.Ui.Store.State;
using Tamro.Madam.Ui.Store.State.ItemMasterdata;

namespace Tamro.Madam.Ui.Pages.ItemMasterdata.Items.Components.Tabs;

public partial class ItemBarcodesTab
{
    [Inject]
    private IState<ItemState> _itemState { get; set; }
    [Inject]
    private IState<UserProfileState> _userProfileState { get; set; }
    [Inject]
    private IDispatcher _dispatcher { get; set; }
    [Inject]
    private IMediator _mediator { get; set; }

    private async Task OnCreateBarcode()
    {
        await ShowBarcodeDetailsDialog(new BarcodeModel() { Measure = true, }, DialogState.Create);
    }

    private async Task OnEditBarcode(BarcodeModel? barcode)
    {
        await ShowBarcodeDetailsDialog(barcode, DialogState.View);
    }

    private async Task OnDeleteBarcode(int id)
    {
        var command = new DeleteBarcodesCommand(new int[] { id, });
        var parameters = new DialogParameters<ConfirmationDialog>
        {
            { x => x.Command, command },
            { x => x.Title, "Delete barcode" },
            { x => x.Content, $"Are you sure you want to delete barcode?" },
            { x => x.SuccessMessage, $"Barcode deleted" },
        };
        var options = new DialogOptions
        {
            CloseButton = true,
            MaxWidth = MaxWidth.Small,
            FullWidth = true,
            CloseOnEscapeKey = true,
        };
        var dialog = DialogService.Show<ConfirmationDialog>("Delete", parameters, options);
        var state = await dialog.Result;

        if (!state.Canceled)
        {
            await RefreshBarcodes();
        }
    }

    private async Task ShowBarcodeDetailsDialog(BarcodeModel? model, DialogState dialogState)
    {
        model.Item = new ItemClsfModel()
        {
            Name = _itemState.Value.Item.ItemName,
            Id = _itemState.Value.Item.Id,
        };
        var parameters = new DialogParameters<BarcodeDetailsDialog>
        {
            { x => x.Model, model },
            { x => x.State, dialogState },
            { x => x.IsItemSelectionDisabled, true },
        };
        var options = new DialogOptions
        {
            CloseButton = true,
            MaxWidth = MaxWidth.Small,
            CloseOnEscapeKey = true,
            FullWidth = true,
        };
        var dialog = DialogService.Show<BarcodeDetailsDialog>(string.Empty, parameters, options);
        var state = await dialog.Result;
        await RefreshBarcodes();
    }

    private async Task RefreshBarcodes()
    {
        var getItemBarcodesCommand = await _mediator.Send(new GetItemBarcodesCommand(_itemState.Value.Item.Id));
        if (getItemBarcodesCommand.Succeeded)
        {
            _dispatcher.Dispatch(new SetItemBarcodesAction()
            {
                Barcodes = getItemBarcodesCommand.Data,
                UserProfileState = _userProfileState.Value,
            });
        }
        else
        {
            Snackbar.Add("Failed to load barcodes", Severity.Error);
        }
    }
}