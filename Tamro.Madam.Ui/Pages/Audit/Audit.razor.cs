using MediatR;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using MudBlazor;
using Tamro.Madam.Application.Commands.Audit;
using Tamro.Madam.Application.Queries.Audit;
using Tamro.Madam.Common.Constants.SearchExpressionConstants;
using Tamro.Madam.Models.Audit;
using Tamro.Madam.Ui.ComponentExtensions;
using Tamro.Madam.Ui.Components.DataGrid.Filtering;
using Tamro.Madam.Ui.Pages.Audit.Components;
using Tamro.Madam.Ui.Utils;

namespace Tamro.Madam.Ui.Pages.Audit;

public partial class Audit
{
    private bool _loading;
    private int _defaultPageSize;
    private DictionaryFilterOptions _entityTypeFilterOptions = new();
    private DictionaryFilterOptions _stateNameFilterOptions = new();
    private AuditQuery _query { get; set; } = new();
    private MudDataGrid<AuditGridModel> _table = new();

    [Inject]
    private UserSettingsUtils _userSettings { get; set; }
    [Inject]
    private IMediator _mediator { get; set; }

    protected override async Task OnInitializedAsync()
    {
        _defaultPageSize = _userSettings.GetRowsPerPageSetting();
    }

    private async Task OnReset()
    {
        _query = new();
        await MudDataGridExtensions<AuditGridModel>.ResetGridState(_table);
        await ClearEntityTypeFilter();
        await ClearStateNameFilter();
    }

    private async Task OnBasicSearchKeyDown(KeyboardEventArgs e)
    {
        if (e.Key == "Enter")
        {
            await _table.ReloadServerData();
        }
    }

    private async Task OnOpen(AuditGridModel model)
    {
        await ShowDetailsDialog(model.Id);
    }

    private async Task<GridData<AuditGridModel>> DataSource(GridState<AuditGridModel> state)
    {
        _loading = true;
        MudDataGridExtensions<AuditGridModel>.ApplyDefaultFilterColumns(state.FilterDefinitions, _table.RenderedColumns);
        GridData<AuditGridModel> result = new();
        _query.OrderBy = state.SortDefinitions.FirstOrDefault()?.SortBy ?? nameof(AuditGridModel.CreatedDate);
        _query.SortDirection = state.SortDefinitions.FirstOrDefault()?.Descending ?? false ? nameof(SortDirection.Ascending) : nameof(SortDirection.Descending);
        _query.PageNumber = state.Page + 1;
        _query.PageSize = state.PageSize;
        _query.Filters = state.FilterDefinitions;

        var queryResult = await _mediator.Send(_query);

        if (queryResult.Succeeded)
        {
            result.TotalItems = queryResult.Data.TotalItems;
            result.Items = queryResult.Data.Items;
        }
        else
        {
            Snackbar.Add("Failed to load audit", Severity.Error);
        }
        _loading = false;
        return result;
    }

    private async Task ShowDetailsDialog(int id)
    {
        var cmd = new GetAuditByIdCommand(id);
        var result = await _mediator.Send(cmd);

        if (!result.Succeeded)
        {
            Snackbar.Add(result.ErrorMessage, Severity.Error);
        }

        var parameters = new DialogParameters<AuditDetailsDialog>
        {
            { x => x.Model, result.Data },
        };

        var options = new DialogOptions
        {
            Position = DialogPosition.Center,
            CloseButton = true,
            MaxWidth = MaxWidth.Large,
            CloseOnEscapeKey = true,
            FullWidth = true,
        };

        await DialogService.ShowAsync<AuditDetailsDialog>(string.Empty, parameters, options);
    }

    private async Task ClearEntityTypeFilter()
    {
        var filter = _table.FilterDefinitions.Find(x => x.Column.Title == nameof(AuditGridModel.EntityTypeName));
        if (filter != null)
        {
            _table.FilterDefinitions.Remove(filter);
            await _table.ReloadServerData();
        }
        _entityTypeFilterOptions.IsOpen = false;
        _entityTypeFilterOptions.SelectAllChecked = false;
        _entityTypeFilterOptions.SelectedOptions = [];
    }

    private async Task ApplyEntityTypeFilter()
    {
        var filter = _table.FilterDefinitions.Find(x => x.Title == nameof(AuditGridModel.EntityTypeName));
        if (filter != null)
        {
            _table.FilterDefinitions.Remove(filter);
        }
        _entityTypeFilterOptions.IsOpen = false;
        await _table.AddFilterAsync(new FilterDefinition<AuditGridModel>()
        {
            Title = nameof(AuditGridModel.EntityTypeName),
            Operator = SearchStringConstants.EqualsOneOf,
            Value = _entityTypeFilterOptions.SelectedOptions,
        });
    }

    private async Task ClearStateNameFilter()
    {
        var filter = _table.FilterDefinitions.Find(x => x.Column.Title == nameof(AuditGridModel.StateName));
        if (filter != null)
        {
            _table.FilterDefinitions.Remove(filter);
            await _table.ReloadServerData();
        }
        _stateNameFilterOptions.IsOpen = false;
        _stateNameFilterOptions.SelectAllChecked = false;
        _stateNameFilterOptions.SelectedOptions = [];
    }

    private async Task ApplyStateNameFilter()
    {
        var filter = _table.FilterDefinitions.Find(x => x.Title == nameof(AuditGridModel.StateName));
        if (filter != null)
        {
            _table.FilterDefinitions.Remove(filter);
        }
        _stateNameFilterOptions.IsOpen = false;
        await _table.AddFilterAsync(new FilterDefinition<AuditGridModel>()
        {
            Title = nameof(AuditGridModel.StateName),
            Operator = SearchStringConstants.EqualsOneOf,
            Value = _stateNameFilterOptions.SelectedOptions,
        });
    }
}
