using BlazorDownloadFile;
using Fluxor;
using MediatR;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using MudBlazor;
using Tamro.Madam.Application.Commands.Requestors;
using Tamro.Madam.Application.Queries.Requestors;
using Tamro.Madam.Application.Services.Files;
using Tamro.Madam.Common.Utils;
using Tamro.Madam.Models.Common.Dialog;
using Tamro.Madam.Models.ItemMasterdata.Requestors;
using Tamro.Madam.Repository.Entities.ItemMasterdata.Requestors;
using Tamro.Madam.Ui.ComponentExtensions;
using Tamro.Madam.Ui.Components.Audit;
using Tamro.Madam.Ui.Components.Dialogs;
using Tamro.Madam.Ui.Pages.ItemMasterdata.Requestors.Components;
using Tamro.Madam.Ui.Store.State;
using Tamro.Madam.Ui.Utils;

namespace Tamro.Madam.Ui.Pages.ItemMasterdata.Requestors;

public partial class Requestors
{
    #region Properties
    private int _defaultPageSize;
    private bool _loading;
    private HashSet<RequestorModel> _selectedRequestors = new();
    private RequestorQuery _query { get; set; } = new();
    private MudDataGrid<RequestorModel> _table = new();
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
        await MudDataGridExtensions<RequestorModel>.ResetGridState(_table);
    }

    private async Task OnDeleteSingle(RequestorModel requestor)
    {
        await OnDelete([requestor.Id], count: 1);
    }

    private async Task OnDeleteChecked()
    {
        var idsToDelete = _selectedRequestors.Select(x => x.Id).ToArray();
        await OnDelete(idsToDelete, _selectedRequestors.Count);
    }

    private async Task OnDelete(int[] ids, int count)
    {
        var command = new DeleteRequestorsCommand(ids);
        var parameters = new DialogParameters<ConfirmationDialog>
        {
            { x => x.Command, command },
            { x => x.Title, "Delete requestors" },
            { x => x.Content, $"Are you sure you want to delete {count} requestors?" },
            { x => x.SuccessMessage, $"{count} requestors deleted" },
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
            _selectedRequestors.Clear();
        }
    }

    private async Task OnCreate()
    {
        await ShowDetailsDialog(new RequestorModel(), DialogState.Create);
    }

    private async Task OnCopy()
    {
        var requestor = _selectedRequestors.First();
        requestor.Id = default;
        await ShowDetailsDialog(requestor, DialogState.Copy);
    }

    private async Task OnCopy(RequestorModel requestor)
    {
        requestor.Id = default;
        await ShowDetailsDialog(requestor, DialogState.Copy);
    }
    private async Task OnOpen()
    {
        await ShowDetailsDialog(_selectedRequestors.First(), DialogState.View);
    }

    private async Task OnOpen(RequestorModel? requestor)
    {
        await ShowDetailsDialog(requestor, DialogState.View);
    }

    private async Task OnExport()
    {
        try
        {
            _loading = true;
            var query = new RequestorQuery()
            {
                Filters = _table.FilterDefinitions,
                OrderBy = _table.SortDefinitions.Values.FirstOrDefault()?.SortBy ?? nameof(RequestorModel.Name),
                SortDirection = _table.SortDefinitions.Values.FirstOrDefault()?.Descending ?? false ? SortDirection.Descending.ToString() : SortDirection.Ascending.ToString(),
                PageNumber = 1,
                PageSize = int.MaxValue,
                SearchTerm = _query.SearchTerm,
            };
            var exportableRequestors = await _mediator.Send(query);

            var mappers = new Dictionary<string, Func<RequestorModel, object?>>
            {
                { DisplayNameHelper.Get(typeof(RequestorModel), nameof(RequestorModel.Name)), b => b.Name },
            };
            var fileContent = await _excelService.ExportAsync(exportableRequestors.Items, mappers, "Requestors");
            await _blazorDownloadFileService.DownloadFile("Requestors.xlsx", fileContent, "application/octet-stream");
            Snackbar.Add("Requestors exported successfully", Severity.Success);
        }
        catch (Exception)
        {
            Snackbar.Add("Failed to export requestors", Severity.Error);
        }
        finally
        {
            _loading = false;
        }
    }

    private async Task ShowDetailsDialog(RequestorModel? model, DialogState dialogState)
    {
        var parameters = new DialogParameters<RequestorDetailsDialog>
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
        var dialog = DialogService.Show<RequestorDetailsDialog>(string.Empty, parameters, options);
        var state = await dialog.Result;

        await _table.ReloadServerData();
    }

    private async Task OnHistoryView()
    {
        await OnHistoryView(_selectedRequestors.First());
    }

    private async Task OnHistoryView(RequestorModel? requestor)
    {
        await ShowHistoryDialog(requestor.Id);
    }

    private async Task ShowHistoryDialog(int id)
    {
        var parameters = new DialogParameters<EntityHistory>
        {
            { x => x.EntityTypeName, nameof(Requestor) },
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

        await DialogService.ShowAsync<EntityHistory>($"Requestor {id} history", parameters, options);
    }

    private async Task<GridData<RequestorModel>> DataSource(GridState<RequestorModel> state)
    {
        try
        {
            _loading = true;
            _query.OrderBy = state.SortDefinitions.FirstOrDefault()?.SortBy ?? nameof(RequestorModel.Name);
            _query.SortDirection = state.SortDefinitions.FirstOrDefault()?.Descending ?? true ? nameof(SortDirection.Ascending) : nameof(SortDirection.Descending);
            _query.PageNumber = state.Page + 1;
            _query.PageSize = state.PageSize;
            _query.Filters = state.FilterDefinitions;

            var result = await _mediator.Send(_query);

            return new GridData<RequestorModel>()
            {
                TotalItems = result.TotalItems,
                Items = result.Items,
            };
        }
        catch (Exception)
        {
            Snackbar.Add("Failed to load requestors", Severity.Error);
            return new();
        }
        finally
        {
            _loading = false;
        }
    }
    #endregion
}

