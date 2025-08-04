using AutoMapper;
using BlazorDownloadFile;
using Fluxor;
using MediatR;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using MudBlazor;
using Tamro.Madam.Application.Commands.ItemMasterdata.Items.Bindings.Vlk;
using Tamro.Madam.Application.Queries.Items.Bindings.Vlk;
using Tamro.Madam.Application.Services.Files;
using Tamro.Madam.Common.Utils;
using Tamro.Madam.Models.Common.Dialog;
using Tamro.Madam.Models.ItemMasterdata.Items.Bindings.Vlk;
using Tamro.Madam.Repository.Entities.ItemMasterdata.Items.Bindings.Vlk;
using Tamro.Madam.Ui.ComponentExtensions;
using Tamro.Madam.Ui.Components.Audit;
using Tamro.Madam.Ui.Components.Dialogs;
using Tamro.Madam.Ui.Pages.ItemMasterdata.Items.Components;
using Tamro.Madam.Ui.Store.State;

namespace Tamro.Madam.Ui.Pages.ItemMasterdata.Items.Vlk;

public partial class VlkBindings
{
    //#region Properties
    private int _defaultPageSize = 15;
    private bool _loading;
    private HashSet<VlkBindingGridModel> _selectedVlkBindings = new();
    private VlkBindingQuery _query { get; set; } = new();
    private MudDataGrid<VlkBindingGridModel> _table = new();
    //#endregion

    //#region IoC
    [Inject]
    private IMediator _mediator { get; set; }
    [Inject]
    private IMapper _mapper { get; set; }
    [Inject]
    private IExcelService _excelService { get; set; }
    [Inject]
    private IBlazorDownloadFileService _blazorDownloadFileService { get; set; }
    [Inject]
    private IState<UserProfileState> _userProfileState { get; set; }
    //#endregion

    //#region Events
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
        await MudDataGridExtensions<VlkBindingGridModel>.ResetGridState(_table);
    }

    private async Task OnDeleteSingle(VlkBindingGridModel vlkBinding)
    {
        await OnDelete([vlkBinding.Id]);
    }

    private async Task OnDeleteChecked()
    {
        var idsToDelete = _selectedVlkBindings.Select(x => x.Id).ToArray();
        await OnDelete(idsToDelete);
    }

    private async Task OnDelete(int[] ids)
    {
        var cmd = new DeleteVlkBindingsCommand(ids);
        var parameters = new DialogParameters<ConfirmationDialog>
        {
            { x => x.Command, cmd },
            { x => x.Title, "Delete vlk bindings" },
            { x => x.Content, $"Are you sure you want to delete {ids.Count()} vlk bindings?" },
            { x => x.SuccessMessage, $"{ids.Count()} vlk bindings deleted" },
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
            _selectedVlkBindings.Clear();
        }
    }

    private async Task OnCreate()
    {
        await ShowDetailsDialog(new VlkBindingGridModel(), DialogState.Create);
    }

    private async Task OnOpen()
    {
        await ShowDetailsDialog(_selectedVlkBindings.First(), DialogState.View);
    }

    private async Task OnOpen(VlkBindingGridModel? model)
    {
        await ShowDetailsDialog(model, DialogState.View);
    }

    private async Task OnExport()
    {
        try
        {
            _loading = true;
            var query = new VlkBindingQuery()
            {
                Filters = _table.FilterDefinitions,
                OrderBy = _table.SortDefinitions.Values.FirstOrDefault()?.SortBy ?? nameof(VlkBindingGridModel.Id),
                SortDirection = _table.SortDefinitions.Values.FirstOrDefault()?.Descending ?? false ? SortDirection.Descending.ToString() : SortDirection.Ascending.ToString(),
                PageNumber = 1,
                SearchTerm = _query.SearchTerm,
                PageSize = int.MaxValue,
            };
            var exportableBrands = await _mediator.Send(query);

            var mappers = new Dictionary<string, Func<VlkBindingGridModel, object?>>
            {
                { DisplayNameHelper.Get(typeof(VlkBindingGridModel), nameof(VlkBindingGridModel.NpakId7)), b => b.NpakId7 },
                { DisplayNameHelper.Get(typeof(VlkBindingGridModel), nameof(VlkBindingGridModel.ItemNo2)), b => b.ItemNo2 },
                { DisplayNameHelper.Get(typeof(VlkBindingGridModel), nameof(VlkBindingGridModel.ItemName)), b => b.ItemName },
            };
            var fileContent = await _excelService.ExportAsync(exportableBrands.Items, mappers, "VlkBindings");
            await _blazorDownloadFileService.DownloadFile("VlkBindings.xlsx", fileContent, "application/octet-stream");
            Snackbar.Add("Vlk bindings exported successfully", Severity.Success);
        }
        catch (Exception)
        {
            Snackbar.Add("Failed to export vlk bindings", Severity.Error);
        }
        finally
        {
            _loading = false;
        }
    }

    private async Task ShowDetailsDialog(VlkBindingGridModel? model, DialogState dialogState)
    {
        var parameters = new DialogParameters<VlkBindingDetailsDialog>
        {
            { x => x.Model, _mapper.Map<VlkBindingDetailsModel>(model) },
            { x => x.State, dialogState },
        };
        var options = new DialogOptions
        {
            CloseButton = true,
            MaxWidth = MaxWidth.Small,
            CloseOnEscapeKey = true,
            FullWidth = true,
        };
        var dialog = DialogService.Show<VlkBindingDetailsDialog>(string.Empty, parameters, options);
        var state = await dialog.Result;

        await _table.ReloadServerData();
    }

    private async Task OnHistoryView()
    {
        await OnHistoryView(_selectedVlkBindings.First());
    }

    private async Task OnHistoryView(VlkBindingGridModel? vlkBinding)
    {
        await ShowHistoryDialog(vlkBinding.Id);
    }

    private async Task ShowHistoryDialog(int id)
    {
        var parameters = new DialogParameters<EntityHistory>
    {
        { x => x.EntityTypeName, nameof(VlkBinding) },
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

        await DialogService.ShowAsync<EntityHistory>($"Vlk binding {id} history", parameters, options);
    }

    private async Task<GridData<VlkBindingGridModel>> DataSource(GridState<VlkBindingGridModel> state)
    {
        try
        {
            _loading = true;
            _query.OrderBy = state.SortDefinitions.FirstOrDefault()?.SortBy ?? nameof(VlkBindingGridModel.NpakId7);
            _query.SortDirection = state.SortDefinitions.FirstOrDefault()?.Descending ?? true ? nameof(SortDirection.Ascending) : nameof(SortDirection.Descending);
            _query.PageNumber = state.Page + 1;
            _query.PageSize = state.PageSize;
            _query.Filters = state.FilterDefinitions;
            var result = await _mediator.Send(_query);

            return new GridData<VlkBindingGridModel>()
            {
                TotalItems = result.TotalItems,
                Items = result.Items,
            };
        }
        catch (Exception)
        {
            Snackbar.Add("Failed to load vlk bindings", Severity.Error);
            return new();
        }
        finally
        {
            _loading = false;
        }
    }
    //#endregion 
}
