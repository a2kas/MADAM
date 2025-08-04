using BlazorDownloadFile;
using Fluxor;
using MediatR;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using MudBlazor;
using Tamro.Madam.Application.Commands.ItemMasterdata.Items;
using Tamro.Madam.Application.Queries.Items;
using Tamro.Madam.Application.Services.Files;
using Tamro.Madam.Common.Constants.SearchExpressionConstants;
using Tamro.Madam.Common.Utils;
using Tamro.Madam.Models.Common.Dialog;
using Tamro.Madam.Models.ItemMasterdata.Items;
using Tamro.Madam.Repository.Entities.ItemMasterdata.Items;
using Tamro.Madam.Ui.ComponentExtensions;
using Tamro.Madam.Ui.Components.Audit;
using Tamro.Madam.Ui.Components.DataGrid.Filtering;
using Tamro.Madam.Ui.Components.Dialogs;
using Tamro.Madam.Ui.Pages.ItemMasterdata.Items.Components;
using Tamro.Madam.Ui.Store.Actions.ItemMasterdata.Items;
using Tamro.Madam.Ui.Store.State;
using Tamro.Madam.Ui.Utils;

namespace Tamro.Madam.Ui.Pages.ItemMasterdata.Items;

public partial class Items
{
    #region Properties
    private int _defaultPageSize;
    private bool _loading;
    private BooleanFilterOptions _isActiveFilterOptions = new BooleanFilterOptions()
    {
        YesChecked = true,
    };
    private HashSet<ItemGridModel> _selectedItems = new();
    private ItemQuery _query { get; set; } = new();
    private MudDataGrid<ItemGridModel> _table = new();
    private List<IFilterDefinition<ItemGridModel>> _defaultFilterDefinition = new()
    {
        new FilterDefinition<ItemGridModel>()
        {
            Title = nameof(ItemGridModel.Active),
            Operator = SearchBoolConstants.Is,
            Value = true,
        }
    };
    #endregion

