using AutoMapper;
using Fluxor;
using MediatR;
using Microsoft.AspNetCore.Components;
using MudBlazor;
using MudExtensions;
using System.Text.RegularExpressions;
using Tamro.Madam.Application.Commands.ItemMasterdata.Items.SafetyStocks;
using Tamro.Madam.Models.General;
using Tamro.Madam.Models.ItemMasterdata.Items.SafetyStocks;
using Tamro.Madam.Models.ItemMasterdata.Items.SafetyStocks.PharmacyChains;
using Tamro.Madam.Ui.Store.State;

namespace Tamro.Madam.Ui.Pages.SafetyStock.SafetyStocks.Components;

public partial class SafetyStockImportDialog
{
    private bool _isLoading;
    private BalticCountry _country;
    private MudStepperExtended? _stepper = new();
    [CascadingParameter]
    private IMudDialogInstance _dialog { get; set; } = default!;
    private MudTextField<string> _itemNumbersField = new();
    private string _itemNumbersFieldValue = "";
    private string _assignFormErrorMessage = "";
    private List<PharmacyChainModel> _pharmacyChains = [];
    private List<string> _pharmacyGroups = [];
    private List<SafetyStockGridDataModel> _rows { get; set; } = [];
    private List<SafetyStockImportResultModel> _importResults { get; set; } = [];

    [Inject]
    private IMediator _mediator { get; set; }
    [Inject]
    private IMapper _mapper { get; set; }
    [Inject]
    private IState<UserProfileState> _userProfileState { get; set; }

    protected override async Task OnInitializedAsync()
    {
        LoadPharmacyGroups();
        await LoadPharmacyChains();
    }

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (firstRender)
        {
            _country = _userProfileState.Value.UserProfile.Country ?? default;
        }
    }

    private void LoadPharmacyGroups()
    {
        _pharmacyGroups = Enum.GetNames(typeof(PharmacyGroup)).Where(x => x != nameof(PharmacyGroup.All)).ToList();
    }

    private async Task LoadPharmacyChains()
    {
        var result = await _mediator.Send(new GetSafetyStockPharmacyChainsCommand(_userProfileState.Value.UserProfile.Country));
        if (result.Succeeded)
        {
            _pharmacyChains = [.. result.Data.OrderBy(x => x.DisplayName)];
        }
        else
        {
            Snackbar.Add(result.ErrorMessage, Severity.Error);
        }
    }

    private async Task OnActiveStepChanged(int step)
    {
        _isLoading = true;
        if (step == (int)SafetyStockImportDialogStep.FillItems)
        {
            await GetItemRows();
        }
        if (step == (int)SafetyStockImportDialogStep.FinalizeImport)
        {
            var isImportSuccess = await PerformImport();
            if (!isImportSuccess)
            {
                await _stepper.SetActiveStepByIndex((int)SafetyStockImportDialogStep.FillItems);
            }
        }
        _isLoading = false;
    }

    private async Task<bool> CheckChange(StepChangeDirection direction, int targetIndex)
    {
        if (direction == StepChangeDirection.Backward || (targetIndex > _stepper.GetActiveIndex() + 1))
        {
            return true;
        }
        if (targetIndex == (int)SafetyStockImportDialogStep.FillItems)
        {
            _assignFormErrorMessage = string.Empty;
            var itemNumbers = Regex.Split(_itemNumbersFieldValue, @"\s+|\r?\n").Where(x => !string.IsNullOrEmpty(x.Trim()));
            if (!itemNumbers.Any())
            {
                _itemNumbersField.Validate();
                return true;
            }
        }
        if (targetIndex == (int)SafetyStockImportDialogStep.FinalizeImport)
        {
            var allRowsAreValid = _rows.Where(x => !string.IsNullOrEmpty(x.ItemName)).All(x => x.PharmacyChainId != default || x.SafetyStockPharmacyChainGroup != default);
            if (!allRowsAreValid)
            {
                _assignFormErrorMessage = "In order to be able to import, all rows should have restriction set either on pharmacy group or pharmacy chain";
                return true;
            }
            var atLeastOneItemIsExisting = _rows.Any(x => !string.IsNullOrEmpty(x.ItemName));
            if (!atLeastOneItemIsExisting)
            {
                _assignFormErrorMessage = "At least one valid item no to be provided to proceed with import";
                return true;
            }
        }
        if (targetIndex == (int)SafetyStockImportDialogStep.CloseDialog)
        {
            Close();
        }

        return false;
    }

    private void OnPharmacyGroupChanged(SafetyStockGridDataModel row, string? pharmacyChainGroup)
    {
        row.SafetyStockPharmacyChainGroup = pharmacyChainGroup;
        row.PharmacyChainId = null;
    }

    private void OnPharmacyChainChanged(SafetyStockGridDataModel row, int? pharmacyChainId)
    {
        row.SafetyStockPharmacyChainGroup = null;
        row.PharmacyChainId = pharmacyChainId;
        row.PharmacyChainDisplayName = _pharmacyChains.SingleOrDefault(x => x.Id == pharmacyChainId)?.DisplayName;
    }

    private async Task OnDeleteRow(SafetyStockGridDataModel? row)
    {
        _rows.Remove(row);
    }

    private async Task OnCopyRow(SafetyStockGridDataModel? row)
    {
        _rows.Add(_mapper.Map<SafetyStockGridDataModel>(row));
        _rows = _rows.OrderBy(x => x.ItemNo).ToList();
    }

    private async Task GetItemRows()
    {
        var itemNumbers = Regex.Split(_itemNumbersFieldValue, @"\s+|\r?\n").Where(x => !string.IsNullOrEmpty(x.Trim()));
        var cmd = new GetImportableSafetyStockItemsInfoCommand(itemNumbers.Distinct().ToArray(), _country);
        var result = await _mediator.Send(cmd);

        if (result.Succeeded)
        {
            var safetyStockItems = result.Data.ToList();
            foreach (var itemNumber in itemNumbers)
            {
                var foundRow = result.Data.FirstOrDefault(x => x.ItemNo == itemNumber);
                if (foundRow == null)
                {
                    _rows.Add(new SafetyStockGridDataModel() { ItemNo = itemNumber, });
                }
                else
                {
                    _rows.Add(_mapper.Map<SafetyStockGridDataModel>(result.Data.Single(x => x.ItemNo == itemNumber)));
                }
            }
        }
        else
        {
            Snackbar.Add(result.ErrorMessage, Severity.Error);
        }
    }

    private async Task<bool> PerformImport()
    {
        var cmd = new ImportSafetyStocksCommand(_rows, _country);
        var result = await _mediator.Send(cmd);
        _importResults = result.Data?.ToList();

        if (!result.Succeeded)
        {
            Snackbar.Add(result.ErrorMessage, Severity.Error);
        }

        return result.Succeeded;
    }

    private void Close() => _dialog.Close();
}

public enum SafetyStockImportDialogStep
{
    SelectItems = 0,
    FillItems = 1,
    FinalizeImport = 2,
    CloseDialog = 3,
}
