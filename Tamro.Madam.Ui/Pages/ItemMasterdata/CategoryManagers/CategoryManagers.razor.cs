using BlazorDownloadFile;
using Fluxor;
using MediatR;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using MudBlazor;
using Tamro.Madam.Application.Commands.ItemMasterdata.CategoryManagers.Delete;
using Tamro.Madam.Application.Queries.CategoryManagers.Grid;
using Tamro.Madam.Application.Services.Files;
using Tamro.Madam.Common.Utils;
using Tamro.Madam.Models.Common.Dialog;
using Tamro.Madam.Models.ItemMasterdata.CategoryManagers;
using Tamro.Madam.Repository.Entities.ItemMasterdata.CategoryManagers;
using Tamro.Madam.Ui.ComponentExtensions;
using Tamro.Madam.Ui.Components.Audit;
using Tamro.Madam.Ui.Components.DataGrid.Filtering;
using Tamro.Madam.Ui.Components.Dialogs;
using Tamro.Madam.Ui.Pages.ItemMasterdata.CategoryManagers.Components;
using Tamro.Madam.Ui.Store.State;
using Tamro.Madam.Ui.Utils;

namespace Tamro.Madam.Ui.Pages.ItemMasterdata.CategoryManagers;

public partial class CategoryManagers
{
    #region Properties
    protected int _defaultPageSize;
    protected bool _loading;
    protected HashSet<CategoryManagerModel> _selectedCategoryManagers = new();
    protected CategoryManagersGridQuery _query { get; set; } = new();
    protected MudDataGrid<CategoryManagerModel> _table = new();
    protected DictionaryFilterOptions _countriesFilterOptions = new();
    #endregion

    #region IoC
    [Inject]
    protected UserSettingsUtils _userSettings { get; set; }
    [Inject]
    protected IMediator _mediator { get; set; }
    [Inject]
    protected IExcelService _excelService { get; set; }
    [Inject]
    protected IBlazorDownloadFileService _blazorDownloadFileService { get; set; }
    [Inject]
    protected IState<UserProfileState> _userProfileState { get; set; }

    [Inject]
    protected ISnackbar _snackbar { get; set; }
    #endregion

    #region Events
    protected override async Task OnInitializedAsync()
    {
        _defaultPageSize = _userSettings.GetRowsPerPageSetting();
    }

    protected async Task OnBasicSearchKeyDown(KeyboardEventArgs e)
    {
        if (e.Key == "Enter")
        {
            await _table.ReloadServerData();
        }
    }

    protected async Task OnReset()
    {
        _query = new();
        _countriesFilterOptions.Reset();
        await MudDataGridExtensions<CategoryManagerModel>.ResetGridState(_table);
    }

    protected async Task OnDeleteSingle(CategoryManagerModel manager)
    {
        await OnDelete(new[] { manager.Id }, count: 1);
    }

    protected async Task OnDeleteChecked()
    {
        var idsToDelete = _selectedCategoryManagers.Select(x => x.Id).ToArray();
        await OnDelete(idsToDelete, _selectedCategoryManagers.Count);
    }

    protected async Task OnDelete(int[] ids, int count)
    {
        var command = new DeleteCategoryManagersCommand(ids);
        var parameters = new DialogParameters<ConfirmationDialog>
        {
            { x => x.Command, command },
            { x => x.Title, "Delete category managers" },
            { x => x.Content, $"Are you sure you want to delete {count} category managers?" },
            { x => x.SuccessMessage, $"{count} category managers deleted" },
        };
        var options = new DialogOptions
        {
            CloseButton = true,
            MaxWidth = MaxWidth.Small,
            FullWidth = true,
            CloseOnEscapeKey = true,
        };
        var dialog = DialogService.ShowAsync<ConfirmationDialog>("Delete", parameters, options);
        var state = await dialog;
        var result = await state.Result;

        if (!result!.Canceled)
        {
            await _table.ReloadServerData();
            _selectedCategoryManagers.Clear();
        }
    }

    protected async Task OnCreate()
    {
        await ShowDetailsDialog(new CategoryManagerModel(), DialogState.Create);
    }

    protected async Task OnCopy()
    {
        var manager = _selectedCategoryManagers.First();
        manager.Id = default;
        await ShowDetailsDialog(manager, DialogState.Copy);
    }

