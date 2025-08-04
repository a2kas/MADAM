using AutoMapper;
using BlazorDownloadFile;
using Fluxor;
using MediatR;
using Microsoft.AspNetCore.Components;
using MudBlazor;
using Tamro.Madam.Application.Commands.Sales.CanceledOrderLines.ExcludedCustomers;
using Tamro.Madam.Application.Queries.Sales.CanceledOrderLines;
using Tamro.Madam.Application.Services.Files;
using Tamro.Madam.Common.Utils;
using Tamro.Madam.Models.Sales.CanceledOrderLines.ExcludedCustomers;
using Tamro.Madam.Ui.ComponentExtensions;
using Tamro.Madam.Ui.Components.Dialogs;
using Tamro.Madam.Ui.Pages.Sales.CanceledOrderLines.Components;
using Tamro.Madam.Ui.Store.State;
using Tamro.Madam.Ui.Utils;

namespace Tamro.Madam.Ui.Pages.Sales.CanceledOrderLines;

public partial class ExcludedCustomers
{
    #region Properties
    private int _defaultPageSize;
    private bool _loading;
    private HashSet<ExcludedCustomerGridModel> _selectedExcludedCustomers = new();
    private ExcludedCustomersQuery _query { get; set; } = new();
    private MudDataGrid<ExcludedCustomerGridModel> _table = new();
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
    }

    private async Task OnReset()
    {
        _query = new();
        await MudDataGridExtensions<ExcludedCustomerGridModel>.ResetGridState(_table);
    }

    private async Task OnExport()
    {
        try
        {
            _loading = true;
            var query = new ExcludedCustomersQuery()
            {
                Filters = _table.FilterDefinitions,
                Country = _userProfileState.Value.UserProfile.Country,
                OrderBy = _table.SortDefinitions.Values.FirstOrDefault()?.SortBy ?? nameof(ExcludedCustomerGridModel.E1SoldTo),
                SortDirection = _table.SortDefinitions.Values.FirstOrDefault()?.Descending ?? false ? SortDirection.Descending.ToString() : SortDirection.Ascending.ToString(),
                PageNumber = 1,
                PageSize = int.MaxValue,
            };
            var exportableExcludedCustomers = await _mediator.Send(query);

            var mappers = new Dictionary<string, Func<ExcludedCustomerGridModel, object?>>
            {
                { DisplayNameHelper.Get(typeof(ExcludedCustomerGridModel), nameof(ExcludedCustomerGridModel.E1SoldTo)), b => b.E1SoldTo},
                { DisplayNameHelper.Get(typeof(ExcludedCustomerGridModel), nameof(ExcludedCustomerGridModel.Name)), b => b.Name },
             };
            var fileContent = await _excelService.ExportAsync(exportableExcludedCustomers.Items, mappers, "ExcludedCustomers");
            await _blazorDownloadFileService.DownloadFile("ExcludedCustomers.xlsx", fileContent, "application/octet-stream");
            Snackbar.Add("Excluded customers exported successfully", Severity.Success);
        }
        catch (Exception)
        {
            Snackbar.Add("Failed to export excluded customers", Severity.Error);
        }
        finally
        {
            _loading = false;
        }
    }

    private async Task OnCreate()
    {
        var options = new DialogOptions
        {
            CloseButton = true,
            MaxWidth = MaxWidth.Small,
            CloseOnEscapeKey = true,
            FullWidth = true,
        };
        var dialog = DialogService.Show<ExcludedCustomersDetailsDialog>(string.Empty, options);
        var state = await dialog.Result;

        await _table.ReloadServerData();
    }

    private async Task OnDeleteSingle(ExcludedCustomerGridModel excludedCustomer)
    {
        await OnDelete(new[] { excludedCustomer.Id }, count: 1);
    }

    private async Task OnDeleteChecked()
    {
        var idsToDelete = _selectedExcludedCustomers.Select(x => x.Id).ToArray();
        await OnDelete(idsToDelete, _selectedExcludedCustomers.Count);
    }

    private async Task OnDelete(int[] ids, int count)
    {
        var command = new RemoveCustomerFromExcludedCanceledOrderLineNotificationsCommand(ids);
        var parameters = new DialogParameters<ConfirmationDialog>
        {
            { x => x.Command, command },
            { x => x.Title, "Delete excluded customers" },
            { x => x.Content, $"Are you sure you want to delete {count} excluded customers?" },
            { x => x.SuccessMessage, $"{count} excluded customers deleted" },
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
            _selectedExcludedCustomers.Clear();
        }
    }

    private async Task<GridData<ExcludedCustomerGridModel>> DataSource(GridState<ExcludedCustomerGridModel> state)
    {
        try
        {
            _loading = true;
            MudDataGridExtensions<ExcludedCustomerGridModel>.ApplyDefaultFilterColumns(state.FilterDefinitions, _table.RenderedColumns);
            _query.OrderBy = state.SortDefinitions.FirstOrDefault()?.SortBy ?? nameof(ExcludedCustomerGridModel.E1SoldTo);
            _query.SortDirection = state.SortDefinitions.FirstOrDefault()?.Descending ?? true ? nameof(SortDirection.Ascending) : nameof(SortDirection.Descending);
            _query.PageNumber = state.Page + 1;
            _query.PageSize = state.PageSize;
            _query.Filters = state.FilterDefinitions;
            _query.Country = _userProfileState.Value.UserProfile.Country;

            var result = await _mediator.Send(_query);

            return new GridData<ExcludedCustomerGridModel>()
            {
                TotalItems = result.TotalItems,
                Items = result.Items,
            };
        }
        catch (Exception)
        {
            Snackbar.Add("Failed to load excluded customers", Severity.Error);
            return new();
        }
        finally
        {
            _loading = false;
        }
    }
    #endregion
}
