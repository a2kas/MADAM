using Fluxor;
using MediatR;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using MudBlazor;
using Tamro.Madam.Application.Queries.Items.Bindings.Retail;
using Tamro.Madam.Models.ItemMasterdata.Items.Bindings.Retail;
using Tamro.Madam.Ui.ComponentExtensions;
using Tamro.Madam.Ui.Pages.Tools.Components;
using Tamro.Madam.Ui.Store.State;
using Tamro.Madam.Ui.Utils;

namespace Tamro.Madam.Ui.Pages.Tools;

public partial class RetailCodeGenerator
{
    #region Properties
    private int _defaultPageSize;
    private bool _loading;
    private GeneratedRetailCodeQuery _query { get; set; } = new();
    private MudDataGrid<GeneratedRetailCodeModel> _table = new();
    #endregion

    #region IoC
    [Inject]
    private UserSettingsUtils _userSettings { get; set; }
    [Inject]
    private IMediator _mediator { get; set; }
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
        await MudDataGridExtensions<GeneratedRetailCodeModel>.ResetGridState(_table);
    }

    private async Task OnGenerate()
    {
        var options = new DialogOptions
        {
            Position = DialogPosition.TopCenter,
            CloseButton = true,
            MaxWidth = MaxWidth.Medium,
            CloseOnEscapeKey = true,
            FullWidth = true,
        };
        var dialog = DialogService.Show<RetailCodeGeneratorDialog>(string.Empty, options);
        var state = await dialog.Result;

        await _table.ReloadServerData();
    }

    private async Task<GridData<GeneratedRetailCodeModel>> DataSource(GridState<GeneratedRetailCodeModel> state)
    {
        try
        {
            _loading = true;
            _query.OrderBy = state.SortDefinitions.FirstOrDefault()?.SortBy ?? nameof(GeneratedRetailCodeModel.Code);
            _query.SortDirection = state.SortDefinitions.FirstOrDefault()?.Descending ?? false ? nameof(SortDirection.Ascending) : nameof(SortDirection.Descending);
            _query.PageNumber = state.Page + 1;
            _query.PageSize = state.PageSize;
            _query.Filters = state.FilterDefinitions;

            var result = await _mediator.Send(_query);

            return new GridData<GeneratedRetailCodeModel>()
            {
                TotalItems = result.TotalItems,
                Items = result.Items,
            };
        }
        catch (Exception)
        {
            Snackbar.Add("Failed to load generated retail codes", Severity.Error);
            return new();
        }
        finally
        {
            _loading = false;
        }
    }
    #endregion 
}
