using BlazorDownloadFile;
using Fluxor;
using MediatR;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using MudBlazor;
using Tamro.Madam.Application.Commands.ItemMasterdata.MeasurementUnits;
using Tamro.Madam.Application.Queries.MeasurementUnits;
using Tamro.Madam.Application.Services.Files;
using Tamro.Madam.Common.Utils;
using Tamro.Madam.Models.Common.Dialog;
using Tamro.Madam.Models.ItemMasterdata.MeasurementUnits;
using Tamro.Madam.Repository.Entities.ItemMasterdata.MeasurementUnits;
using Tamro.Madam.Ui.ComponentExtensions;
using Tamro.Madam.Ui.Components.Audit;
using Tamro.Madam.Ui.Components.Dialogs;
using Tamro.Madam.Ui.Pages.ItemMasterdata.MeasurementUnits.Components;
using Tamro.Madam.Ui.Store.State;
using Tamro.Madam.Ui.Utils;

namespace Tamro.Madam.Ui.Pages.ItemMasterdata.MeasurementUnits;

public partial class MeasurementUnits
{
    #region Properties
    private int _defaultPageSize;
    private bool _loading;
    private HashSet<MeasurementUnitModel> _selectedMeasurementUnits = new();
    private MeasurementUnitQuery _query { get; set; } = new();
    private MudDataGrid<MeasurementUnitModel> _table = new();
    #endregion

    #region IoC
    [Inject]
    private UserSettingsUtils _userSettings { get; set; }
    [Inject]
    private IMediator _mediator { get; set; }
    [Inject]
    private IExcelService _excelService { get; set; }
    [Inject]
    private IState<UserProfileState> _userProfileState { get; set; }
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
        await MudDataGridExtensions<MeasurementUnitModel>.ResetGridState(_table);
    }

    private async Task OnDeleteSingle(MeasurementUnitModel measurementUnit)
    {
        await OnDelete(new[] { measurementUnit.Id }, count: 1);
    }

    private async Task OnDeleteChecked()
    {
        var idsToDelete = _selectedMeasurementUnits.Select(x => x.Id).ToArray();
        await OnDelete(idsToDelete, _selectedMeasurementUnits.Count);
    }

    private async Task OnDelete(int[] ids, int count)
    {
        var command = new DeleteMeasurementUnitsCommand(ids);
        var parameters = new DialogParameters<ConfirmationDialog>
        {
            { x => x.Command, command },
            { x => x.Title, "Delete measurement units" },
            { x => x.Content, $"Are you sure you want to delete {count} measurement units?" },
            { x => x.SuccessMessage, $"{count} measurement units deleted" },
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
            _selectedMeasurementUnits.Clear();
        }
    }

    private async Task OnCreate()
    {
        await ShowDetailsDialog(new MeasurementUnitModel(), DialogState.Create);
    }

    private async Task OnCopy()
    {
        var form = _selectedMeasurementUnits.First();
        form.Id = default;
        await ShowDetailsDialog(form, DialogState.Copy);
    }

    private async Task OnCopy(MeasurementUnitModel form)
    {
        form.Id = default;
        await ShowDetailsDialog(form, DialogState.Copy);
    }

    private async Task OnOpen()
    {
        await ShowDetailsDialog(_selectedMeasurementUnits.First(), DialogState.View);
    }

    private async Task OnOpen(MeasurementUnitModel? form)
    {
        await ShowDetailsDialog(form, DialogState.View);
    }

    private async Task OnExport()
    {
        try
        {
            _loading = true;
            var query = new MeasurementUnitQuery()
            {
                Filters = _table.FilterDefinitions,
                OrderBy = _table.SortDefinitions.Values.FirstOrDefault()?.SortBy ?? nameof(MeasurementUnitModel.Name),
                SortDirection = _table.SortDefinitions.Values.FirstOrDefault()?.Descending ?? false ? SortDirection.Descending.ToString() : SortDirection.Ascending.ToString(),
                PageNumber = 1,
                PageSize = int.MaxValue,
                SearchTerm = _query.SearchTerm,
            };
            var exportableMeasurementUnits = await _mediator.Send(query);

            var mappers = new Dictionary<string, Func<MeasurementUnitModel, object?>>
            {
                { DisplayNameHelper.Get(typeof(MeasurementUnitModel), nameof(MeasurementUnitModel.Name)), b => b.Name },
            };
            var fileContent = await _excelService.ExportAsync(exportableMeasurementUnits.Items, mappers, "MeasurementUnits");
            await _blazorDownloadFileService.DownloadFile("MeasurementUnits.xlsx", fileContent, "application/octet-stream");
            Snackbar.Add("Measurement units exported successfully", Severity.Success);
        }
        catch (Exception)
        {
            Snackbar.Add("Failed to export measurement units", Severity.Error);
        }
        finally
        {
            _loading = false;
        }
    }

    private async Task ShowDetailsDialog(MeasurementUnitModel? model, DialogState dialogState)
    {
        var parameters = new DialogParameters<MeasurementUnitDetailsDialog>
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
        var dialog = DialogService.Show<MeasurementUnitDetailsDialog>(string.Empty, parameters, options);
        var state = await dialog.Result;

        await _table.ReloadServerData();
    }

    private async Task OnHistoryView()
    {
        await OnHistoryView(_selectedMeasurementUnits.First());
    }

    private async Task OnHistoryView(MeasurementUnitModel? measurementUnit)
    {
        await ShowHistoryDialog(measurementUnit.Id);
    }

    private async Task ShowHistoryDialog(int id)
    {
        var parameters = new DialogParameters<EntityHistory>
    {
        { x => x.EntityTypeName, nameof(MeasurementUnit) },
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

        await DialogService.ShowAsync<EntityHistory>($"Measurement unit {id} history", parameters, options);
    }

    private async Task<GridData<MeasurementUnitModel>> DataSource(GridState<MeasurementUnitModel> state)
    {
        try
        {
            _loading = true;
            _query.OrderBy = state.SortDefinitions.FirstOrDefault()?.SortBy ?? nameof(MeasurementUnitModel.Name);
            _query.SortDirection = state.SortDefinitions.FirstOrDefault()?.Descending ?? true ? nameof(SortDirection.Ascending) : nameof(SortDirection.Descending);
            _query.PageNumber = state.Page + 1;
            _query.PageSize = state.PageSize;
            _query.Filters = state.FilterDefinitions;

            var result = await _mediator.Send(_query);

            return new GridData<MeasurementUnitModel>()
            {
                TotalItems = result.TotalItems,
                Items = result.Items,
            };
        }
        catch (Exception)
        {
            Snackbar.Add("Failed to load measurement units", Severity.Error);
            return new();
        }
        finally
        {
            _loading = false;
        }
    }
    #endregion 
}
