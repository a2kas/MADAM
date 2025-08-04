using BlazorDownloadFile;
using MediatR;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using MudBlazor;
using Tamro.Madam.Application.Commands.Finance.Peppol;
using Tamro.Madam.Application.Queries.Finance.Peppol;
using Tamro.Madam.Application.Services.Files;
using Tamro.Madam.Common.Utils;
using Tamro.Madam.Models.Finance.Peppol;
using Tamro.Madam.Ui.ComponentExtensions;
using Tamro.Madam.Ui.Components.DataGrid.Filtering;
using Tamro.Madam.Ui.Utils;

namespace Tamro.Madam.Ui.Pages.Finance.Peppol;

public partial class Peppol
{
    #region Properties
    private int _defaultPageSize;
    private bool _loading;
    private DictionaryFilterOptions _statusFilterOptions = new();
    private PeppolInvoiceQuery _query { get; set; } = new();
    private MudDataGrid<PeppolInvoiceGridModel> _table = new();
    private DictionaryFilterOptions _typeFilterOptions = new();
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
        _typeFilterOptions.Reset();
        await ClearStatusFilter();
        await MudDataGridExtensions<PeppolInvoiceGridModel>.ResetGridState(_table);
    }

    private async Task OnExport()
    {
        try
        {
            _loading = true;
            var query = new PeppolInvoiceQuery()
            {
                Filters = _table.FilterDefinitions,
                OrderBy = _table.SortDefinitions.Values.FirstOrDefault()?.SortBy ?? nameof(PeppolInvoiceGridModel.CreatedDate),
                SortDirection = _table.SortDefinitions.Values.FirstOrDefault()?.Descending ?? false ? SortDirection.Descending.ToString() : SortDirection.Ascending.ToString(),
                PageNumber = 1,
                PageSize = int.MaxValue,
                SearchTerm = _query.SearchTerm,
            };
            var exportableInvoices = await _mediator.Send(query);

            var mappers = new Dictionary<string, Func<PeppolInvoiceGridModel, object?>>
            {
                { DisplayNameHelper.Get(typeof(PeppolInvoiceGridModel), nameof(PeppolInvoiceGridModel.CustomerName)), x => x.CustomerName },
                { DisplayNameHelper.Get(typeof(PeppolInvoiceGridModel), nameof(PeppolInvoiceGridModel.SellerName)), x => x.SellerName },
                { DisplayNameHelper.Get(typeof(PeppolInvoiceGridModel), nameof(PeppolInvoiceGridModel.SellerRegNo)), x => x.SellerRegNo },
                { DisplayNameHelper.Get(typeof(PeppolInvoiceGridModel), nameof(PeppolInvoiceGridModel.InvoiceNumber)), x => x.InvoiceNumber },
                { DisplayNameHelper.Get(typeof(PeppolInvoiceGridModel), nameof(PeppolInvoiceGridModel.InvoiceDate)), x => x.InvoiceDate },
                { DisplayNameHelper.Get(typeof(PeppolInvoiceGridModel), nameof(PeppolInvoiceGridModel.InvoiceDueDate)), x => x.InvoiceDueDate },
                { DisplayNameHelper.Get(typeof(PeppolInvoiceGridModel), nameof(PeppolInvoiceGridModel.TotalInvoiceTaxAmount)), x => x.TotalInvoiceTaxAmount },
                { DisplayNameHelper.Get(typeof(PeppolInvoiceGridModel), nameof(PeppolInvoiceGridModel.TotalInvoiceAmountWithoutTax)), x => x.TotalInvoiceAmountWithoutTax },
                { DisplayNameHelper.Get(typeof(PeppolInvoiceGridModel), nameof(PeppolInvoiceGridModel.TotalInvoiceAmountWithTax)), x => x.TotalInvoiceAmountWithTax },
                { DisplayNameHelper.Get(typeof(PeppolInvoiceGridModel), nameof(PeppolInvoiceGridModel.Status)), x => x.Status },
                { DisplayNameHelper.Get(typeof(PeppolInvoiceGridModel), nameof(PeppolInvoiceGridModel.CreatedDate)), x => x.CreatedDate },
            };
            var fileContent = await _excelService.ExportAsync(exportableInvoices.Items, mappers, "Peppol invoices");
            await _blazorDownloadFileService.DownloadFile("Peppol_invoices.xlsx", fileContent, "application/octet-stream");
            Snackbar.Add("Invoices exported successfully", Severity.Success);
        }
        catch (Exception)
        {
            Snackbar.Add("Failed to export invoices", Severity.Error);
        }
        finally
        {
            _loading = false;
        }
    }

    private async Task<GridData<PeppolInvoiceGridModel>> DataSource(GridState<PeppolInvoiceGridModel> state)
    {
        try
        {
            _loading = true;
            MudDataGridExtensions<PeppolInvoiceGridModel>.ApplyDefaultFilterColumns(state.FilterDefinitions, _table.RenderedColumns);
            _query.OrderBy = state.SortDefinitions.FirstOrDefault()?.SortBy ?? nameof(PeppolInvoiceGridModel.CreatedDate);
            _query.SortDirection = state.SortDefinitions.FirstOrDefault()?.Descending ?? true ? nameof(SortDirection.Descending) : nameof(SortDirection.Ascending);
            _query.PageNumber = state.Page + 1;
            _query.PageSize = state.PageSize;
            _query.Filters = state.FilterDefinitions;

            var result = await _mediator.Send(_query);

            return new GridData<PeppolInvoiceGridModel>()
            {
                TotalItems = result.TotalItems,
                Items = result.Items,
            };
        }
        catch (Exception)
        {
            Snackbar.Add("Failed to load invoices", Severity.Error);
            return new();
        }
        finally
        {
            _loading = false;
        }
    }

    private async Task ApplyStatusFilter()
    {
        _query.Status = _statusFilterOptions.SelectedOptions
                .Select(pair => Enum.Parse<PeppolInvoiceStatus>(pair.Value))
                .ToHashSet();
        _statusFilterOptions.IsOpen = false;
        await _table.ReloadServerData();
    }

    private async Task ClearStatusFilter()
    {
        var filter = _table.FilterDefinitions.Find(x => x.Column.Title == nameof(PeppolInvoiceGridModel.Status));
        if (filter != null)
        {
            _table.FilterDefinitions.Remove(filter);
            await _table.ReloadServerData();
        }
        _statusFilterOptions.IsOpen = false;
        _statusFilterOptions.SelectAllChecked = false;
        _statusFilterOptions.SelectedOptions = [];
    }

    private async Task OnInvoiceDownload(string invoiceNumber)
    {
        try
        {
            var cmd = new DownloadPeppolInvoiceCommand(invoiceNumber);
            var result = await _mediator.Send(cmd);

            if (result.Succeeded)
            {
                string fileName = $"{invoiceNumber}.xml";
                await _blazorDownloadFileService.DownloadFile(fileName, result.Data, "application/octet-stream");
            }
            else
            {
                Snackbar.Add(result.ErrorMessage, Severity.Error);
            }
        }
        catch
        {
            Snackbar.Add("Failed to download invoice", Severity.Error);
        }
    }

    private static bool IsRegularInvoice(PeppolInvoiceGridModel item)
    {
        return item.Type == PeppolInvoiceType.Regular;
    }

    private static string ResolveInvoiceNumber(PeppolInvoiceGridModel item) 
    { 
        return IsRegularInvoice(item) ? item.InvoiceNumber : item.ConsolidationNumber; 
    }
    #endregion
}