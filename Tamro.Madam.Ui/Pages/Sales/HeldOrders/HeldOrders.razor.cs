using BlazorDownloadFile;
using Fluxor;
using MediatR;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using MudBlazor;
using Tamro.Madam.Application.Queries.Sales.HeldOrders;
using Tamro.Madam.Application.Services.Files;
using Tamro.Madam.Common.Utils;
using Tamro.Madam.Models.Sales.HeldOrders;
using Tamro.Madam.Ui.ComponentExtensions;
using Tamro.Madam.Ui.Components.DataGrid.Filtering;
using Tamro.Madam.Ui.Store.State;
using Tamro.Madam.Ui.Utils;

namespace Tamro.Madam.Ui.Pages.Sales.HeldOrders;

public partial class HeldOrders
{
    #region Properties
    private int _defaultPageSize;
    private bool _loading;
    private HeldOrderQuery _query { get; set; } = new();
    private MudDataGrid<HeldOrderGridModel> _table = new();
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
        await MudDataGridExtensions<HeldOrderGridModel>.ResetGridState(_table);
    }


    private async Task<GridData<HeldOrderGridModel>> DataSource(GridState<HeldOrderGridModel> state)
    {
        try
        {
            _loading = true;
            MudDataGridExtensions<HeldOrderGridModel>.ApplyDefaultFilterColumns(state.FilterDefinitions, _table.RenderedColumns);
            _query.OrderBy = state.SortDefinitions.FirstOrDefault()?.SortBy ?? nameof(HeldOrderGridModel.OrderDate);
            _query.SortDirection = state.SortDefinitions.FirstOrDefault()?.Descending ?? true ? nameof(SortDirection.Descending) : nameof(SortDirection.Ascending);
            _query.PageNumber = state.Page + 1;
            _query.PageSize = state.PageSize;
            _query.Filters = state.FilterDefinitions;
            _query.Country = _userProfileState.Value.UserProfile.Country;

            var result = await _mediator.Send(_query);

            return new GridData<HeldOrderGridModel>
            {
                TotalItems = result.TotalItems,
                Items = result.Items,
            };
        }
        catch (Exception ex)
        {
            Snackbar.Add("Failed to load held orders", Severity.Error);
            return new();
        }
        finally
        {
            _loading = false;
        }
    }

    private async Task OnExport()
    {
        try
        {
            _loading = true;
            var query = new HeldOrderQuery
            {
                Country = _userProfileState.Value.UserProfile.Country,
                Filters = _table.FilterDefinitions,
                OrderBy = _table.SortDefinitions.Values.FirstOrDefault()?.SortBy ?? nameof(HeldOrderGridModel.OrderDate),
                SortDirection = _table.SortDefinitions.Values.FirstOrDefault()?.Descending ?? false ? SortDirection.Descending.ToString() : SortDirection.Ascending.ToString(),
                PageNumber = 1,
                PageSize = int.MaxValue,
                SearchTerm = _query.SearchTerm,
            };

            var exportHeldOrders = await _mediator.Send(query);

            var mappers = new Dictionary<string, Func<HeldOrderGridModel, object?>>
            {
                { DisplayNameHelper.Get(typeof(HeldOrderGridModel), nameof(HeldOrderGridModel.OrderDate)), x => x.OrderDate },
                { DisplayNameHelper.Get(typeof(HeldOrderGridModel), nameof(HeldOrderGridModel.E1ShipTo)), x => x.E1ShipTo },
                { DisplayNameHelper.Get(typeof(HeldOrderGridModel), nameof(HeldOrderGridModel.CustomerName)), x => x.CustomerName },
                { DisplayNameHelper.Get(typeof(HeldOrderGridModel), nameof(HeldOrderGridModel.DocumentNo)), x => x.DocumentNo },
                { DisplayNameHelper.Get(typeof(HeldOrderGridModel), nameof(HeldOrderGridModel.NotificationStatus)), x => x.NotificationStatus },
                { DisplayNameHelper.Get(typeof(HeldOrderGridModel), nameof(HeldOrderGridModel.NotificationSendDate)), x => x.NotificationSendDate },
                { DisplayNameHelper.Get(typeof(HeldOrderGridModel), nameof(HeldOrderGridModel.Email)), x => x.Email },
            };
            var fileContent = await _excelService.ExportAsync(exportHeldOrders.Items, mappers, "Held orders");
            await _blazorDownloadFileService.DownloadFile("HeldOrders.xlsx", fileContent, "application/octet-stream");
            Snackbar.Add("Held orders exported successfully", Severity.Success);
        }
        catch (Exception)
        {
            Snackbar.Add("Failed to export held orders", Severity.Error);
        }
        finally
        {
            _loading = false;
        }
    }

    private async Task ClearNotificationStatusFilter()
    {
        var filter = _table.FilterDefinitions.Find(x => x.Column.Title == nameof(HeldOrderGridModel.NotificationStatus));
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
        _query.NotificationStatus = _notificationStatusFilterOptions.SelectedOptions
                .Select(pair => Enum.Parse<E1HeldNotificationStatusModel>(pair.Key))
                .ToHashSet();
        _notificationStatusFilterOptions.IsOpen = false;
        await _table.ReloadServerData();
    }
    #endregion
}