    #region IoC
    [Inject]
    private UserSettingsUtils _userSettings { get; set; }
    [Inject]
    private IMediator _mediator { get; set; }
    [Inject]
    private IExcelService _excelService { get; set; }
    [Inject]
    private IBlazorDownloadFileService _blazorDownloadFileService { get; set; }
    [Inject]
    private IState<UserProfileState> _userProfileState { get; set; }
    [Inject]
    private IDispatcher _dispatcher { get; set; }
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
        await ResetIsActiveFilter();
        await MudDataGridExtensions<ItemGridModel>.ResetGridState(_table);
        await _table.AddFilterAsync(new FilterDefinition<ItemGridModel>()
        {
            Title = nameof(ItemGridModel.Active),
            Operator = SearchBoolConstants.Is,
            Value = true,
        });
    }

    private async Task OnCreate()
    {
        _dispatcher.Dispatch(new SetCurrentItemAction()
        {
            Item = new ItemModel()
            {
                Active = true,
            },
            State = DialogState.Create,
            UserProfileState = _userProfileState.Value,
        });
        await ShowDetailsDialog();
    }

    private async Task OnCreateParallel()
    {
        await OnCreateParallel(_selectedItems.First());
    }

    private async Task OnCreateParallel(ItemGridModel? item)
    {
        if (item == null)
        {
            return;
        }

        _loading = true;

        var command = new InitializeParallelItemCommand(item.Id);
        var initParalelItemCommandResult = await _mediator.Send(command);

        if (initParalelItemCommandResult.Succeeded)
        {
            _dispatcher.Dispatch(new SetCurrentItemAction()
            {
                Item = initParalelItemCommandResult.Data,
                State = DialogState.Create,
                UserProfileState = _userProfileState.Value,
            });
            await ShowDetailsDialog();
        }
        else
        {
            Snackbar.Add(initParalelItemCommandResult.Errors?[0], Severity.Error);
        }
    }

    private async Task OnCopy()
    {
        await OnCopy(_selectedItems.First());
    }

    private async Task OnCopy(ItemGridModel? item)
    {
        if (item == null)
        {
            return;
        }

        _loading = true;

        var command = new CopyItemCommand(item.Id);
        var result = await _mediator.Send(command);

        _loading = false;

        if (result.Succeeded)
        {
            _dispatcher.Dispatch(new SetCurrentItemAction()
            {
                Item = result.Data,
                State = DialogState.Copy,
                UserProfileState = _userProfileState.Value,
            });
            await ShowDetailsDialog();
        }
        else
        {
            Snackbar.Add(result.Errors?[0], Severity.Error);
        }
    }

    private async Task OnOpen()
    {
        await OnOpen(_selectedItems.First());
    }

    private async Task OnOpen(ItemGridModel? item)
    {
        if (item == null)
        {
            return;
        }

        _loading = true;

        var command = new GetItemCommand(item.Id);
        var itemResult = await _mediator.Send(command);

        _loading = false;

        if (itemResult.Succeeded)
        {
            _dispatcher.Dispatch(new SetCurrentItemAction()
            {
                Item = itemResult.Data,
                State = DialogState.View,
                UserProfileState = _userProfileState.Value,
            });
            await ShowDetailsDialog();
        }
        else
        {
            Snackbar.Add(itemResult.Errors?[0], Severity.Error);
        }
    }

    private async Task OnExport()
    {
        try
        {
            _loading = true;
            var query = new ItemQuery()
            {
                Filters = _table.FilterDefinitions,
                OrderBy = _table.SortDefinitions.Values.FirstOrDefault()?.SortBy ?? nameof(ItemGridModel.ItemName),
                SortDirection = _table.SortDefinitions.Values.FirstOrDefault()?.Descending ?? false ? SortDirection.Descending.ToString() : SortDirection.Ascending.ToString(),
                PageNumber = 1,
                PageSize = int.MaxValue,
                SearchTerm = _query.SearchTerm,
            };
            var exportableItems = await _mediator.Send(query);

            var mappers = new Dictionary<string, Func<ItemGridModel, object?>>
            {
                { DisplayNameHelper.Get(typeof(ItemGridModel), nameof(ItemGridModel.Id)), x => x.Id },
                { DisplayNameHelper.Get(typeof(ItemGridModel), nameof(ItemGridModel.ItemName)), x => x.ItemName },
                { DisplayNameHelper.Get(typeof(ItemGridModel), nameof(ItemGridModel.Description)), x => x.Description },
                { DisplayNameHelper.Get(typeof(ItemGridModel), nameof(ItemGridModel.Producer)), x => x.Producer },
                { DisplayNameHelper.Get(typeof(ItemGridModel), nameof(ItemGridModel.Brand)), x => x.Brand },
                { DisplayNameHelper.Get(typeof(ItemGridModel), nameof(ItemGridModel.Strength)), x => x.Strength },
                { DisplayNameHelper.Get(typeof(ItemGridModel), nameof(ItemGridModel.Form)), x => x.Form },
                { DisplayNameHelper.Get(typeof(ItemGridModel), nameof(ItemGridModel.Measure)), x => x.Measure },
                { DisplayNameHelper.Get(typeof(ItemGridModel), nameof(ItemGridModel.MeasurementUnit)), x => x.MeasurementUnit },
                { DisplayNameHelper.Get(typeof(ItemGridModel), nameof(ItemGridModel.AtcCode)), x => x.AtcCode },
                { DisplayNameHelper.Get(typeof(ItemGridModel), nameof(ItemGridModel.AtcName)), x => x.AtcName },
                { DisplayNameHelper.Get(typeof(ItemGridModel), nameof(ItemGridModel.SupplierNick)), x => x.SupplierNick },
                { DisplayNameHelper.Get(typeof(ItemGridModel), nameof(ItemGridModel.Numero)), x => x.Numero },
                { DisplayNameHelper.Get(typeof(ItemGridModel), nameof(ItemGridModel.ActiveSubstance)), x => x.ActiveSubstance },
                { DisplayNameHelper.Get(typeof(ItemGridModel), nameof(ItemGridModel.Active)), x => x.Active },
            };
            var fileContent = await _excelService.ExportAsync(exportableItems.Items, mappers, "Items");
            await _blazorDownloadFileService.DownloadFile("Items.xlsx", fileContent, "application/octet-stream");
            Snackbar.Add("Items exported successfully", Severity.Success);
        }
        catch (Exception)
        {
            Snackbar.Add("Failed to export items", Severity.Error);
        }
        finally
        {
            _loading = false;
        }
    }

    private async Task OnActivate(ItemGridModel item)
    {
        await ActivateItems(new[] { item.Id }, count: 1);
    }

    private async Task OnActivate()
    {
        var idsToActivate = _selectedItems.Select(x => x.Id).ToArray();
        await ActivateItems(idsToActivate, _selectedItems.Count);
    }

    private async Task ActivateItems(int[] ids, int count)
    {
        var command = new ActivateItemsCommand(ids, _userProfileState.Value.UserProfile);

        var parameters = new DialogParameters<ConfirmationDialog>
        {
            { x => x.Command, command },
            { x => x.Title, "Activate items" },
            { x => x.Content, $"Are you sure you want to activate {count} item(s)?" },
            { x => x.SuccessMessage, $"{count} item(s) activated" },
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
            _selectedItems.Clear();
        }
    }

    private async Task OnHistoryView()
    {
        await OnHistoryView(_selectedItems.First());
    }

    private async Task OnHistoryView(ItemGridModel? item)
    {
        await ShowHistoryDialog(item.Id);
    }

    private async Task ShowHistoryDialog(int id)
    {
        var parameters = new DialogParameters<EntityHistory>
        {
            { x => x.EntityTypeName, nameof(Item) },
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

        await DialogService.ShowAsync<EntityHistory>($"Item {id} history", parameters, options);
    }

    private async Task OnDeactivate(ItemGridModel item)
    {
        await DeactivateItems(new[] { item.Id }, count: 1);
    }

    private async Task OnDeactivate()
    {
        var idsToActivate = _selectedItems.Select(x => x.Id).ToArray();
        await DeactivateItems(idsToActivate, _selectedItems.Count);
    }

    private async Task DeactivateItems(int[] ids, int count)
    {
        var command = new DeactivateItemsCommand(ids, _userProfileState.Value.UserProfile);

        var parameters = new DialogParameters<ConfirmationDialog>
        {
            { x => x.Command, command },
            { x => x.Title, "Deactivate items" },
            { x => x.Content, $"Are you sure you want to deactivate {count} item(s)?" },
            { x => x.SuccessMessage, $"{count} item(s) deactivated" },
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
            _selectedItems.Clear();
        }
    }

    private async Task ShowDetailsDialog()
    {
        var options = new DialogOptions
        {
            Position = DialogPosition.TopCenter,
            CloseButton = true,
            MaxWidth = MaxWidth.Large,
            CloseOnEscapeKey = true,
            FullWidth = true,
        };
        var dialog = DialogService.Show<ItemDetailsDialog>(string.Empty, new DialogParameters(), options);
        var state = await dialog.Result;

        await _table.ReloadServerData();
    }

    private async Task<GridData<ItemGridModel>> DataSource(GridState<ItemGridModel> state)
    {
        try
        {
            _loading = true;
            MudDataGridExtensions<ItemGridModel>.ApplyDefaultFilterColumns(state.FilterDefinitions, _table.RenderedColumns);
            _query.OrderBy = state.SortDefinitions.FirstOrDefault()?.SortBy ?? nameof(ItemGridModel.ItemName);
            _query.SortDirection = state.SortDefinitions.FirstOrDefault()?.Descending ?? true ? nameof(SortDirection.Ascending) : nameof(SortDirection.Descending);
            _query.PageNumber = state.Page + 1;
            _query.PageSize = state.PageSize;
            _query.Filters = state.FilterDefinitions;

            var result = await _mediator.Send(_query);

            return new GridData<ItemGridModel>()
            {
                TotalItems = result.TotalItems,
                Items = result.Items,
            };
        }
        catch (Exception)
        {
            Snackbar.Add("Failed to load items", Severity.Error);
            return new();
        }
        finally
        {
            _loading = false;
        }
    }

    private async Task ApplyIsActiveFilter()
    {
        var filter = _table.FilterDefinitions.Find(x => x.Title == nameof(ItemGridModel.Active));
        if (filter != null)
        {
            _table.FilterDefinitions.Remove(filter);
        }
        _isActiveFilterOptions.IsOpen = false;
        if (_isActiveFilterOptions.YesChecked != _isActiveFilterOptions.NoChecked)
        {
            await _table.AddFilterAsync(new FilterDefinition<ItemGridModel>()
            {
                Title = nameof(ItemGridModel.Active),
                Operator = SearchBoolConstants.Is,
                Value = _isActiveFilterOptions.YesChecked,
            });
        }
        await _table.ReloadServerData();
    }

    private async Task ClearIsActiveFilter()
    {
        var filter = _table.FilterDefinitions.Find(x => x.Title == nameof(ItemGridModel.Active));
        if (filter != null)
        {
            _table.FilterDefinitions.Remove(filter);
            await _table.ReloadServerData();
        }
        _isActiveFilterOptions.IsOpen = false;
        _isActiveFilterOptions.YesChecked = false;
        _isActiveFilterOptions.NoChecked = false;
    }

    private async Task ResetIsActiveFilter()
    {
        var filter = _table.FilterDefinitions.Find(x => x.Title == nameof(ItemGridModel.Active));
        if (filter != null)
        {
            _table.FilterDefinitions.Remove(filter);
            await _table.ReloadServerData();
        }
        _isActiveFilterOptions.IsOpen = false;
        _isActiveFilterOptions.YesChecked = true;
        _isActiveFilterOptions.NoChecked = false;
    }
    #endregion 
}
