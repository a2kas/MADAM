using Fluxor;
using MediatR;
using Microsoft.AspNetCore.Components;
using MudBlazor;
using Tamro.Madam.Application.Commands.ItemMasterdata.Items.Bindings;
using Tamro.Madam.Models.Common.Dialog;
using Tamro.Madam.Models.ItemMasterdata.Items;
using Tamro.Madam.Models.ItemMasterdata.Items.Bindings;
using Tamro.Madam.Ui.Components.Dialogs;
using Tamro.Madam.Ui.Store.Actions.ItemMasterdata.Items;
using Tamro.Madam.Ui.Store.State;
using Tamro.Madam.Ui.Store.State.ItemMasterdata;

namespace Tamro.Madam.Ui.Pages.ItemMasterdata.Items.Components.Tabs;

public partial class ItemBindingsTab
{
    private Func<ItemBindingModel, object> _bindingsGroupBy = x => x.Company?.Country;

    private Func<ItemBindingModel, object> _companySortBy = x => x.Company?.Country;

    private Func<ItemBindingModel, object> _languageSortBy = x => x.Languages.Any() ? x.Languages.First().Language.Code : null;

    [Inject]
    private IState<ItemState> _itemState { get; set; }
    [Inject]
    private IState<UserProfileState> _userProfileState { get; set; }
    [Inject]
    private IDispatcher _dispatcher { get; set; }
    [Inject]
    private IMediator _mediator { get; set; }

    private async Task OnCreateBinding()
    {
        await ShowBindingDetailsDialog(new ItemBindingModel(), DialogState.Create);
    }

    private async Task OnEditBinding(ItemBindingModel? binding)
    {
        await ShowBindingDetailsDialog(binding, DialogState.View);
    }

    private async Task OnDeleteBinding(int id)
    {
        var command = new DeleteItemBindingCommand(id);
        var parameters = new DialogParameters<ConfirmationDialog>
        {
            { x => x.Command, command },
            { x => x.Title, "Delete binding" },
            { x => x.Content, $"Are you sure you want to delete binding?" },
            { x => x.SuccessMessage, $"Binding deleted" },
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
            await RefreshBindings();
        }
    }

    private async Task ShowBindingDetailsDialog(ItemBindingModel? model, DialogState dialogState)
    {
        model.Item = new ItemClsfModel()
        {
            Name = _itemState.Value.Item.ItemName,
            Id = _itemState.Value.Item.Id,
        };
        var parameters = new DialogParameters<ItemBindingDetailsDialog>
        {
            { x => x.Model, model },
            { x => x.State, dialogState },
        };
        var options = new DialogOptions
        {
            CloseButton = true,
            MaxWidth = MaxWidth.Small,
            CloseOnEscapeKey = true,
            FullWidth = true,
        };
        var dialog = DialogService.Show<ItemBindingDetailsDialog>(string.Empty, parameters, options);
        var state = await dialog.Result;
        await RefreshBindings();
    }

    private async Task RefreshBindings()
    {
        var getItemItemBindingsCommand = await _mediator.Send(new GetItemItemBindingsCommand(_itemState.Value.Item.Id));
        if (getItemItemBindingsCommand.Succeeded)
        {
            _dispatcher.Dispatch(new SetItemBindingsAction()
            {
                Bindings = getItemItemBindingsCommand.Data,
                UserProfileState = _userProfileState.Value,
            });
        }
        else
        {
            Snackbar.Add("Failed to load bindings", Severity.Error);
        }
    }
}