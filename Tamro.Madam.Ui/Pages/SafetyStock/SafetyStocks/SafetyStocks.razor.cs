using AutoMapper;
using BlazorDownloadFile;
using Fluxor;
using MediatR;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using MudBlazor;
using Tamro.Madam.Application.Commands.ItemMasterdata.Items.SafetyStocks;
using Tamro.Madam.Application.Queries.Items.SafetyStocks;
using Tamro.Madam.Application.Services.Files;
using Tamro.Madam.Common.Constants.SearchExpressionConstants;
using Tamro.Madam.Common.Utils;
using Tamro.Madam.Models.General;
using Tamro.Madam.Models.ItemMasterdata.Items.SafetyStocks;
using Tamro.Madam.Models.ItemMasterdata.Items.SafetyStocks.PharmacyChains;
using Tamro.Madam.Repository.Entities.ItemMasterdata.Items.SafetyStocks;
using Tamro.Madam.Ui.ComponentExtensions;
using Tamro.Madam.Ui.Components.Audit;
using Tamro.Madam.Ui.Components.DataGrid.Filtering;
using Tamro.Madam.Ui.Components.Dialogs;
using Tamro.Madam.Ui.Pages.SafetyStock.SafetyStocks.Components;
using Tamro.Madam.Ui.Store.State;
using Tamro.Madam.Ui.Utils;

namespace Tamro.Madam.Ui.Pages.SafetyStock.SafetyStocks;

public partial class SafetyStocks
{
    #region Properties
    private int _defaultPageSize;
    private bool _loading;
    private DictionaryFilterOptions _countryFilterOptions = new();
    private BooleanFilterOptions _canBuyFilterOptions = new();
    private HashSet<SafetyStockGridDataModel> _selectedSafetyStocks = [];
    private SafetyStockQuery _query { get; set; } = new();
    private MudDataGrid<SafetyStockGridDataModel> _table = new();
    private List<IFilterDefinition<SafetyStockGridDataModel>> _defaultFilterDefinition = [];
    private List<PharmacyChainModel> _pharmacyChains = [];
    private List<string> _pharmacyGroups = [];
    private IEnumerable<SafetyStockGridDataModel> _data;
    #endregion

    #region IoC
    [Inject]
    private UserSettingsUtils _userSettings { get; set; }
    [Inject]
    private IState<UserProfileState> _userProfileState { get; set; }
    [Inject]
    private IMediator _mediator { get; set; }
    [Inject]
    private IExcelService _excelService { get; set; }
    [Inject]
    private IBlazorDownloadFileService _blazorDownloadFileService { get; set; }
    [Inject]
    private IMapper _mapper { get; set; }
    #endregion

    #region Events
    protected override async Task OnInitializedAsync()
    {
        _defaultPageSize = _userSettings.GetRowsPerPageSetting();
        LoadPharmacyGroups();
        await LoadPharmacyChains();
    }