    protected async Task OnCopy(CategoryManagerModel manager)
    {
        manager.Id = default;
        await ShowDetailsDialog(manager, DialogState.Copy);
    }

    protected async Task OnOpen()
    {
        await ShowDetailsDialog(_selectedCategoryManagers.First(), DialogState.View);
    }

    protected async Task OnOpen(CategoryManagerModel? manager)
    {
        await ShowDetailsDialog(manager, DialogState.View);
    }

    protected async Task OnExport()
    {
        try
        {
            _loading = true;
            var query = new CategoryManagersGridQuery()
            {
                Filters = _table.FilterDefinitions.ConvertToApplicationLevelFilterDefinitions(),
                OrderBy = _table.SortDefinitions.Values.FirstOrDefault()?.SortBy ?? nameof(CategoryManager.EmailAddress),
                SortDirection = _table.SortDefinitions.Values.FirstOrDefault()?.Descending ?? false ? SortDirection.Descending.ToString() : SortDirection.Ascending.ToString(),
                PageNumber = 1,
                PageSize = int.MaxValue,
                SearchTerm = _query.SearchTerm,
            };
            var exportableManagers = await _mediator.Send(query);

            var mappers = new Dictionary<string, Func<CategoryManagerModel, object?>>
            {
                { DisplayNameHelper.Get(typeof(CategoryManagerModel), nameof(CategoryManagerModel.EmailAddress)), b => b.EmailAddress },
                { DisplayNameHelper.Get(typeof(CategoryManagerModel), nameof(CategoryManagerModel.FirstName)), b => b.FirstName },
                { DisplayNameHelper.Get(typeof(CategoryManagerModel), nameof(CategoryManagerModel.LastName)), b => b.LastName },
                { DisplayNameHelper.Get(typeof(CategoryManagerModel), nameof(CategoryManagerModel.Country)), b => b.Country },
            };
            var fileContent = await _excelService.ExportAsync(exportableManagers.Items, mappers, "ItemCategoryManagers");
            await _blazorDownloadFileService.DownloadFile("ItemCategoryManagers.xlsx", fileContent, "application/octet-stream");
            _snackbar.Add("Category managers exported successfully", Severity.Success);
        }
        catch (Exception)
        {
            _snackbar.Add("Failed to export category managers", Severity.Error);
        }
        finally
        {
            _loading = false;
        }
    }

    protected async Task ShowDetailsDialog(CategoryManagerModel? model, DialogState dialogState)
    {
        var parameters = new DialogParameters<CategoryManagerDetailsDialog>
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
        var dialog = await DialogService.ShowAsync<CategoryManagerDetailsDialog>(string.Empty, parameters, options);
        var state = await dialog.Result;

        await _table.ReloadServerData();
    }

    protected Task OnHistoryView()
    {
        return OnHistoryView(_selectedCategoryManagers.First());
    }

    protected Task OnHistoryView(CategoryManagerModel? manager)
    {
        return ShowHistoryDialog(manager.Id);
    }

    protected async Task ShowHistoryDialog(int id)
    {
        var parameters = new DialogParameters<EntityHistory>
        {
            { x => x.EntityTypeName, nameof(CategoryManager) },
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

        await DialogService.ShowAsync<EntityHistory>($"Category manager {id} history", parameters, options);
    }

    protected async Task<GridData<CategoryManagerModel>> DataSource(GridState<CategoryManagerModel> state)
    {
        try
        {
            _loading = true;
            _query.OrderBy = state.SortDefinitions.FirstOrDefault()?.SortBy ?? nameof(CategoryManager.EmailAddress);
            _query.SortDirection = state.SortDefinitions.FirstOrDefault()?.Descending ?? true ? nameof(SortDirection.Ascending) : nameof(SortDirection.Descending);
            _query.PageNumber = state.Page + 1;
            _query.PageSize = state.PageSize;
            _query.Filters = state.FilterDefinitions.ConvertToApplicationLevelFilterDefinitions();

            var result = await _mediator.Send(_query);

            return new GridData<CategoryManagerModel>()
            {
                TotalItems = result.TotalItems,
                Items = result.Items,
            };
        }
        catch (Exception)
        {
            _snackbar.Add("Failed to load category managers", Severity.Error);
            return new();
        }
        finally
        {
            _loading = false;
        }
    }
    #endregion 
}
