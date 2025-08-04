using Fluxor;
using MediatR;
using Microsoft.AspNetCore.Components;
using MudBlazor;
using Tamro.Madam.Application.Commands.Commerce.ItemAssortmentSalesChannels;
using Tamro.Madam.Application.Validation;
using Tamro.Madam.Models.Commerce.ItemAssortmentSalesChannels;
using Tamro.Madam.Models.Commerce.ItemAssortmentSalesChannels.Import;
using Tamro.Madam.Models.General;
using Tamro.Madam.Ui.Store.Actions.Commerce.ItemAssortmentSalesChannels;
using Tamro.Madam.Ui.Store.State;
using Tamro.Madam.Ui.Store.State.Commerce.ItemAssortmentSalesChannels;
using Tamro.Madam.Ui.Utils;

namespace Tamro.Madam.Ui.Pages.Commerce.ItemAssortmentSalesChannels.Components;

public partial class ItemAssortmentImportDialog
{
    private ItemAssortmentImportModel Model = new();
    private List<ItemAssortmentItemModel> _items = new List<ItemAssortmentItemModel>();
    private ItemAssortmentImportResultModel _importResult = new();

    [Inject]
    private UserSettingsUtils _userSettings { get; set; }
    [Inject]
    private IValidationService _validator { get; set; }
    [Inject]
    private IState<UserProfileState> _userProfileState { get; set; }
    [Inject]
    private IState<ItemAssortmentSalesChannelState> _state { get; set; }
    [Inject]
    private IDispatcher _dispatcher { get; set; }
    [Inject]
    private IMediator _mediator { get; set; }

    [CascadingParameter]
    private IMudDialogInstance _dialog { get; set; } = default!;
    private MudForm? _form;
    private bool _isLoading;
    private int _defaultPageSize;

    protected override async Task OnInitializedAsync()
    {
        _defaultPageSize = _userSettings.GetRowsPerPageSetting();
    }

    private async Task OnPreviewInteraction(StepperInteractionEventArgs arg)
    {
        if (arg.Action == StepAction.Complete)
        {
            if (arg.StepIndex == 2)
            {
                var model = _state.Value.Model;
                model.Assortment = _importResult.Assortment;
                _dispatcher.Dispatch(new SetActiveFormAction(ItemAssortmentSalesChannelForm.Details, model));
                _dialog.Close();
                Snackbar.Add("Import successfull", Severity.Success);
            }
            else
            {
                await ControlStepCompletion(arg);
            }
        }
    }

    private async Task ControlStepCompletion(StepperInteractionEventArgs arg)
    {
        switch (arg.StepIndex)
        {
            case 0:
                await _form.Validate();
                if (!_form.IsValid)
                {
                    arg.Cancel = true;
                }
                break;

        }
    }

    private async Task OnActiveIndexChanged(int index)
    {
        switch (index)
        {
            case 1:
                await PrepareItemsForValidation();
                break;
            case 2:
                await GetOverview();
                break;
            default:
                break;
        }
    }

    private async Task PrepareItemsForValidation()
    {
        _isLoading = true;
        Model.Country = _userProfileState.Value.UserProfile.Country ?? BalticCountry.LT;
        var cmd = new GetImportedAssortmentCommand(Model);
        var result = await _mediator.Send(cmd);

        if (result.Succeeded)
        {
            _items = result.Data.ToList();
        }
        else
        {
            Snackbar.Add(result.ErrorMessage, Severity.Error);
        }
        _isLoading = false;
    }

    private async Task GetOverview()
    {
        _isLoading = true;
        var model = new ItemAssortmentFinalizeImportModel()
        {
            ExistingAssortment = _state.Value.Model.Assortment,
            ImportedAssortment = _items,
            ImportAction = Model.Action,
            SalesChannelId = _state.Value.Model.Id,
        };
        var cmd = new FinalizeItemAssortmentImportCommand(model);
        var result = await _mediator.Send(cmd);

        if (result.Succeeded)
        {
            _importResult = result.Data;
        }
        else
        {
            Snackbar.Add(result.ErrorMessage, Severity.Error);
        }
        _isLoading = false;
    }

    private async Task OnDeleteRow(ItemAssortmentItemModel? row)
    {
        _items.Remove(row);
    }

    private void Close() => _dialog.Close();
}
