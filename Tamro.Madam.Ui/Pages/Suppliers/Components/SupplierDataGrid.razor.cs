using BlazorDownloadFile;
using Fluxor;
using MediatR;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using MudBlazor;
using Tamro.Madam.Application.Commands.Suppliers;
using Tamro.Madam.Application.Queries.Suppliers;
using Tamro.Madam.Application.Services.Files;
using Tamro.Madam.Common.Utils;
using Tamro.Madam.Models.Suppliers;
using Tamro.Madam.Repository.Entities.Suppliers;
using Tamro.Madam.Ui.ComponentExtensions;
using Tamro.Madam.Ui.Components.Audit;
using Tamro.Madam.Ui.Store.Actions.Suppliers;
using Tamro.Madam.Ui.Store.State;
using Tamro.Madam.Ui.Utils;

namespace Tamro.Madam.Ui.Pages.Suppliers.Components;

public partial class SupplierDataGrid
{
    #region Properties
    private int _defaultPageSize;
    private bool _loading;
    private HashSet<SupplierGridModel> _selectedSuppliers = new();
    private SupplierQuery _query { get; set; } = new();
    private MudDataGrid<SupplierGridModel> _table = new();
    #endregion

    #region IoC
    [Inject]
    private UserSettingsUtils _userSettings { get; set; }
    [Inject]
    private IMediator _mediator { get; set; }
    [Inject]
    private IExcelService _excelService { get; set; }
    [Inject]
    private IDispatcher _dispatcher { get; set; }
    [Inject]
    private IActionSubscriber _actionSubscriber { get; set; }
    [Inject]
    private IBlazorDownloadFileService _blazorDownloadFileService { get; set; }
    [Inject]
    private IState<UserProfileState> _userProfileState { get; set; }
    #endregion

    #region Events
    protected override async Task OnInitializedAsync()
    {
        _defaultPageSize = _userSettings.GetRowsPerPageSetting();
        _actionSubscriber?.SubscribeToAction<RefreshGridAction>(this, _ => OnReset());
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
        await MudDataGridExtensions<SupplierGridModel>.ResetGridState(_table);
    }

    private async Task OnExport()
    {
        try
        {
            _loading = true;
            var query = new SupplierQuery()
            {
                Filters = _table.FilterDefinitions,
                OrderBy = _table.SortDefinitions.Values.FirstOrDefault()?.SortBy ?? nameof(SupplierGridModel.CreatedDate),
                SortDirection = _table.SortDefinitions.Values.FirstOrDefault()?.Descending ?? false ? SortDirection.Descending.ToString() : SortDirection.Ascending.ToString(),
                PageNumber = 1,
                PageSize = int.MaxValue,
                SearchTerm = _query.SearchTerm,
                Country = _userProfileState.Value.UserProfile.Country,
            };
            var exportableSuppliers = await _mediator.Send(query);

            var mappers = new Dictionary<string, Func<SupplierGridModel, object?>>
            {
                { DisplayNameHelper.Get(typeof(SupplierGridModel), nameof(SupplierGridModel.Name)), b => b.Name },
                { DisplayNameHelper.Get(typeof(SupplierGridModel), nameof(SupplierGridModel.RegistrationNumber)), b => b.RegistrationNumber },
                { DisplayNameHelper.Get(typeof(SupplierGridModel), nameof(SupplierGridModel.CreatedDate)), b => b.CreatedDate },
            };
            var fileContent = await _excelService.ExportAsync(exportableSuppliers.Items, mappers, "Suppliers");
            await _blazorDownloadFileService.DownloadFile("Suppliers.xlsx", fileContent, "application/octet-stream");
            Snackbar.Add("Suppliers exported successfully", Severity.Success);
        }
        catch (Exception)
        {
            Snackbar.Add("Failed to export suppliers", Severity.Error);
        }
        finally
        {
            _loading = false;
        }
    }

    private async Task OnCreate()
    {
        _dispatcher.Dispatch(new SetActiveFormAction(SupplierForm.Details));
    }

    private async Task OnOpen()
    {
        await OnOpen(_selectedSuppliers.First());
    }

    private async Task OnOpen(SupplierGridModel supplier)
    {
        if (supplier == null)
        {
            return;
        }

        _loading = true;

        var command = new GetSupplierCommand(supplier.Id);
        var result = await _mediator.Send(command);

        _loading = false;

        if (result.Succeeded)
        {
            _dispatcher.Dispatch(new SetActiveFormAction(SupplierForm.Details, result.Data));
        }
        else
        {
            Snackbar.Add(result.ErrorMessage, Severity.Error);
        }
    }

    private async Task OnHistoryView()
    {
        await OnHistoryView(_selectedSuppliers.First());
    }

    private async Task OnHistoryView(SupplierGridModel? supplier)
    {
        await ShowHistoryDialog(supplier.Id);
    }

    private async Task ShowHistoryDialog(int id)
    {
        var parameters = new DialogParameters<EntityHistory>
        {
            { x => x.EntityTypeName, nameof(Supplier) },
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

        await DialogService.ShowAsync<EntityHistory>($"Supplier {id} history", parameters, options);
    }

    private async Task<GridData<SupplierGridModel>> DataSource(GridState<SupplierGridModel> state)
    {
        try
        {
            _loading = true;
            _query.OrderBy = state.SortDefinitions.FirstOrDefault()?.SortBy ?? nameof(SupplierGridModel.CreatedDate);
            _query.Country = _userProfileState.Value.UserProfile.Country;
            _query.SortDirection = state.SortDefinitions.FirstOrDefault()?.Descending ?? true ? nameof(SortDirection.Ascending) : nameof(SortDirection.Descending);
            _query.PageNumber = state.Page + 1;
            _query.PageSize = state.PageSize;
            _query.Filters = state.FilterDefinitions;

            var result = await _mediator.Send(_query);

            return new GridData<SupplierGridModel>()
            {
                TotalItems = result.TotalItems,
                Items = result.Items,
            };
        }
        catch (Exception)
        {
            Snackbar.Add("Failed to load suppliers", Severity.Error);
            return new();
        }
        finally
        {
            _loading = false;
        }
    }
    #endregion 
}
