using BlazorDownloadFile;
using Fluxor;
using MediatR;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using MudBlazor;
using Tamro.Madam.Application.Queries.Brands;
using Tamro.Madam.Application.Services.Files;
using Tamro.Madam.Common.Utils;
using Tamro.Madam.Models.Common.Dialog;
using Tamro.Madam.Models.ItemMasterdata.Brands;
using Tamro.Madam.Repository.Entities.Brands;
using Tamro.Madam.Ui.ComponentExtensions;
using Tamro.Madam.Ui.Components.Audit;
using Tamro.Madam.Ui.Pages.ItemMasterdata.Brands.Components;
using Tamro.Madam.Ui.Store.Actions.ItemMasterdata.Brands;
using Tamro.Madam.Ui.Store.State;
using Tamro.Madam.Ui.Utils;

namespace Tamro.Madam.Ui.Pages.ItemMasterdata.Brands;

public partial class Brands
{
    #region Properties
    private int _defaultPageSize;
    private bool _loading;
    private HashSet<BrandModel> _selectedBrands = new();
    private BrandQuery _query { get; set; } = new();
    private MudDataGrid<BrandModel> _table = new();
    #endregion

    #region IoC
    [Inject]
    private UserSettingsUtils _userSettings { get; set; }
    [Inject]
    private IMediator _mediator { get; set; }
    [Inject]
    private IDispatcher _dispatcher { get; set; }
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
        await MudDataGridExtensions<BrandModel>.ResetGridState(_table);
    }

    private async Task OnDeleteSingle(BrandModel brand)
    {
        await OnDelete([brand]);
    }

    private async Task OnDeleteChecked()
    {
        await OnDelete(_selectedBrands);
    }

    private async Task OnDelete(HashSet<BrandModel> brands)
    {
        _dispatcher.Dispatch(new SetBrandsToDeleteAction()
        {
            Brands = brands,
        });
        var options = new DialogOptions
        {
            CloseButton = true,
            MaxWidth = MaxWidth.Medium,
            FullWidth = true,
            CloseOnEscapeKey = true,
        };
        var dialog = DialogService.Show<BrandDeleteDialog>(string.Empty, new DialogParameters(), options);
        await dialog.Result;
        await _table.ReloadServerData();
        _selectedBrands.Clear();
    }

    private async Task OnCreate()
    {
        await ShowDetailsDialog(new BrandModel(), DialogState.Create);
    }

    private async Task OnCopy()
    {
        var brand = _selectedBrands.First();
        brand.Id = default;
        await ShowDetailsDialog(brand, DialogState.Copy);
    }

    private async Task OnCopy(BrandModel brand)
    {
        brand.Id = default;
        await ShowDetailsDialog(brand, DialogState.Copy);
    }

    private async Task OnOpen()
    {
        await ShowDetailsDialog(_selectedBrands.First(), DialogState.View);
    }

    private async Task OnOpen(BrandModel? brand)
    {
        await ShowDetailsDialog(brand, DialogState.View);
    }

    private async Task OnExport()
    {
        try
        {
            _loading = true;
            var query = new BrandQuery()
            {
                Filters = _table.FilterDefinitions,
                OrderBy = _table.SortDefinitions.Values.FirstOrDefault()?.SortBy ?? nameof(BrandModel.Name),
                SortDirection = _table.SortDefinitions.Values.FirstOrDefault()?.Descending ?? false ? SortDirection.Descending.ToString() : SortDirection.Ascending.ToString(),
                PageNumber = 1,
                SearchTerm = _query.SearchTerm,
                PageSize = int.MaxValue,
            };
            var exportableBrands = await _mediator.Send(query);

            var mappers = new Dictionary<string, Func<BrandModel, object?>>
            {
                { DisplayNameHelper.Get(typeof(BrandModel), nameof(BrandModel.Name)), b => b.Name },
            };
            var fileContent = await _excelService.ExportAsync(exportableBrands.Items, mappers, "Brands");
            await _blazorDownloadFileService.DownloadFile("Brands.xlsx", fileContent, "application/octet-stream");
            Snackbar.Add("Brands exported successfully", Severity.Success);
        }
        catch (Exception)
        {
            Snackbar.Add("Failed to export brands", Severity.Error);
        }
        finally
        {
            _loading = false;
        }
    }

    private async Task ShowDetailsDialog(BrandModel? model, DialogState dialogState)
    {
        var parameters = new DialogParameters<BrandDetailsDialog>
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
        var dialog = DialogService.Show<BrandDetailsDialog>(string.Empty, parameters, options);
        var state = await dialog.Result;

        await _table.ReloadServerData();
    }

    private async Task OnHistoryView()
    {
        await OnHistoryView(_selectedBrands.First());
    }

    private async Task OnHistoryView(BrandModel? brand)
    {
        await ShowHistoryDialog(brand.Id);
    }

    private async Task ShowHistoryDialog(int id)
    {
        var parameters = new DialogParameters<EntityHistory>
    {
        { x => x.EntityTypeName, nameof(Brand) },
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

        await DialogService.ShowAsync<EntityHistory>($"Brand {id} history", parameters, options);
    }

    private async Task<GridData<BrandModel>> DataSource(GridState<BrandModel> state)
    {
        try
        {
            _loading = true;
            _query.OrderBy = state.SortDefinitions.FirstOrDefault()?.SortBy ?? nameof(BrandModel.Name);
            _query.SortDirection = state.SortDefinitions.FirstOrDefault()?.Descending ?? true ? nameof(SortDirection.Ascending) : nameof(SortDirection.Descending);
            _query.PageNumber = state.Page + 1;
            _query.PageSize = state.PageSize;
            _query.Filters = state.FilterDefinitions;

            var result = await _mediator.Send(_query);

            return new GridData<BrandModel>()
            {
                TotalItems = result.TotalItems,
                Items = result.Items,
            };
        }
        catch (Exception)
        {
            Snackbar.Add("Failed to load brands", Severity.Error);
            return new();
        }
        finally
        {
            _loading = false;
        }
    }
    #endregion 
}
