using BlazorDownloadFile;
using Fluxor;
using MediatR;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using MudBlazor;
using Tamro.Madam.Application.Commands.ItemMasterdata.Items.SafetyStocks.PharmacyChains;
using Tamro.Madam.Application.Queries.Items.SafetyStocks.PharmacyChains;
using Tamro.Madam.Application.Services.Files;
using Tamro.Madam.Common.Constants.SearchExpressionConstants;
using Tamro.Madam.Common.Utils;
using Tamro.Madam.Models.Common.Dialog;
using Tamro.Madam.Models.General;
using Tamro.Madam.Models.ItemMasterdata.Items.SafetyStocks.PharmacyChains;
using Tamro.Madam.Repository.Entities.ItemMasterdata.Items.SafetyStocks;
using Tamro.Madam.Ui.ComponentExtensions;
using Tamro.Madam.Ui.Components.Audit;
using Tamro.Madam.Ui.Components.DataGrid.Filtering;
using Tamro.Madam.Ui.Components.Dialogs;
using Tamro.Madam.Ui.Pages.SafetyStock.PharmacyChains.Components;
using Tamro.Madam.Ui.Store.State;
using Tamro.Madam.Ui.Utils;

namespace Tamro.Madam.Ui.Pages.SafetyStock.PharmacyChains;

public partial class PharmacyChains
{
    #region Properties
    private int _defaultPageSize;
    private bool _loading;

    private DictionaryFilterOptions _groupFilterOptions = new();

    private BooleanFilterOptions _isActiveFilterOptions = new BooleanFilterOptions();

    private HashSet<PharmacyChainModel> _selectedPharmacyChains = new();
    private PharmacyChainQuery _query { get; set; } = new();
    private MudDataGrid<PharmacyChainModel> _table = new();
    private List<IFilterDefinition<PharmacyChainModel>> _defaultFilterDefinition = new()
    {
        new FilterDefinition<PharmacyChainModel>()
        {
            Title = nameof(PharmacyChainModel.IsActive),
            Operator = SearchBoolConstants.Is,
            Value = null,
        }
    };
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
    #endregion

    #region Events
    protected override async Task OnInitializedAsync()
    {
        _defaultPageSize = _userSettings.GetRowsPerPageSetting();
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
        await MudDataGridExtensions<PharmacyChainModel>.ResetGridState(_table);
        await ClearIsActiveFilter();
        await ClearGroupFilter();
    }

    private async Task OnActivate(PharmacyChainModel pharmacyChain)
    {
        await ActivatePharmacyChains(new[] { pharmacyChain.Id }, count: 1);
    }

    private async Task OnActivate()
    {
        var idsToActivate = _selectedPharmacyChains.Select(x => x.Id).ToArray();
        await ActivatePharmacyChains(idsToActivate, _selectedPharmacyChains.Count);
    }

    private async Task ActivatePharmacyChains(int[] ids, int count)
    {
        var command = new ActivatePharmacyChainCommand(ids);
        var parameters = new DialogParameters<ConfirmationDialog>
        {
            { x => x.Command, command },
            { x => x.Title, "Activate pharmacy chains" },
            { x => x.Content, $"Are you sure you want to activate {count} pharmacy chain(s)?" },
            { x => x.SuccessMessage, $"{count} pharmacy chain(s) activated" },
            { x => x.SubmitIcon, Icons.Material.Outlined.ToggleOn },
            { x => x.SubmitText, "Yes" },
            { x => x.SubmitVariant, Variant.Filled },
            { x => x.SubmitColor, Color.Primary }
        };

        var options = new DialogOptions
        {
            CloseButton = true,
            MaxWidth = MaxWidth.Small,
            FullWidth = true,
            CloseOnEscapeKey = true,
        };
        var dialog = DialogService.Show<ConfirmationDialog>("Activate", parameters, options);
        var state = await dialog.Result;

        if (!state.Canceled)
        {
            await _table.ReloadServerData();
            _selectedPharmacyChains.Clear();
        }
    }

