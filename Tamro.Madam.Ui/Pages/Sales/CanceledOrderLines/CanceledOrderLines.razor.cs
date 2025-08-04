using BlazorDownloadFile;
using Fluxor;
using MediatR;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using MudBlazor;
using Tamro.Madam.Application.Queries.Sales.CanceledOrderLines;
using Tamro.Madam.Application.Services.Files;
using Tamro.Madam.Common.Utils;
using Tamro.Madam.Models.Data.Sales.CanceledOrderLines;
using Tamro.Madam.Models.Sales.CanceledOrderLines;
using Tamro.Madam.Ui.ComponentExtensions;
using Tamro.Madam.Ui.Components.DataGrid.Filtering;
using Tamro.Madam.Ui.Components.Dialogs;
using Tamro.Madam.Ui.Store.State;
using Tamro.Madam.Ui.Utils;

namespace Tamro.Madam.Ui.Pages.Sales.CanceledOrderLines;

public partial class CanceledOrderLines
{
    #region Properties
    private int _defaultPageSize;
    private bool _loading;
    private CanceledOrderHeaderQuery _query { get; set; } = new();
    private MudDataGrid<CanceledOrderHeaderGridModel> _table = new();
    private DictionaryFilterOptions _notificationStatusFilterOptions = new();
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
        await ClearNotificationStatusFilter();
        await MudDataGridExtensions<CanceledOrderHeaderGridModel>.ResetGridState(_table);
    }

    private async Task OnExport()
    {
        try
        {
            _loading = true;

            var parameters = new DialogParameters<ExportDateRangeDialog>
            {
                { x => x.DateRange, new DateRange(DateTime.Now.AddDays(-30).Date, DateTime.Now.AddDays(1).Date) },
                { x => x.MaximumAllowedRange, TimeSpan.FromDays(31)}
            };
            var options = new DialogOptions
            {
                CloseButton = true,
                MaxWidth = MaxWidth.Medium,
                CloseOnEscapeKey = true,
                FullWidth = true,
                Position = DialogPosition.TopCenter,
            };
            var dialog = await DialogService.ShowAsync<ExportDateRangeDialog>(string.Empty, parameters, options);
            var state = await dialog.Result;
            if (state.Canceled)
            {
                return;
            }

            var exportDateRange = state.Data as DateRange;

            var query = new CanceledOrderHeaderQuery()
            {
                Country = _userProfileState.Value.UserProfile.Country,
                Filters = _table.FilterDefinitions,
                OrderBy = _table.SortDefinitions.Values.FirstOrDefault()?.SortBy ?? nameof(CanceledOrderHeaderGridModel.OrderDate),
                SortDirection = _table.SortDefinitions.Values.FirstOrDefault()?.Descending ?? true ? nameof(SortDirection.Descending) : nameof(SortDirection.Ascending),
                OrderDateFrom = exportDateRange.Start,
                OrderDateTo = exportDateRange.End,
                PageNumber = 1,
                SearchTerm = _query.SearchTerm,
                PageSize = int.MaxValue,
            };
            var exportableCanceledOrderHeaders = await _mediator.Send(query);

            var mappers = new Dictionary<string, Func<CanceledOrderHeaderGridModel, object?>>
            {
                { DisplayNameHelper.Get(typeof(CanceledOrderHeaderGridModel), nameof(CanceledOrderHeaderGridModel.OrderDate)), b => b.OrderDate.ToString("yyyy-MM-dd") },
                { DisplayNameHelper.Get(typeof(CanceledOrderHeaderGridModel), nameof(CanceledOrderHeaderGridModel.E1ShipTo)), b => b.E1ShipTo },
                { DisplayNameHelper.Get(typeof(CanceledOrderHeaderGridModel), nameof(CanceledOrderHeaderGridModel.CustomerName)), b => b.CustomerName },
                { DisplayNameHelper.Get(typeof(CanceledOrderHeaderGridModel), nameof(CanceledOrderHeaderGridModel.CustomerOrderNo)), b => b.CustomerOrderNo },
                { DisplayNameHelper.Get(typeof(CanceledOrderHeaderGridModel), nameof(CanceledOrderHeaderGridModel.DocumentNo)), b => b.DocumentNo },
                { DisplayNameHelper.Get(typeof(CanceledOrderHeaderGridModel), nameof(CanceledOrderHeaderGridModel.EmailStatus)), b => CanceledOrderHeaderData.EmailNotificationStatus[b.EmailStatus.ToString()] },
            };
            var fileContent = await _excelService.ExportAsync(exportableCanceledOrderHeaders.Items, mappers, "CanceledOrders");
            await _blazorDownloadFileService.DownloadFile("CanceledOrders.xlsx", fileContent, "application/octet-stream");
            Snackbar.Add("Canceled orders exported successfully", Severity.Success);
        }
        catch (Exception)
        {
            Snackbar.Add("Failed to export canceled orders", Severity.Error);
        }
        finally
        {
            _loading = false;
        }
    }

    private async Task<GridData<CanceledOrderHeaderGridModel>> DataSource(GridState<CanceledOrderHeaderGridModel> state)
    {
        try
        {
            _loading = true;
            MudDataGridExtensions<CanceledOrderHeaderGridModel>.ApplyDefaultFilterColumns(state.FilterDefinitions, _table.RenderedColumns);
            _query.OrderBy = state.SortDefinitions.FirstOrDefault()?.SortBy ?? nameof(CanceledOrderHeaderGridModel.OrderDate);
            _query.SortDirection = state.SortDefinitions.FirstOrDefault()?.Descending ?? true ? nameof(SortDirection.Descending) : nameof(SortDirection.Ascending);
            _query.PageNumber = state.Page + 1;
            _query.PageSize = state.PageSize;
            _query.Filters = state.FilterDefinitions;
            _query.Country = _userProfileState.Value.UserProfile.Country;

            var result = await _mediator.Send(_query);

            return new GridData<CanceledOrderHeaderGridModel>()
            {
                TotalItems = result.TotalItems,
                Items = result.Items,
            };
        }
        catch (Exception)
        {
            Snackbar.Add("Failed to load canceled orders", Severity.Error);
            return new();
        }
        finally
        {
            _loading = false;
        }
    }

    private async Task ClearNotificationStatusFilter()
    {
        var filter = _table.FilterDefinitions.Find(x => x.Column.Title == nameof(CanceledOrderLineGridModel.EmailStatus));
        if (filter != null)
        {
            _table.FilterDefinitions.Remove(filter);
            await _table.ReloadServerData();
        }
        _notificationStatusFilterOptions.IsOpen = false;
        _notificationStatusFilterOptions.SelectAllChecked = false;
        _notificationStatusFilterOptions.SelectedOptions = [];
    }

    private async Task ApplyNotificationStatusFilter()
    {
        _query.EmailStatus = _notificationStatusFilterOptions.SelectedOptions
                .Select(pair => Enum.Parse<CanceledOrderHeaderEmailStatus>(pair.Key))
                .ToHashSet();
        _notificationStatusFilterOptions.IsOpen = false;
        await _table.ReloadServerData();
    }
    #endregion
}
