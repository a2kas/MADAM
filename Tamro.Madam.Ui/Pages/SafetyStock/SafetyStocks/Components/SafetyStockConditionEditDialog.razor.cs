using AutoMapper;
using DocumentFormat.OpenXml.Spreadsheet;
using Fluxor;
using MediatR;
using Microsoft.AspNetCore.Components;
using MudBlazor;
using Tamro.Madam.Application.Commands.ItemMasterdata.Items.SafetyStocks;
using Tamro.Madam.Application.Validation;
using Tamro.Madam.Models.ItemMasterdata.Items.SafetyStocks;
using Tamro.Madam.Ui.Store.State;

namespace Tamro.Madam.Ui.Pages.SafetyStock.SafetyStocks.Components;

public partial class SafetyStockConditionEditDialog
{
    [EditorRequired]
    [Parameter]
    public SafetyStockConditionEditDialogModel Model { get; set; }

    [Inject]
    private IState<UserProfileState> _userProfileState { get; set; }
    [Inject]
    private IMediator _mediator { get; set; }
    [Inject]
    private IMapper _mapper { get; set; }
    [Inject]
    private IValidationService _validator { get; set; }
    [CascadingParameter]
    private IMudDialogInstance _mudDialog { get; set; } = default!;

    private bool _isItemInfoPopoverOpen;
    private int _initialCheckDays;
    private bool _isSaving;
    private MudForm? _form;


    protected override async Task OnInitializedAsync()
    {
        _initialCheckDays = Model.CheckDays;
    }

    private async Task Submit()
    {
        _isSaving = true;
        await _form!.Validate();

        if (!_form.IsValid)
        {
            return;
        }

        var upsertModel = _mapper.Map<SafetyStockConditionUpsertModel>(Model);

        var cmd = new UpdateSafetyStockConditionCommand(upsertModel);
        var result = await _mediator.Send(cmd);
        if (result.Succeeded)
        {
            _mudDialog.Close(DialogResult.Ok(true));
            Snackbar.Add($"Safety stock condition updated", Severity.Success);
        }
        else
        {
            Snackbar.Add(result.ErrorMessage, Severity.Error);
        }

        _isSaving = false;
    }

    private void ToggleItemInfoPopover()
    {
        _isItemInfoPopoverOpen = !_isItemInfoPopoverOpen;
    }

    private void Cancel() => _mudDialog.Cancel();
}