    private async Task OnDeactivate(PharmacyChainModel pharmacyChain)
    {
        await DeactivatePharmacyChains(new[] { pharmacyChain.Id }, count: 1);
    }

    private async Task OnDeactivate()
    {
        var idsToDeactivate = _selectedPharmacyChains.Select(x => x.Id).ToArray();
        await DeactivatePharmacyChains(idsToDeactivate, _selectedPharmacyChains.Count);
    }

    private async Task DeactivatePharmacyChains(int[] ids, int count)
    {
        var command = new DeactivatePharmacyChainCommand(ids);
        var parameters = new DialogParameters<ConfirmationDialog>
        {
            { x => x.Command, command },
            { x => x.Title, "Deactivate pharmacy chains" },
            { x => x.Content, $"Are you sure you want to deactivate {count} pharmacy chain(s)?" },
            { x => x.SuccessMessage, $"{count} pharmacy chain(s) deactivated" },
            { x => x.SubmitIcon, Icons.Material.Outlined.ToggleOff },
            { x => x.SubmitText, "Yes" },
            { x => x.SubmitVariant, Variant.Filled },
            { x => x.SubmitColor, Color.Primary }
        };

        var options = new DialogOptions
        {
            CloseButton = true,
            MaxWidth = MaxWidth.Small,
            FullWidth = true,
            CloseOnEscapeKey = true,
        };
        var dialog = DialogService.Show<ConfirmationDialog>("Deactivate", parameters, options);
        var state = await dialog.Result;

        if (!state.Canceled)
        {
            await _table.ReloadServerData();
            _selectedPharmacyChains.Clear();
        }
    }

    private async Task OnCreate()
    {
        await ShowDetailsDialog(new PharmacyChainModel() { Group = PharmacyGroup.NonBenu, }, DialogState.Create);
    }

    private async Task OnCopy()
    {
        var pharmacyChains = _selectedPharmacyChains.First();
        pharmacyChains.Id = default;
        await ShowDetailsDialog(pharmacyChains, DialogState.Copy);
    }

    private async Task OnCopy(PharmacyChainModel pharmacyChains)
    {
        pharmacyChains.Id = default;
        await ShowDetailsDialog(pharmacyChains, DialogState.Copy);
    }

    private async Task OnOpen()
    {
        await ShowDetailsDialog(_selectedPharmacyChains.First(), DialogState.View);
    }

    private async Task OnOpen(PharmacyChainModel? pharmacyChains)
    {
        await ShowDetailsDialog(pharmacyChains, DialogState.View);
    }

    private async Task OnExport()
    {
        try
        {
            _loading = true;
            var query = new PharmacyChainQuery()
            {
                Filters = _table.FilterDefinitions,
                OrderBy = _table.SortDefinitions.Values.FirstOrDefault()?.SortBy ?? nameof(PharmacyChainModel.DisplayName),
                SortDirection = _table.SortDefinitions.Values.FirstOrDefault()?.Descending ?? false ? SortDirection.Descending.ToString() : SortDirection.Ascending.ToString(),
                PageNumber = 1,
                SearchTerm = _query.SearchTerm,
                Country = _userProfileState.Value.UserProfile.Country,
                PageSize = int.MaxValue,
            };
            var exportablePharmacyChains = await _mediator.Send(query);

            var mappers = new Dictionary<string, Func<PharmacyChainModel, object?>>
            {
                { DisplayNameHelper.Get(typeof(PharmacyChainModel), nameof(PharmacyChainModel.Country)), b => b.Country },
                { DisplayNameHelper.Get(typeof(PharmacyChainModel), nameof(PharmacyChainModel.DisplayName)), b => b.DisplayName },
                { DisplayNameHelper.Get(typeof(PharmacyChainModel), nameof(PharmacyChainModel.Group)), b => b.Group },
                { DisplayNameHelper.Get(typeof(PharmacyChainModel), nameof(PharmacyChainModel.IsActive)), b => b.IsActive },
            };
            var fileContent = await _excelService.ExportAsync(exportablePharmacyChains.Items, mappers, "PharmacyChains");
            await _blazorDownloadFileService.DownloadFile("PharmacyChains.xlsx", fileContent, "application/octet-stream");
            Snackbar.Add("Pharmacy chains exported successfully", Severity.Success);
        }
        catch (Exception)
        {
            Snackbar.Add("Failed to export pharmacy chains", Severity.Error);
        }
        finally
        {
            _loading = false;
        }
    }

