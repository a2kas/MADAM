using MediatR;
using Microsoft.AspNetCore.Components;
using MudBlazor;
using Org.BouncyCastle.Crypto;
using Tamro.Madam.Application.Commands.Hr.Jira.Administration;
using Tamro.Madam.Application.Queries.Hr.Jira.Administration.Grid;
using Tamro.Madam.Application.Utilities;
using Tamro.Madam.Models.Data;
using Tamro.Madam.Models.Hr.Jira.Administration;
using Tamro.Madam.Repository.Entities.Jpg;
using Tamro.Madam.Ui.ComponentExtensions;
using Tamro.Madam.Ui.Components.DataGrid.Filtering;
using Tamro.Madam.Ui.Components.Dialogs;
using Tamro.Madam.Ui.Utils;

namespace Tamro.Madam.Ui.Pages.Hr.Jira.Administration;

public partial class JiraAdministration
{
    #region Properties
    protected int _defaultPageSize;
    protected bool _loading;
    protected HashSet<JiraAccountModel> _selectedJiraAccounts = new();
    protected JiraAccountsGridQuery _query { get; set; } = new();
    protected MudDataGrid<JiraAccountModel> _table = new();
    protected DictionaryFilterOptions _teamsFilterOptions = new();
    protected DictionaryFilterOptions _isActiveFilterOptions = SetDefaultIsActiveFilterOptions();
    #endregion

    #region IoC
    [Inject]
    protected UserSettingsUtils _userSettings { get; set; }
    [Inject]
    protected IMediator _mediator { get; set; }
    [Inject]
    protected ISnackbar _snackbar { get; set; }

    #endregion

    #region Events
    protected override async Task OnInitializedAsync()
    {
        _defaultPageSize = _userSettings.GetRowsPerPageSetting();
    }

    protected async Task OnReset()
    {
        _query = new();
        _teamsFilterOptions.Reset();
        _isActiveFilterOptions = SetDefaultIsActiveFilterOptions();
        await MudDataGridExtensions<JiraAccountModel>.ResetGridState(_table);
    }

    protected async Task<GridData<JiraAccountModel>> DataSource(GridState<JiraAccountModel> state)
    {
        try
        {
            _loading = true;
            _query.OrderBy = state.SortDefinitions.FirstOrDefault()?.SortBy ?? nameof(JiraAccount.DisplayName);
            _query.SortDirection = state.SortDefinitions.FirstOrDefault()?.Descending ?? true ? nameof(SortDirection.Ascending) : nameof(SortDirection.Descending);
            _query.PageNumber = state.Page + 1;
            _query.PageSize = state.PageSize;
            _query.Filters = state.FilterDefinitions;

            var result = await _mediator.Send(_query);

            return new GridData<JiraAccountModel>()
            {
                TotalItems = result.TotalItems,
                Items = result.Items,
            };
        }
        catch (Exception)
        {
            _snackbar.Add("Failed to load jira accounts", Severity.Error);
            return new();
        }
        finally
        {
            _loading = false;
        }
    }

    private async Task OnSetIsActivated(bool isActivated)
    {
        var idsToToggle = _selectedJiraAccounts.Select(x => x.Id).ToArray();
        var count = _selectedJiraAccounts.Count;
        await SetIsActivated(idsToToggle, count, isActivated);
    }

    private async Task SetIsActivated(string[] ids, int count, bool isActivated)
    {
        var command = new SetAccountsIsActivatedCommand(ids, isActivated);

        var prefix = isActivated ? "" : "De";
        var icon = isActivated ? Icons.Material.Outlined.ToggleOn : Icons.Material.Outlined.ToggleOff;

        var parameters = new DialogParameters<ConfirmationDialog>
        {
            { x => x.Command, command },
            { x => x.Title, $"{prefix}activate users" },
            { x => x.Content, $"Are you sure you want to {prefix.ToLower()}activate {count} user(s)?" },
            { x => x.SuccessMessage, $"{count} user(s) {prefix.ToLower()}activated" },
            { x => x.SubmitIcon, icon },
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

        var dialog = DialogService.Show<ConfirmationDialog>($"{prefix}activate", parameters, options);
        var state = await dialog.Result;

        if (!state.Canceled)
        {
            await _table.ReloadServerData();
            _selectedJiraAccounts.Clear();
        }
    }

    private static DictionaryFilterOptions SetDefaultIsActiveFilterOptions()
    {
        return new DictionaryFilterOptions
        {
            SelectedOptions =
            [
                GenericData.BooleanDisplayNames.First(x => x.Key == "Yes")
            ]
        };
    }

    private async Task OnTeamChanged(JiraAccountModel row, string? team)
    {
        row.Team = EnumParser.ParseNullable<JiraAccountTeam>(team);
        await CommitRowChanges(row);
    }

    public async Task CommitRowChanges(JiraAccountModel row)
    {
        try
        {
            _loading = true;
            var result = await _mediator.Send(new UpdateAccountsCommand(row));

            if (result.Succeeded)
            {
                _snackbar.Add("Saved", Severity.Success);
                await _table.ReloadServerData();
            }
            else
            {
                _snackbar.Add(result.ErrorMessage, Severity.Error);
            }
        }
        catch (Exception)
        {
            _snackbar.Add("Failed to save changes", Severity.Error);
        }

        _loading = false;
    }
    #endregion
}
