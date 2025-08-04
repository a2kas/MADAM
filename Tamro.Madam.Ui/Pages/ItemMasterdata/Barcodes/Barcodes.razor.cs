using BlazorDownloadFile;
using Fluxor;
using MediatR;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using MudBlazor;
using Tamro.Madam.Application.Commands.ItemMasterdata.Barcodes;
using Tamro.Madam.Application.Commands.ItemMasterdata.Items;
using Tamro.Madam.Application.Queries.Barcodes;
using Tamro.Madam.Application.Services.Files;
using Tamro.Madam.Common.Utils;
using Tamro.Madam.Models.Common.Dialog;
using Tamro.Madam.Models.ItemMasterdata.Barcodes;
using Tamro.Madam.Repository.Entities.ItemMasterdata.Barcodes;
using Tamro.Madam.Ui.ComponentExtensions;
using Tamro.Madam.Ui.Components.Audit;
using Tamro.Madam.Ui.Components.Dialogs;
using Tamro.Madam.Ui.Pages.ItemMasterdata.Barcodes.Components;
using Tamro.Madam.Ui.Pages.ItemMasterdata.Items.Components;
using Tamro.Madam.Ui.Store.Actions.ItemMasterdata.Items;
using Tamro.Madam.Ui.Store.State;
using Tamro.Madam.Ui.Utils;

namespace Tamro.Madam.Ui.Pages.ItemMasterdata.Barcodes;

