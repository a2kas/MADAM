using MediatR;
using Microsoft.AspNetCore.Components;
using MudBlazor;
using Tamro.Madam.Application.Commands.Audit;
using Tamro.Madam.Application.Queries.Audit;
using Tamro.Madam.Models.Audit;
using Tamro.Madam.Ui.Utils;

namespace Tamro.Madam.Ui.Components.Audit;

public partial class EntityHistory
{
    [EditorRequired]
    [Parameter]
    public string EntityTypeName { get; set; }
    [EditorRequired]
    [Parameter]
    public string EntityId { get; set; }
    public bool AllExpanded { get; set; } = false;

    private AuditQuery _query { get; set; } = new();
    private int _defaultPageSize;
    private bool _loading;
    private MudDataGrid<AuditGridModel> _table = new();
    private List<int> itemIds { get; set; }
    private List<string?> ItemPropertyNames { get; set; } = [];

    [Parameter]
    public List<AuditEntryDetailsGrid> AuditEntryDetails { get; set; } = new List<AuditEntryDetailsGrid>();

    [CascadingParameter]
    private IMudDialogInstance _dialog { get; set; } = default!;

    [Inject]
    private UserSettingsUtils _userSettings { get; set; }
    [Inject]
    private IMediator _mediator { get; set; }
    public AuditPropertyModel? EntityHistoryGlobalFilter { get; set; } = new AuditPropertyModel()
    {
        OldValue = string.Empty,
        NewValue = string.Empty,
        PropertyName = string.Empty,
    };

    protected override async Task OnInitializedAsync()
    {
        _defaultPageSize = _userSettings.GetRowsPerPageSetting();
    }

    private void OnSearchTermChange()
    {
        if (!string.IsNullOrEmpty(EntityHistoryGlobalFilter.NewValue) || !string.IsNullOrEmpty(EntityHistoryGlobalFilter.OldValue) || !string.IsNullOrEmpty(EntityHistoryGlobalFilter.PropertyName))
        {
            OnChangeAllStateClick(true);
        }
        else if (string.IsNullOrEmpty(EntityHistoryGlobalFilter.NewValue) && string.IsNullOrEmpty(EntityHistoryGlobalFilter.OldValue) && string.IsNullOrEmpty(EntityHistoryGlobalFilter.PropertyName))
        {
            OnChangeAllStateClick(false);
        }
    }

    private void OnSelectPropertyName(string value)
    {
        EntityHistoryGlobalFilter.PropertyName = value;
        EntityHistoryGlobalFilter.OldValue = string.Empty;
        EntityHistoryGlobalFilter.NewValue = string.Empty;

        OnSearchTermChange();
    }

    private async Task FetchAuditEntry(int id)
    {
        var cmd = new GetAuditByIdCommand(id);
        var result = await _mediator.Send(cmd);

        if (!result.Succeeded)
        {
            Snackbar.Add(result.ErrorMessage, Severity.Error);
        }
        else
        {
            var data = result.Data.Properties.Where(x => x.PropertyName != "RowVer" && x.PropertyName != "EditedBy" && x.OldValue != x.NewValue).ToList();

            bool entryExists = AuditEntryDetails.Any(entry => entry.Id == id);

            if (!entryExists)
            {
                var newEntry = new AuditEntryDetailsGrid()
                {
                    Items = data,
                    StateName = result.Data.StateName,
                    Id = id,
                    IsExpanded = false,
                };

                AuditEntryDetails.Add(newEntry);

                var distinctPropertyNames = data.Select(x => x.PropertyName).Distinct();

                foreach (var propertyName in distinctPropertyNames)
                {
                    if (!ItemPropertyNames.Contains(propertyName))
                    {
                        ItemPropertyNames.Add(propertyName);
                    }
                }
            }
        }
    }

    private void OnChangeAllStateClick(bool expanded)
    {
        AllExpanded = expanded;

        foreach (var auditEntryDetail in AuditEntryDetails)
        {
            auditEntryDetail.IsExpanded = AllExpanded;
        }
    }

    private void OnChangeStateClick(int id)
    {
        var existingEntry = AuditEntryDetails.FirstOrDefault(d => d.Id == id);

        if (existingEntry != null)
        {
            existingEntry.IsExpanded = !existingEntry.IsExpanded;
        }

        if (AuditEntryDetails.All(d => d.IsExpanded == true))
        {
            AllExpanded = true;
        }
        else
        {
            AllExpanded = false;
        }
    }

    private void Close() => _dialog.Close();

    private async Task<GridData<AuditGridModel>> DataSource(GridState<AuditGridModel> state)
    {
        _loading = true;
        GridData<AuditGridModel> result = new();
        _query.OrderBy = state.SortDefinitions.FirstOrDefault()?.SortBy ?? nameof(AuditGridModel.CreatedDate);
        _query.SortDirection = state.SortDefinitions.FirstOrDefault()?.Descending ?? false ? nameof(SortDirection.Ascending) : nameof(SortDirection.Descending);
        _query.PageNumber = state.Page + 1;
        _query.PageSize = state.PageSize;
        _query.EntityId = EntityId;
        _query.EntityTypeName = EntityTypeName;
        _query.Filters = state.FilterDefinitions;
        var queryResult = await _mediator.Send(_query);
        if (queryResult.Succeeded)
        {
            result.TotalItems = queryResult.Data.TotalItems;
            result.Items = queryResult.Data.Items;

            itemIds = queryResult.Data.Items.Select(x => x.Id).ToList();
            if (itemIds != null)
            {
                foreach (var id in itemIds)
                {
                    await FetchAuditEntry(id);
                }
            }
        }
        else
        {
            Snackbar.Add(queryResult.ErrorMessage, Severity.Error);
        }
        _loading = false;
        return result;
    }
}