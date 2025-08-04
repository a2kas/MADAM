using BlazorDownloadFile;
using Fluxor;
using MediatR;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using MudBlazor;
using Tamro.Madam.Application.Commands.ItemMasterdata.Forms;
using Tamro.Madam.Application.Queries.Forms;
using Tamro.Madam.Application.Services.Files;
using Tamro.Madam.Common.Utils;
using Tamro.Madam.Models.Common.Dialog;
using Tamro.Madam.Models.ItemMasterdata.Forms;
using Tamro.Madam.Repository.Entities.ItemMasterdata.Forms;
using Tamro.Madam.Ui.ComponentExtensions;
using Tamro.Madam.Ui.Components.Audit;
using Tamro.Madam.Ui.Components.Dialogs;
using Tamro.Madam.Ui.Pages.ItemMasterdata.Forms.Components;
using Tamro.Madam.Ui.Store.State;
using Tamro.Madam.Ui.Utils;

namespace Tamro.Madam.Ui.Pages.ItemMasterdata.Forms;

public partial class Forms
{
    #region Properties
    private int _defaultPageSize;
    private bool _loading;
    private HashSet<FormModel> _selectedForms = new();
    private FormQuery _query { get; set; } = new();
    private MudDataGrid<FormModel> _table = new();
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
        await MudDataGridExtensions<FormModel>.ResetGridState(_table);
    }

    private async Task OnDeleteSingle(FormModel form)
    {
        await OnDelete(new[] { form.Id }, count: 1);
    }

    private async Task OnDeleteChecked()
    {
        var idsToDelete = _selectedForms.Select(x => x.Id).ToArray();
        await OnDelete(idsToDelete, _selectedForms.Count);
    }

    private async Task OnDelete(int[] ids, int count)
    {
        var command = new DeleteFormsCommand(ids);
        var parameters = new DialogParameters<ConfirmationDialog>
        {
            { x => x.Command, command },
            { x => x.Title, "Delete forms" },
            { x => x.Content, $"Are you sure you want to delete {count} forms?" },
            { x => x.SuccessMessage, $"{count} forms deleted" },
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
            _selectedForms.Clear();
        }
    }

    private async Task OnCreate()
    {
        await ShowDetailsDialog(new FormModel(), DialogState.Create);
    }

    private async Task OnCopy()
    {
        var form = _selectedForms.First();
        form.Id = default;
        await ShowDetailsDialog(form, DialogState.Copy);
    }

    private async Task OnCopy(FormModel form)
    {
        form.Id = default;
        await ShowDetailsDialog(form, DialogState.Copy);
    }

    private async Task OnOpen()
    {
        await ShowDetailsDialog(_selectedForms.First(), DialogState.View);
    }

    private async Task OnOpen(FormModel? form)
    {
        await ShowDetailsDialog(form, DialogState.View);
    }

    private async Task OnExport()
    {
        try
        {
            _loading = true;
            var query = new FormQuery()
            {
                Filters = _table.FilterDefinitions,
                OrderBy = _table.SortDefinitions.Values.FirstOrDefault()?.SortBy ?? nameof(FormModel.Name),
                SortDirection = _table.SortDefinitions.Values.FirstOrDefault()?.Descending ?? false ? SortDirection.Descending.ToString() : SortDirection.Ascending.ToString(),
                PageNumber = 1,
                PageSize = int.MaxValue,
                SearchTerm = _query.SearchTerm,
            };
            var exportableForms = await _mediator.Send(query);

            var mappers = new Dictionary<string, Func<FormModel, object?>>
            {
                { DisplayNameHelper.Get(typeof(FormModel), nameof(FormModel.Name)), b => b.Name },
            };
            var fileContent = await _excelService.ExportAsync(exportableForms.Items, mappers, "Forms");
            await _blazorDownloadFileService.DownloadFile("Forms.xlsx", fileContent, "application/octet-stream");
            Snackbar.Add("Forms exported successfully", Severity.Success);
        }
        catch (Exception)
        {
            Snackbar.Add("Failed to export forms", Severity.Error);
        }
        finally
        {
            _loading = false;
        }
    }

    private async Task ShowDetailsDialog(FormModel? model, DialogState dialogState)
    {
        var parameters = new DialogParameters<FormDetailsDialog>
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
        var dialog = DialogService.Show<FormDetailsDialog>(string.Empty, parameters, options);
        var state = await dialog.Result;

        await _table.ReloadServerData();
    }

    private async Task OnHistoryView()
    {
        await OnHistoryView(_selectedForms.First());
    }

    private async Task OnHistoryView(FormModel? form)
    {
        await ShowHistoryDialog(form.Id);
    }

    private async Task ShowHistoryDialog(int id)
    {
        var parameters = new DialogParameters<EntityHistory>
    {
        { x => x.EntityTypeName, nameof(Form) },
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

        await DialogService.ShowAsync<EntityHistory>($"Form {id} history", parameters, options);
    }

    private async Task<GridData<FormModel>> DataSource(GridState<FormModel> state)
    {
        try
        {
            _loading = true;
            _query.OrderBy = state.SortDefinitions.FirstOrDefault()?.SortBy ?? nameof(FormModel.Name);
            _query.SortDirection = state.SortDefinitions.FirstOrDefault()?.Descending ?? true ? nameof(SortDirection.Ascending) : nameof(SortDirection.Descending);
            _query.PageNumber = state.Page + 1;
            _query.PageSize = state.PageSize;
            _query.Filters = state.FilterDefinitions;

            var result = await _mediator.Send(_query);

            return new GridData<FormModel>()
            {
                TotalItems = result.TotalItems,
                Items = result.Items,
            };
        }
        catch (Exception)
        {
            Snackbar.Add("Failed to load forms", Severity.Error);
            return new();
        }
        finally
        {
            _loading = false;
        }
    }
    #endregion 
}