public partial class Barcodes
{
    #region Properties
    private int _defaultPageSize;
    private bool _loading;
    private HashSet<BarcodeGridModel> _selectedBarcodes = new();
    private BarcodeQuery _query { get; set; } = new();
    private MudDataGrid<BarcodeGridModel> _table = new();
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
    private IDispatcher _dispatcher { get; set; }
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
        await MudDataGridExtensions<BarcodeGridModel>.ResetGridState(_table);
    }

    private async Task OnOpenItem(BarcodeGridModel barcode)
    {
        if (barcode == null)
        {
            return;
        }

        _loading = true;

        var command = new GetItemCommand(barcode.ItemId);
        var itemResult = await _mediator.Send(command);

        _loading = false;

        if (itemResult.Succeeded)
        {
            _dispatcher.Dispatch(new SetCurrentItemAction()
            {
                Item = itemResult.Data,
                State = DialogState.View,
                UserProfileState = _userProfileState.Value,
            });
            var options = new DialogOptions
            {
                Position = DialogPosition.TopCenter,
                CloseButton = true,
                MaxWidth = MaxWidth.Large,
                CloseOnEscapeKey = true,
                FullWidth = true,
            };
            var dialog = DialogService.Show<ItemDetailsDialog>(string.Empty, new DialogParameters(), options);
            await dialog.Result;

            await _table.ReloadServerData();
        }
        else
        {
            Snackbar.Add(itemResult.Errors?[0], Severity.Error);
        }
    }

    private async Task OnDeleteSingle(BarcodeGridModel barcode)
    {
        await OnDelete(new[] { barcode.Id }, count: 1);
    }

    private async Task OnDeleteChecked()
    {
        var idsToDelete = _selectedBarcodes.Select(x => x.Id).ToArray();
        await OnDelete(idsToDelete, _selectedBarcodes.Count);
    }

    private async Task OnDelete(int[] ids, int count)
    {
        var command = new DeleteBarcodesCommand(ids);
        var parameters = new DialogParameters<ConfirmationDialog>
        {
            { x => x.Command, command },
            { x => x.Title, "Delete barcodes" },
            { x => x.Content, $"Are you sure you want to delete {count} barcodes?" },
            { x => x.SuccessMessage, $"{count} barcodes deleted" },
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
            _selectedBarcodes.Clear();
        }
    }

    private async Task OnCreate()
    {
        await ShowDetailsDialog(new BarcodeModel() { Measure = true, }, DialogState.Create);
    }

    private async Task OnCopy()
    {
        var barcode = _selectedBarcodes.First();
        await OnCopy(barcode);
    }

    private async Task OnCopy(BarcodeGridModel barcode)
    {
        _loading = true;

        var command = new GetBarcodeCommand(barcode.Id);
        var barcodeResult = await _mediator.Send(command);

        _loading = false;

        if (barcodeResult.Succeeded)
        {
            barcodeResult.Data.Id = default;
            await ShowDetailsDialog(barcodeResult.Data, DialogState.Copy);
        }
        else
        {
            Snackbar.Add(barcodeResult.Errors?[0], Severity.Error);
        }
    }

    private async Task OnOpen()
    {
        await OnOpen(_selectedBarcodes.First());
    }

    private async Task OnOpen(BarcodeGridModel? barcode)
    {
        if (barcode == null)
        {
            return;
        }

        _loading = true;

        var command = new GetBarcodeCommand(barcode.Id);
        var barcodeResult = await _mediator.Send(command);

        _loading = false;

        if (barcodeResult.Succeeded)
        {
            await ShowDetailsDialog(barcodeResult.Data, DialogState.View);
        }
        else
        {
            Snackbar.Add(barcodeResult.Errors?[0], Severity.Error);
        }
    }

    private async Task OnExport()
    {
        try
        {
            _loading = true;
            var query = new BarcodeQuery()
            {
                Filters = _table.FilterDefinitions,
                OrderBy = _table.SortDefinitions.Values.FirstOrDefault()?.SortBy ?? nameof(BarcodeGridModel.ItemName),
                SortDirection = _table.SortDefinitions.Values.FirstOrDefault()?.Descending ?? false ? SortDirection.Descending.ToString() : SortDirection.Ascending.ToString(),
                PageNumber = 1,
                PageSize = int.MaxValue,
                SearchTerm = _query.SearchTerm,
            };
            var exportableBarcodes = await _mediator.Send(query);

            var mappers = new Dictionary<string, Func<BarcodeGridModel, object?>>
            {
                { DisplayNameHelper.Get(typeof(BarcodeGridModel), nameof(BarcodeGridModel.Ean)), b => b.Ean },
                { DisplayNameHelper.Get(typeof(BarcodeGridModel), nameof(BarcodeGridModel.ItemId)), b => b.ItemId },
                { DisplayNameHelper.Get(typeof(BarcodeGridModel), nameof(BarcodeGridModel.ItemName)), b => b.ItemName },
                { DisplayNameHelper.Get(typeof(BarcodeGridModel), nameof(BarcodeGridModel.Measure)), b => b.Measure },
                { DisplayNameHelper.Get(typeof(BarcodeGridModel), nameof(BarcodeGridModel.RowVer)), b => b.RowVer },
            };
            var fileContent = await _excelService.ExportAsync(exportableBarcodes.Items, mappers, "barcodes");
            await _blazorDownloadFileService.DownloadFile("Barcodes.xlsx", fileContent, "application/octet-stream");
            Snackbar.Add("Barcodes exported successfully", Severity.Success);
        }
        catch (Exception)
        {
            Snackbar.Add("Failed to export barcodes", Severity.Error);
        }
        finally
        {
            _loading = false;
        }
    }

    private async Task ShowDetailsDialog(BarcodeModel? model, DialogState dialogState)
    {
        var parameters = new DialogParameters<BarcodeDetailsDialog>
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
        var dialog = DialogService.Show<BarcodeDetailsDialog>(string.Empty, parameters, options);
        var state = await dialog.Result;

        await _table.ReloadServerData();
    }

    private async Task OnHistoryView()
    {
        await OnHistoryView(_selectedBarcodes.First());
    }

    private async Task OnHistoryView(BarcodeGridModel? barcode)
    {
        await ShowHistoryDialog(barcode.Id);
    }

    private async Task ShowHistoryDialog(int id)
    {
        var parameters = new DialogParameters<EntityHistory>
        {
            { x => x.EntityTypeName, nameof(Barcode) },
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

        await DialogService.ShowAsync<EntityHistory>($"Barcode {id} history", parameters, options);
    }

    private async Task<GridData<BarcodeGridModel>> DataSource(GridState<BarcodeGridModel> state)
    {
        try
        {
            _loading = true;
            _query.OrderBy = state.SortDefinitions.FirstOrDefault()?.SortBy ?? nameof(BarcodeGridModel.ItemName);
            _query.SortDirection = state.SortDefinitions.FirstOrDefault()?.Descending ?? true ? nameof(SortDirection.Ascending) : nameof(SortDirection.Descending);
            _query.PageNumber = state.Page + 1;
            _query.PageSize = state.PageSize;
            _query.Filters = state.FilterDefinitions;

            var result = await _mediator.Send(_query);

            return new GridData<BarcodeGridModel>()
            {
                TotalItems = result.TotalItems,
                Items = result.Items,
            };
        }
        catch (Exception)
        {
            Snackbar.Add("Failed to load barcodes", Severity.Error);
            return new();
        }
        finally
        {
            _loading = false;
        }
    }
    #endregion 
}
