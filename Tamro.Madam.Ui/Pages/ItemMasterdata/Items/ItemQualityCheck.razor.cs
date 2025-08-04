using BlazorDownloadFile;
using MediatR;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using MudBlazor;
using Tamro.Madam.Application.Queries.Items.QualityCheck;
using Tamro.Madam.Application.Services.Files;
using Tamro.Madam.Common.Utils;
using Tamro.Madam.Models.ItemMasterdata.Items.QualityCheck;
using Tamro.Madam.Ui.ComponentExtensions;
using Tamro.Madam.Ui.Components.DataGrid.Filtering;
using Tamro.Madam.Ui.Utils;

namespace Tamro.Madam.Ui.Pages.ItemMasterdata.Items;

public partial class ItemQualityCheck
{
    #region Properties
    private int _defaultPageSize;
    private bool _loading;
    private ItemQualityCheckQuery _query { get; set; } = new();
    private MudDataGrid<ItemQualityCheckGridModel> _table = new();
    private DictionaryFilterOptions _severityFilterOptions = new();
    private DictionaryFilterOptions _statusFilterOptions = new();
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
        await MudDataGridExtensions<ItemQualityCheckGridModel>.ResetGridState(_table);
    }

    private async Task OnExport()
    {
        try
        {
            _loading = true;
            var query = new ItemQualityCheckQuery()
            {
                Filters = _table.FilterDefinitions,
                OrderBy = _table.SortDefinitions.Values.FirstOrDefault()?.SortBy ?? nameof(ItemQualityCheckGridModel.ScanDate),
                SortDirection = _table.SortDefinitions.Values.FirstOrDefault()?.Descending ?? true ? SortDirection.Descending.ToString() : SortDirection.Ascending.ToString(),
                PageNumber = 1,
                PageSize = int.MaxValue,
                SearchTerm = _query.SearchTerm,
            };
            var exportableResult = await _mediator.Send(query);

            var mappers = new Dictionary<string, Func<ItemQualityCheckGridModel, object?>>
            {
                { DisplayNameHelper.Get(typeof(ItemQualityCheckGridModel), nameof(ItemQualityCheckGridModel.Id)), x => x.Id },
                { DisplayNameHelper.Get(typeof(ItemQualityCheckGridModel), nameof(ItemQualityCheckGridModel.ItemName)), x => x.ItemName },
                { DisplayNameHelper.Get(typeof(ItemQualityCheckGridModel), nameof(ItemQualityCheckGridModel.IssueCount)), x => x.IssueCount },
                { DisplayNameHelper.Get(typeof(ItemQualityCheckGridModel), nameof(ItemQualityCheckGridModel.UnresolvedIssuesCount)), x => x.UnresolvedIssuesCount },
                { DisplayNameHelper.Get(typeof(ItemQualityCheckGridModel), nameof(ItemQualityCheckGridModel.UnresolvedSeverities)), x => string.Join(",", x.UnresolvedSeverities) },
                { DisplayNameHelper.Get(typeof(ItemQualityCheckGridModel), nameof(ItemQualityCheckGridModel.Status)), x => x.Status },
                { DisplayNameHelper.Get(typeof(ItemQualityCheckGridModel), nameof(ItemQualityCheckGridModel.ScanDate)), x => x.ScanDate },
            };
            var fileContent = await _excelService.ExportAsync(exportableResult.Items, mappers, "Item masterdata quality check");
            await _blazorDownloadFileService.DownloadFile("ItemMasterdataQualityCheck.xlsx", fileContent, "application/octet-stream");
            Snackbar.Add("Exported successfully", Severity.Success);
        }
        catch (Exception)
        {
            Snackbar.Add("Failed to export", Severity.Error);
        }
        finally
        {
            _loading = false;
        }
    }

    private async Task<GridData<ItemQualityCheckGridModel>> DataSource(GridState<ItemQualityCheckGridModel> state)
    {
        try
        {
            _loading = true;
            MudDataGridExtensions<ItemQualityCheckGridModel>.ApplyDefaultFilterColumns(state.FilterDefinitions, _table.RenderedColumns);
            _query.OrderBy = state.SortDefinitions.FirstOrDefault()?.SortBy ?? nameof(ItemQualityCheckGridModel.ScanDate);
            _query.SortDirection = state.SortDefinitions.FirstOrDefault()?.Descending ?? true ? nameof(SortDirection.Descending) : nameof(SortDirection.Ascending);
            _query.PageNumber = state.Page + 1;
            _query.PageSize = state.PageSize;
            _query.Filters = state.FilterDefinitions;

            var result = await _mediator.Send(_query);

            return new GridData<ItemQualityCheckGridModel>()
            {
                TotalItems = result.TotalItems,
                Items = result.Items,
            };
        }
        catch (Exception)
        {
            Snackbar.Add("Failed to load item quality check", Severity.Error);
            return new();
        }
        finally
        {
            _loading = false;
        }
    }

    private async Task ClearSeverityFilter()
    {
        var filter = _table.FilterDefinitions.Find(x => x.Column.Title == nameof(ItemQualityCheckGridModel.UnresolvedSeverities));
        if (filter != null)
        {
            _table.FilterDefinitions.Remove(filter);
            await _table.ReloadServerData();
        }
        _severityFilterOptions.IsOpen = false;
        _severityFilterOptions.SelectAllChecked = false;
        _severityFilterOptions.SelectedOptions = [];
    }

    private async Task ClearStatusFilter()
    {
        var filter = _table.FilterDefinitions.Find(x => x.Column.Title == nameof(ItemQualityCheckGridModel.Status));
        if (filter != null)
        {
            _table.FilterDefinitions.Remove(filter);
            await _table.ReloadServerData();
        }
        _statusFilterOptions.IsOpen = false;
        _statusFilterOptions.SelectAllChecked = false;
        _statusFilterOptions.SelectedOptions = [];
    }

    private async Task ApplySeverityFilter()
    {
        _query.UnresolvedSeverities = _severityFilterOptions.SelectedOptions
                .Select(pair => Enum.Parse<ItemQualityIssueSeverity>(pair.Key))
                .ToHashSet();
        _severityFilterOptions.IsOpen = false;
        await _table.ReloadServerData();
    }

    private async Task ApplyStatusFilter()
    {
        _query.Statuses = _statusFilterOptions.SelectedOptions
                .Select(pair => Enum.Parse<ItemQualityIssueStatus>(pair.Key))
                .ToHashSet();
        _severityFilterOptions.IsOpen = false;
        await _table.ReloadServerData();
    }
    #endregion 
}