    private void LoadPharmacyGroups()
    {
        _pharmacyGroups = Enum.GetNames(typeof(PharmacyGroup)).ToList();
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

    private async Task OnCanBuyChanged(SafetyStockGridDataModel row, bool canBuy)
    {
        var upsertModel = _mapper.Map<SafetyStockConditionUpsertModel>(row);
        upsertModel.CanBuy = canBuy;
        await CommitRowChanges(row, upsertModel);
    }

    private async Task OnPharmacyGroupChanged(SafetyStockGridDataModel row, string? pharmacyChainGroup)
    {
        var upsertModel = _mapper.Map<SafetyStockConditionUpsertModel>(row);
        if (Enum.TryParse(pharmacyChainGroup, out PharmacyGroup pharmacyGroup))
        {
            upsertModel.PharmacyGroup = pharmacyGroup;
            upsertModel.PharmacyChainId = null;
            upsertModel.PharmacyChainName = string.Empty;
            await CommitRowChanges(row, upsertModel);
        }
    }

    private async Task OnPharmacyChainChanged(SafetyStockGridDataModel row, int? pharmacyChainId)
    {
        var upsertModel = _mapper.Map<SafetyStockConditionUpsertModel>(row);
        upsertModel.PharmacyChainId = pharmacyChainId;
        upsertModel.PharmacyChainName = _pharmacyChains?.FirstOrDefault(x => x.Id == pharmacyChainId)?.DisplayName;
        upsertModel.PharmacyGroup = null;
        await CommitRowChanges(row, upsertModel);
    }

    private async Task CommittedItemChanges(SafetyStockGridDataModel row)
    {
        var upsertModel = _mapper.Map<SafetyStockConditionUpsertModel>(row);
        await CommitRowChanges(row, upsertModel);
    }

    private async Task CommitRowChanges(SafetyStockGridDataModel row, SafetyStockConditionUpsertModel upsertModel)
    {
        _loading = true;

        var cmd = new UpdateSafetyStockConditionCommand(upsertModel);
        var result = await _mediator.Send(cmd);
        if (result.Succeeded)
        {
            Snackbar.Add("Saved", Severity.Success);
            _mapper.Map(upsertModel, row);
            var modifiedItems = _data.Where(x => x.ItemNo == row.ItemNo);
            foreach (var modifiedItem in modifiedItems)
            {
                modifiedItem.CheckDays = row.CheckDays;
            }
        }
        else
        {
            Snackbar.Add(result.ErrorMessage, Severity.Error);
        }

        _loading = false;
    }

    private async Task OnBasicSearchKeyDown(KeyboardEventArgs e)
    {
        if (e.Key == "Enter")
        {
            await _table.ReloadServerData();
        }
    }

    private async Task OnReset()
    {
        _query = new();
        await MudDataGridExtensions<SafetyStockGridDataModel>.ResetGridState(_table);

        await ClearCanBuyFilter();
    }

    private async Task OnCreate()
    {
        await ShowCreateDialog();
    }

    private async Task OnOpen()
    {
        await OnOpen(_selectedSafetyStocks.First());
    }

    private async Task OnOpen(SafetyStockGridDataModel? safetyStockCondition)
    {
        if (safetyStockCondition == null)
        {
            return;
        }

        var model = _mapper.Map<SafetyStockConditionEditDialogModel>(safetyStockCondition);
        var parameters = new DialogParameters<SafetyStockConditionEditDialog>
        {
            { x => x.Model, model },
        };
        var options = new DialogOptions()
        {
            CloseButton = true,
            Position = DialogPosition.TopCenter,
            MaxWidth = MaxWidth.Large,
            CloseOnEscapeKey = true,
            FullWidth = true,
        };

        var dialog = DialogService.Show<SafetyStockConditionEditDialog>(string.Empty, parameters, options);
        var state = await dialog.Result;

        await _table.ReloadServerData();
    }

    private async Task OnImport()
    {
        var parameters = new DialogParameters<SafetyStockImportDialog>();
        var options = new DialogOptions
        {
            CloseButton = true,
            Position = DialogPosition.TopCenter,
            MaxWidth = MaxWidth.Large,
            CloseOnEscapeKey = true,
            FullWidth = true,
        };
        var dialog = DialogService.Show<SafetyStockImportDialog>(string.Empty, parameters, options);
        var state = await dialog.Result;

        await _table.ReloadServerData();
    }

    private async Task OnExport()
    {
        try
        {
            _loading = true;
            var query = new SafetyStockQuery()
            {
                Filters = _table.FilterDefinitions,
                OrderBy = _table.SortDefinitions.Values.FirstOrDefault()?.SortBy ?? nameof(SafetyStockGridDataModel.Id),
                SortDirection = _table.SortDefinitions.Values.FirstOrDefault()?.Descending ?? false ? SortDirection.Descending.ToString() : SortDirection.Ascending.ToString(),
                PageNumber = 1,
                SearchTerm = _query.SearchTerm,
                Country = _userProfileState.Value.UserProfile.Country,
                PageSize = int.MaxValue,
            };
            var exportablSafetyStocks = await _mediator.Send(query);

            var mappers = new Dictionary<string, Func<SafetyStockGridDataModel, object?>>
            {
                { DisplayNameHelper.Get(typeof(SafetyStockConditionModel), nameof(SafetyStockGridDataModel.Id)), b => b.Id },
                { DisplayNameHelper.Get(typeof(SafetyStockConditionModel), nameof(SafetyStockGridDataModel.CanBuy)), b => b.CanBuy },
                { DisplayNameHelper.Get(typeof(SafetyStockConditionModel), nameof(SafetyStockGridDataModel.SafetyStockPharmacyChainGroup)), b => b.SafetyStockPharmacyChainGroup },
                { DisplayNameHelper.Get(typeof(SafetyStockConditionModel), nameof(SafetyStockGridDataModel.PharmacyChainDisplayName)), b => b.PharmacyChainDisplayName },
                { DisplayNameHelper.Get(typeof(SafetyStockGridDataModel), nameof(SafetyStockGridDataModel.ItemNo)), b => b.ItemNo },
                { DisplayNameHelper.Get(typeof(SafetyStockGridDataModel), nameof(SafetyStockGridDataModel.ItemName)), b => b.ItemName },
                { DisplayNameHelper.Get(typeof(SafetyStockConditionModel), nameof(SafetyStockConditionModel.CheckDays)), b => b.CheckDays },
                { DisplayNameHelper.Get(typeof(SafetyStockGridDataModel), nameof(SafetyStockGridDataModel.RetailQuantity)), b => b.RetailQuantity },
                { DisplayNameHelper.Get(typeof(SafetyStockGridDataModel), nameof(SafetyStockGridDataModel.WholesaleQuantity)), b => b.WholesaleQuantity },
                { DisplayNameHelper.Get(typeof(SafetyStockGridDataModel), nameof(SafetyStockGridDataModel.QuantityToBuy)), b => b.QuantityToBuy },
                { DisplayNameHelper.Get(typeof(SafetyStockGridDataModel), nameof(SafetyStockGridDataModel.ItemGroup)), b => b.ItemGroup },
                { DisplayNameHelper.Get(typeof(SafetyStockGridDataModel), nameof(SafetyStockGridDataModel.ProductClass)), b => b.ProductClass },
                { DisplayNameHelper.Get(typeof(SafetyStockGridDataModel), nameof(SafetyStockGridDataModel.Brand)), b => b.Brand },
                { DisplayNameHelper.Get(typeof(SafetyStockGridDataModel), nameof(SafetyStockGridDataModel.SupplierNumber)), b => b.SupplierNumber },
                { DisplayNameHelper.Get(typeof(SafetyStockGridDataModel), nameof(SafetyStockGridDataModel.Cn3)), b => b.Cn3 },
                { DisplayNameHelper.Get(typeof(SafetyStockGridDataModel), nameof(SafetyStockGridDataModel.Cn1)), b => b.Cn1 },
                { DisplayNameHelper.Get(typeof(SafetyStockGridDataModel), nameof(SafetyStockGridDataModel.SupplierNick)), b => b.SupplierNick },
                { DisplayNameHelper.Get(typeof(SafetyStockGridDataModel), nameof(SafetyStockGridDataModel.Substance)), b => b.Substance },
                { DisplayNameHelper.Get(typeof(SafetyStockConditionModel), nameof(SafetyStockConditionModel.Comment)), b => b.Comment },
            };
            var fileContent = await _excelService.ExportAsync(exportablSafetyStocks.Items, mappers, "SafetyStocks");
            await _blazorDownloadFileService.DownloadFile("SafetyStocks.xlsx", fileContent, "application/octet-stream");
            Snackbar.Add("Safety stocks exported successfully", Severity.Success);
        }
        catch (Exception)
        {
            Snackbar.Add("Failed to export safety stocks", Severity.Error);
        }
        finally
        {
            _loading = false;
        }
    }

    private async Task OnDeleteSingle(SafetyStockGridDataModel safetyStockCondition)
    {
        await OnDelete(new[] { safetyStockCondition.Id }, count: 1);
    }

    private async Task OnDeleteChecked()
    {
        var idsToDelete = _selectedSafetyStocks.Select(x => x.Id).ToArray();
        await OnDelete(idsToDelete, idsToDelete.Length);
    }

    private async Task OnDelete(int[] ids, int count)
    {
        var command = new DeleteSafetyStockConditionsCommand(ids);
        var parameters = new DialogParameters<ConfirmationDialog>
        {
            { x => x.Command, command },
            { x => x.Title, "Delete safety stock conditions" },
            { x => x.Content, $"Are you sure you want to delete {count} safety stock conditions?" },
            { x => x.SuccessMessage, $"{count} safety stock conditions deleted" },
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
            await _table.ReloadServerData();
            _selectedSafetyStocks.Clear();
        }
    }

    private async Task ShowCreateDialog()
    {
        var model = new SafetyStockItemUpsertFormModel()
        {
            Country = _userProfileState.Value.UserProfile.Country ?? default,
        };
        var parameters = new DialogParameters<SafetyStockCreateDialog>
        {
            { x => x.Model, model },
        };
        var options = new DialogOptions
        {
            CloseButton = true,
            Position = DialogPosition.TopCenter,
            MaxWidth = MaxWidth.Medium,
            CloseOnEscapeKey = true,
            FullWidth = true,
        };
        var dialog = DialogService.Show<SafetyStockCreateDialog>(string.Empty, parameters, options);
        var state = await dialog.Result;

        await _table.ReloadServerData();
    }

    private async Task OnHistoryView()
    {
        await OnHistoryView(_selectedSafetyStocks.First());
    }

    private async Task OnHistoryView(SafetyStockGridDataModel? safetyStock)
    {
        await ShowHistoryDialog(safetyStock.Id);
    }

    private async Task ShowHistoryDialog(int id)
    {
        var parameters = new DialogParameters<EntityHistory>
        {
            { x => x.EntityTypeName, nameof(SafetyStockItem) },
            { x => x.EntityId, id.ToString() },
        };

        var options = new DialogOptions
        {
            Position = DialogPosition.Center,
            CloseButton = true,
            MaxWidth = MaxWidth.Large,
            CloseOnEscapeKey = true,
            FullWidth = true,
        };

        await DialogService.ShowAsync<EntityHistory>($"Safety stock condition {id} history", parameters, options);
    }

    private async Task<GridData<SafetyStockGridDataModel>> DataSource(GridState<SafetyStockGridDataModel> state)
    {
        try
        {
            _loading = true;
            MudDataGridExtensions<SafetyStockGridDataModel>.ApplyDefaultFilterColumns(state.FilterDefinitions, _table.RenderedColumns);
            _query.OrderBy = state.SortDefinitions.FirstOrDefault()?.SortBy ?? nameof(SafetyStockGridDataModel.ItemNo);
            _query.SortDirection = state.SortDefinitions.FirstOrDefault()?.Descending ?? true ? nameof(SortDirection.Ascending) : nameof(SortDirection.Descending);
            _query.PageNumber = state.Page + 1;
            _query.PageSize = state.PageSize;
            _query.Filters = state.FilterDefinitions;
            _query.Country = _userProfileState.Value.UserProfile.Country;

            var result = await _mediator.Send(_query);

            _data = result.Items;

            return new GridData<SafetyStockGridDataModel>()
            {
                TotalItems = result.TotalItems,
                Items = _data,
            };
        }
        catch (Exception)
        {
            Snackbar.Add("Failed to load safety stock", Severity.Error);
            return new();
        }
        finally
        {
            _loading = false;
        }
    }

    private async Task ApplyCanBuyFilter()
    {
        var filter = _table.FilterDefinitions.Find(x => x.Title == nameof(SafetyStockGridDataModel.CanBuy));
        if (filter != null)
        {
            _table.FilterDefinitions.Remove(filter);
        }
        _canBuyFilterOptions.IsOpen = false;
        if (_canBuyFilterOptions.YesChecked != _canBuyFilterOptions.NoChecked)
        {
            await _table.AddFilterAsync(new FilterDefinition<SafetyStockGridDataModel>()
            {
                Title = nameof(SafetyStockGridDataModel.CanBuy),
                Operator = SearchBoolConstants.Is,
                Value = _canBuyFilterOptions.YesChecked,
            });
        }
        await _table.ReloadServerData();
    }

    private async Task ClearCanBuyFilter()
    {
        var filter = _table.FilterDefinitions.Find(x => x.Title == nameof(SafetyStockGridDataModel.CanBuy));
        if (filter != null)
        {
            _table.FilterDefinitions.Remove(filter);
            await _table.ReloadServerData();
        }
        _canBuyFilterOptions.IsOpen = false;
        _canBuyFilterOptions.YesChecked = false;
        _canBuyFilterOptions.NoChecked = false;
    }
    #endregion
}
