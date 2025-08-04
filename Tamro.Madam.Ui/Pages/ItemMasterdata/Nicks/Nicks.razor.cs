using BlazorDownloadFile;
using Fluxor;
using MediatR;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using MudBlazor;
using Tamro.Madam.Application.Commands.ItemMasterdata.Nicks;
using Tamro.Madam.Application.Queries.Nicks;
using Tamro.Madam.Application.Services.Files;
using Tamro.Madam.Common.Utils;
using Tamro.Madam.Models.Common.Dialog;
using Tamro.Madam.Models.ItemMasterdata.Nicks;
using Tamro.Madam.Repository.Entities.ItemMasterdata.Nicks;
using Tamro.Madam.Ui.ComponentExtensions;
using Tamro.Madam.Ui.Components.Audit;
using Tamro.Madam.Ui.Components.Dialogs;
using Tamro.Madam.Ui.Pages.ItemMasterdata.Nicks.Components;
using Tamro.Madam.Ui.Store.State;
using Tamro.Madam.Ui.Utils;

namespace Tamro.Madam.Ui.Pages.ItemMasterdata.Nicks;

public partial class Nicks
{
    #region Properties
    private int _defaultPageSize;
    private bool _loading;
    private HashSet<NickModel> _selectedNicks = new();
    private NickQuery _query { get; set; } = new();
    private MudDataGrid<NickModel> _table = new();
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
        await MudDataGridExtensions<NickModel>.ResetGridState(_table);
    }

    private async Task OnDeleteSingle(NickModel nick)
    {
        await OnDelete(new[] { nick.Id }, count: 1);
    }

    private async Task OnDeleteChecked()
    {
        var idsToDelete = _selectedNicks.Select(x => x.Id).ToArray();
        await OnDelete(idsToDelete, _selectedNicks.Count);
    }

    private async Task OnDelete(int[] ids, int count)
    {
        var command = new DeleteNicksCommand(ids);
        var parameters = new DialogParameters<ConfirmationDialog>
        {
            { x => x.Command, command },
            { x => x.Title, "Delete baltic nicks" },
            { x => x.Content, $"Are you sure you want to delete {count} baltic nicks?" },
            { x => x.SuccessMessage, $"{count} baltic nicks deleted" },
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
            _selectedNicks.Clear();
        }
    }

    private async Task OnCreate()
    {
        await ShowDetailsDialog(new NickModel(), DialogState.Create);
    }

    private async Task OnCopy()
    {
        var nick = _selectedNicks.First();
        nick.Id = default;
        await ShowDetailsDialog(nick, DialogState.Copy);
    }

    private async Task OnCopy(NickModel nick)
    {
        nick.Id = default;
        await ShowDetailsDialog(nick, DialogState.Copy);
    }

    private async Task OnOpen()
    {
        await ShowDetailsDialog(_selectedNicks.First(), DialogState.View);
    }

    private async Task OnOpen(NickModel? nick)
    {
        await ShowDetailsDialog(nick, DialogState.View);
    }

    private async Task OnExport()
    {
        try
        {
            _loading = true;
            var query = new NickQuery()
            {
                Filters = _table.FilterDefinitions,
                OrderBy = _table.SortDefinitions.Values.FirstOrDefault()?.SortBy ?? nameof(NickModel.Name),
                SortDirection = _table.SortDefinitions.Values.FirstOrDefault()?.Descending ?? false ? SortDirection.Descending.ToString() : SortDirection.Ascending.ToString(),
                PageNumber = 1,
                PageSize = int.MaxValue,
                SearchTerm = _query.SearchTerm,
            };
            var exportableNicks = await _mediator.Send(query);

            var mappers = new Dictionary<string, Func<NickModel, object?>>
            {
                { DisplayNameHelper.Get(typeof(NickModel), nameof(NickModel.Name)), b => b.Name },
            };
            var fileContent = await _excelService.ExportAsync(exportableNicks.Items, mappers, "Baltic nicks");
            await _blazorDownloadFileService.DownloadFile("BalticNicks.xlsx", fileContent, "application/octet-stream");
            Snackbar.Add("Baltic nicks exported successfully", Severity.Success);
        }
        catch (Exception)
        {
            Snackbar.Add("Failed to export baltic nicks", Severity.Error);
        }
        finally
        {
            _loading = false;
        }
    }

    private async Task ShowDetailsDialog(NickModel? model, DialogState dialogState)
    {
        var parameters = new DialogParameters<NickDetailsDialog>
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
        var dialog = DialogService.Show<NickDetailsDialog>(string.Empty, parameters, options);
        var state = await dialog.Result;

        await _table.ReloadServerData();
    }

    private async Task OnHistoryView()
    {
        await OnHistoryView(_selectedNicks.First());
    }

    private async Task OnHistoryView(NickModel? nick)
    {
        await ShowHistoryDialog(nick.Id);
    }

    private async Task ShowHistoryDialog(int id)
    {
        var parameters = new DialogParameters<EntityHistory>
    {
        { x => x.EntityTypeName, nameof(Nick) },
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

        await DialogService.ShowAsync<EntityHistory>($"Baltic nick {id} history", parameters, options);
    }

    private async Task<GridData<NickModel>> DataSource(GridState<NickModel> state)
    {
        try
        {
            _loading = true;
            _query.OrderBy = state.SortDefinitions.FirstOrDefault()?.SortBy ?? nameof(NickModel.Name);
            _query.SortDirection = state.SortDefinitions.FirstOrDefault()?.Descending ?? true ? nameof(SortDirection.Ascending) : nameof(SortDirection.Descending);
            _query.PageNumber = state.Page + 1;
            _query.PageSize = state.PageSize;
            _query.Filters = state.FilterDefinitions;

            var result = await _mediator.Send(_query);

            return new GridData<NickModel>()
            {
                TotalItems = result.TotalItems,
                Items = result.Items,
            };
        }
        catch (Exception)
        {
            Snackbar.Add("Failed to load baltic nicks", Severity.Error);
            return new();
        }
        finally
        {
            _loading = false;
        }
    }
    #endregion 
}
