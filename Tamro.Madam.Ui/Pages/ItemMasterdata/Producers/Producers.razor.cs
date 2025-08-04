using BlazorDownloadFile;
using Fluxor;
using MediatR;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using MudBlazor;
using Tamro.Madam.Application.Commands.ItemMasterdata.Producers;
using Tamro.Madam.Application.Queries.Producers;
using Tamro.Madam.Application.Services.Files;
using Tamro.Madam.Common.Utils;
using Tamro.Madam.Models.Common.Dialog;
using Tamro.Madam.Models.ItemMasterdata.Producers;
using Tamro.Madam.Repository.Entities.ItemMasterdata.Producers;
using Tamro.Madam.Ui.ComponentExtensions;
using Tamro.Madam.Ui.Components.Audit;
using Tamro.Madam.Ui.Components.Dialogs;
using Tamro.Madam.Ui.Pages.ItemMasterdata.Producers.Components;
using Tamro.Madam.Ui.Store.State;
using Tamro.Madam.Ui.Utils;

namespace Tamro.Madam.Ui.Pages.ItemMasterdata.Producers;

public partial class Producers
{
    #region Properties
    private int _defaultPageSize;
    private bool _loading;
    private HashSet<ProducerModel> _selectedProducers = new();
    private ProducerQuery _query { get; set; } = new();
    private MudDataGrid<ProducerModel> _table = new();
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
        await MudDataGridExtensions<ProducerModel>.ResetGridState(_table);
    }

    private async Task OnDeleteSingle(ProducerModel producer)
    {
        await OnDelete(new[] { producer.Id }, count: 1);
    }

    private async Task OnDeleteChecked()
    {
        var idsToDelete = _selectedProducers.Select(x => x.Id).ToArray();
        await OnDelete(idsToDelete, _selectedProducers.Count);
    }

    private async Task OnDelete(int[] ids, int count)
    {
        var command = new DeleteProducersCommand(ids);
        var parameters = new DialogParameters<ConfirmationDialog>
        {
            { x => x.Command, command },
            { x => x.Title, "Delete producers" },
            { x => x.Content, $"Are you sure you want to delete {count} producers?" },
            { x => x.SuccessMessage, $"{count} producers deleted" },
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
            _selectedProducers.Clear();
        }
    }

    private async Task OnCreate()
    {
        await ShowDetailsDialog(new ProducerModel(), DialogState.Create);
    }

    private async Task OnCopy()
    {
        var producer = _selectedProducers.First();
        producer.Id = default;
        await ShowDetailsDialog(producer, DialogState.Copy);
    }

    private async Task OnCopy(ProducerModel producer)
    {
        producer.Id = default;
        await ShowDetailsDialog(producer, DialogState.Copy);
    }

    private async Task OnOpen()
    {
        await ShowDetailsDialog(_selectedProducers.First(), DialogState.View);
    }

    private async Task OnOpen(ProducerModel? producer)
    {
        await ShowDetailsDialog(producer, DialogState.View);
    }

    private async Task OnExport()
    {
        try
        {
            _loading = true;
            var query = new ProducerQuery()
            {
                Filters = _table.FilterDefinitions,
                OrderBy = _table.SortDefinitions.Values.FirstOrDefault()?.SortBy ?? nameof(ProducerModel.Name),
                SortDirection = _table.SortDefinitions.Values.FirstOrDefault()?.Descending ?? false ? SortDirection.Descending.ToString() : SortDirection.Ascending.ToString(),
                PageNumber = 1,
                PageSize = int.MaxValue,
                SearchTerm = _query.SearchTerm,
            };
            var exportableProducers = await _mediator.Send(query);

            var mappers = new Dictionary<string, Func<ProducerModel, object?>>
            {
                { DisplayNameHelper.Get(typeof(ProducerModel), nameof(ProducerModel.Name)), b => b.Name },
            };
            var fileContent = await _excelService.ExportAsync(exportableProducers.Items, mappers, "Producers");
            await _blazorDownloadFileService.DownloadFile("Producers.xlsx", fileContent, "application/octet-stream");
            Snackbar.Add("Producers exported successfully", Severity.Success);
        }
        catch (Exception)
        {
            Snackbar.Add("Failed to export producers", Severity.Error);
        }
        finally
        {
            _loading = false;
        }
    }

    private async Task ShowDetailsDialog(ProducerModel? model, DialogState dialogState)
    {
        var parameters = new DialogParameters<ProducerDetailsDialog>
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
        var dialog = DialogService.Show<ProducerDetailsDialog>(string.Empty, parameters, options);
        var state = await dialog.Result;

        await _table.ReloadServerData();
    }

    private async Task OnHistoryView()
    {
        await OnHistoryView(_selectedProducers.First());
    }

    private async Task OnHistoryView(ProducerModel? producer)
    {
        await ShowHistoryDialog(producer.Id);
    }

    private async Task ShowHistoryDialog(int id)
    {
        var parameters = new DialogParameters<EntityHistory>
    {
        { x => x.EntityTypeName, nameof(Producer) },
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

        await DialogService.ShowAsync<EntityHistory>($"Producer {id} history", parameters, options);
    }

    private async Task<GridData<ProducerModel>> DataSource(GridState<ProducerModel> state)
    {
        try
        {
            _loading = true;
            _query.OrderBy = state.SortDefinitions.FirstOrDefault()?.SortBy ?? nameof(ProducerModel.Name);
            _query.SortDirection = state.SortDefinitions.FirstOrDefault()?.Descending ?? true ? nameof(SortDirection.Ascending) : nameof(SortDirection.Descending);
            _query.PageNumber = state.Page + 1;
            _query.PageSize = state.PageSize;
            _query.Filters = state.FilterDefinitions;

            var result = await _mediator.Send(_query);

            return new GridData<ProducerModel>()
            {
                TotalItems = result.TotalItems,
                Items = result.Items,
            };
        }
        catch (Exception)
        {
            Snackbar.Add("Failed to load producers", Severity.Error);
            return new();
        }
        finally
        {
            _loading = false;
        }
    }
    #endregion 
}
