using AutoMapper;
using BlazorDownloadFile;
using Fluxor;
using MediatR;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using MudBlazor;
using Tamro.Madam.Application.Commands.Sales.Sabis;
using Tamro.Madam.Application.Queries.Sales.Sabis;
using Tamro.Madam.Application.Services.Files;
using Tamro.Madam.Common.Utils;
using Tamro.Madam.Models.Common.Dialog;
using Tamro.Madam.Models.Sales.Sabis;
using Tamro.Madam.Ui.ComponentExtensions;
using Tamro.Madam.Ui.Components.Dialogs;
using Tamro.Madam.Ui.Pages.Sales.Sabis.Components;
using Tamro.Madam.Ui.Store.State;
using Tamro.Madam.Ui.Utils;

namespace Tamro.Madam.Ui.Pages.Sales.Sabis;

public partial class Sabis
{
    #region Properties
    private int _defaultPageSize;
    private bool _loading;
    private HashSet<SksContractGridModel> _selectedSksContracts = new();
    private SksContractQuery _query { get; set; } = new();
    private MudDataGrid<SksContractGridModel> _table = new();
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
    [Inject]
    private IMapper _mapper { get; set; }
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
        await MudDataGridExtensions<SksContractGridModel>.ResetGridState(_table);
    }

    private async Task OnDeleteSingle(SksContractGridModel sksContract)
    {
        await OnDelete(new[] { sksContract.Id }, count: 1);
    }

    private async Task OnDeleteChecked()
    {
        var idsToDelete = _selectedSksContracts.Select(x => x.Id).ToArray();
        await OnDelete(idsToDelete, _selectedSksContracts.Count);
    }

    private async Task OnDelete(int[] ids, int count)
    {
        var command = new DeleteSksContractsCommand(ids);
        var parameters = new DialogParameters<ConfirmationDialog>
        {
            { x => x.Command, command },
            { x => x.Title, "Delete contract mappings" },
            { x => x.Content, $"Are you sure you want to delete {count} contract mappings?" },
            { x => x.SuccessMessage, $"{count} contract mappings deleted" },
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
            _selectedSksContracts.Clear();
        }
    }

    private async Task OnCreate()
    {
        await ShowDetailsDialog(new SksContractModel(), DialogState.Create);
    }

    private async Task OnCopy()
    {
        var sksContract = _selectedSksContracts.First();
        await OnCopy(sksContract);
    }

    private async Task OnCopy(SksContractGridModel model)
    {
        var sksContract = _mapper.Map<SksContractModel>(model);
        sksContract.Id = default;
        await ShowDetailsDialog(sksContract, DialogState.Copy);
    }

    private async Task OnOpen()
    {
        await OnOpen(_selectedSksContracts.First());
    }

    private async Task OnOpen(SksContractGridModel? model)
    {
        var sksContract = _mapper.Map<SksContractModel>(model);
        await ShowDetailsDialog(sksContract, DialogState.View);
    }

    private async Task OnExport()
    {
        try
        {
            _loading = true;
            var query = new SksContractQuery()
            {
                Filters = _table.FilterDefinitions,
                OrderBy = _table.SortDefinitions.Values.FirstOrDefault()?.SortBy ?? nameof(SksContractGridModel.Id),
                SortDirection = _table.SortDefinitions.Values.FirstOrDefault()?.Descending ?? false ? SortDirection.Descending.ToString() : SortDirection.Ascending.ToString(),
                PageNumber = 1,
                PageSize = int.MaxValue,
                SearchTerm = _query.SearchTerm,
            };
            var exportableSksContractMappings = await _mediator.Send(query);

            var mappers = new Dictionary<string, Func<SksContractGridModel, object?>>
            {
                { DisplayNameHelper.Get(typeof(SksContractGridModel), nameof(SksContractGridModel.CustomerName)), b => b.CustomerName },
                { DisplayNameHelper.Get(typeof(SksContractGridModel), nameof(SksContractGridModel.CompanyId)), b => b.CompanyId },
                { DisplayNameHelper.Get(typeof(SksContractGridModel), nameof(SksContractGridModel.AddressNumber)), b => b.AddressNumber },
                { DisplayNameHelper.Get(typeof(SksContractGridModel), nameof(SksContractGridModel.ContractTamro)), b => b.ContractTamro },
                { DisplayNameHelper.Get(typeof(SksContractGridModel), nameof(SksContractGridModel.ContractSabis)), b => b.ContractSabis },
                { DisplayNameHelper.Get(typeof(SksContractGridModel), nameof(SksContractGridModel.EditedAt)), b => b.EditedAt },
                { DisplayNameHelper.Get(typeof(SksContractGridModel), nameof(SksContractGridModel.EditedBy)), b => b.EditedBy },
            };
            var fileContent = await _excelService.ExportAsync(exportableSksContractMappings.Items, mappers, "Sks contract mappings");
            await _blazorDownloadFileService.DownloadFile("SksContractMappings.xlsx", fileContent, "application/octet-stream");
            Snackbar.Add("SKS contract mappings exported successfully", Severity.Success);
        }
        catch (Exception)
        {
            Snackbar.Add("Failed to export SKS contract mappings", Severity.Error);
        }
        finally
        {
            _loading = false;
        }
    }

    private async Task ShowDetailsDialog(SksContractModel? model, DialogState dialogState)
    {
        var parameters = new DialogParameters<SabisDetailsDialog>
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
        var dialog = DialogService.Show<SabisDetailsDialog>(string.Empty, parameters, options);
        var state = await dialog.Result;

        await _table.ReloadServerData();
    }

    private async Task<GridData<SksContractGridModel>> DataSource(GridState<SksContractGridModel> state)
    {
        try
        {
            _loading = true;
            _query.OrderBy = state.SortDefinitions.FirstOrDefault()?.SortBy ?? nameof(SksContractGridModel.Id);
            _query.SortDirection = state.SortDefinitions.FirstOrDefault()?.Descending ?? true ? nameof(SortDirection.Ascending) : nameof(SortDirection.Descending);
            _query.PageNumber = state.Page + 1;
            _query.PageSize = state.PageSize;
            _query.Filters = state.FilterDefinitions;

            var result = await _mediator.Send(_query);

            return new GridData<SksContractGridModel>()
            {
                TotalItems = result.TotalItems,
                Items = result.Items,
            };
        }
        catch (Exception)
        {
            Snackbar.Add("Failed to load SKS contract mappings", Severity.Error);
            return new();
        }
        finally
        {
            _loading = false;
        }
    }
    #endregion 
}
