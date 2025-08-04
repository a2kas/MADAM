using Fluxor;
using MediatR;
using Microsoft.AspNetCore.Components;
using MudBlazor;
using Tamro.Madam.Application.Commands.ItemMasterdata.Items;
using Tamro.Madam.Application.Validation;
using Tamro.Madam.Models.Common.Dialog;
using Tamro.Madam.Ui.Store.Actions.ItemMasterdata.Items;
using Tamro.Madam.Ui.Store.State;
using Tamro.Madam.Ui.Store.State.ItemMasterdata;

namespace Tamro.Madam.Ui.Pages.ItemMasterdata.Items.Components;

public partial class ItemDetailsDialog
{
    [CascadingParameter]
    private IMudDialogInstance _mudDialog { get; set; } = default!;
    private bool _saving;
    private MudForm? _form;

    [Inject]
    private IState<UserProfileState> _userProfileState { get; set; }
    [Inject]
    private IState<ItemState> _itemState { get; set; }
    [Inject]
    private IDispatcher _dispatcher { get; set; }
    [Inject]
    private IValidationService _validator { get; set; }
    [Inject]
    private IMediator _mediator { get; set; }
    [Inject]
    private IActionSubscriber _actionSubscriber { get; set; }

    protected override async Task OnInitializedAsync()
    {
        _actionSubscriber?.SubscribeToAction<SetItemBarcodesAction>(this, _ => StateHasChanged());
        _actionSubscriber?.SubscribeToAction<SetItemBindingsAction>(this, _ => StateHasChanged());

        await base.OnInitializedAsync();
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

            var result = await _mediator.Send(new UpsertItemCommand(_itemState.Value.Item, _userProfileState.Value.UserProfile));

            if (result.Succeeded)
            {
                Snackbar.Add(GetSuccessMessage(), Severity.Success);
                if (_itemState.Value.DialogState == DialogState.Create || _itemState.Value.DialogState == DialogState.Copy)
                {
                    var createdItemResult = await _mediator.Send(new GetItemCommand(result.Data));
                    _dispatcher.Dispatch(new SetCurrentItemAction()
                    {
                        Item = createdItemResult.Data,
                        State = DialogState.View,
                        UserProfileState = _userProfileState.Value,
                    });
                    _mudDialog.StateHasChanged();
                }
                else
                {
                    _mudDialog.Close(DialogResult.Ok(true));
                }
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
        return _itemState.Value.DialogState switch
        {
            DialogState.Create => $"Create new item",
            DialogState.View => $"Item",
            DialogState.Copy => $"Copy item",
            _ => "",
        };
    }

    private string GetSuccessMessage()
    {
        return _itemState.Value.DialogState switch
        {
            DialogState.Create or DialogState.Copy => $"Item {_itemState.Value.Item.ItemName} created",
            DialogState.View => $"Item {_itemState.Value.Item.ItemName} updated",
            _ => "",
        };
    }
}