    private async Task ShowDetailsDialog(PharmacyChainModel? model, DialogState dialogState)
    {
        var parameters = new DialogParameters<PharmacyChainsDetailsDialog>
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
        var dialog = DialogService.Show<PharmacyChainsDetailsDialog>(string.Empty, parameters, options);
        var state = await dialog.Result;

        await _table.ReloadServerData();
    }

    private async Task OnHistoryView()
    {
        await OnHistoryView(_selectedPharmacyChains.First());
    }

    private async Task OnHistoryView(PharmacyChainModel? pharmacyChain)
    {
        await ShowHistoryDialog(pharmacyChain.Id);
    }

    private async Task ShowHistoryDialog(int id)
    {
        var parameters = new DialogParameters<EntityHistory>
        {
            { x => x.EntityTypeName, nameof(SafetyStockPharmacyChain) },
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

        await DialogService.ShowAsync<EntityHistory>($"Pharmacy chain {id} history", parameters, options);
    }

    private async Task<GridData<PharmacyChainModel>> DataSource(GridState<PharmacyChainModel> state)
    {
        try
        {
            _loading = true;
            MudDataGridExtensions<PharmacyChainModel>.ApplyDefaultFilterColumns(state.FilterDefinitions, _table.RenderedColumns);
            _query.OrderBy = state.SortDefinitions.FirstOrDefault()?.SortBy ?? nameof(PharmacyChainModel.DisplayName);
            _query.SortDirection = state.SortDefinitions.FirstOrDefault()?.Descending ?? true ? nameof(SortDirection.Ascending) : nameof(SortDirection.Descending);
            _query.PageNumber = state.Page + 1;
            _query.PageSize = state.PageSize;
            _query.Filters = state.FilterDefinitions;
            _query.Country = _userProfileState.Value.UserProfile.Country;

            var result = await _mediator.Send(_query);

            return new GridData<PharmacyChainModel>()
            {
                TotalItems = result.TotalItems,
                Items = result.Items,
            };
        }
        catch (Exception)
        {
            Snackbar.Add("Failed to load pharmacy chains", Severity.Error);
            return new();
        }
        finally
        {
            _loading = false;
        }
    }

    private async Task ApplyIsActiveFilter()
    {
        var filter = _table.FilterDefinitions.Find(x => x.Title == nameof(PharmacyChainModel.IsActive));
        if (filter != null)
        {
            _table.FilterDefinitions.Remove(filter);
        }
        _isActiveFilterOptions.IsOpen = false;
        if (_isActiveFilterOptions.YesChecked != _isActiveFilterOptions.NoChecked)
        {
            await _table.AddFilterAsync(new FilterDefinition<PharmacyChainModel>()
            {
                Title = nameof(PharmacyChainModel.IsActive),
                Operator = SearchBoolConstants.Is,
                Value = _isActiveFilterOptions.YesChecked,
            });
        }
        await _table.ReloadServerData();
    }

    private async Task ClearIsActiveFilter()
    {
        var filter = _table.FilterDefinitions.Find(x => x.Title == nameof(PharmacyChainModel.IsActive));
        if (filter != null)
        {
            _table.FilterDefinitions.Remove(filter);
            await _table.ReloadServerData();
        }
        _isActiveFilterOptions.IsOpen = false;
        _isActiveFilterOptions.YesChecked = false;
        _isActiveFilterOptions.NoChecked = false;
    }

    private async Task ClearGroupFilter()
    {
        var filter = _table.FilterDefinitions.Find(x => x.Column.Title == nameof(PharmacyChainModel.Group));
        if (filter != null)
        {
            _table.FilterDefinitions.Remove(filter);
            await _table.ReloadServerData();
        }
        _groupFilterOptions.Reset();
    }
    #endregion
}