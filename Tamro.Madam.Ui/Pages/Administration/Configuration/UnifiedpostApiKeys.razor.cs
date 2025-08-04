using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using MudBlazor;
using Tamro.Madam.Application.Commands.Administration.Configuration.Ubl;
using Tamro.Madam.Application.Queries.Administration.Configuration.Ubl;
using Tamro.Madam.Models.Administration.Configuration.Ubl;
using Tamro.Madam.Models.Common.Dialog;
using Tamro.Madam.Ui.ComponentExtensions;
using Tamro.Madam.Ui.Components.Dialogs;
using Tamro.Madam.Ui.Pages.Administration.Configuration.Components;
using Tamro.Madam.Ui.Utils;

namespace Tamro.Madam.Ui.Pages.Administration.Configuration;

public partial class UnifiedpostApiKeys
{
    #region Properties
    private int _defaultPageSize;
    private bool _loading;
    private UblApiKeyQuery _query { get; set; } = new();
    private MudDataGrid<UblApiKeyModel> _table = new();
    private HashSet<UblApiKeyModel> _selectedUblApiKeys = new();
    #endregion

    #region IoC
    [Inject]
    private UserSettingsUtils _userSettings { get; set; }
    [Inject]
    private IMediator _mediator { get; set; }
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
        await MudDataGridExtensions<UblApiKeyModel>.ResetGridState(_table);
    }

    private async Task<GridData<UblApiKeyModel>> DataSource(GridState<UblApiKeyModel> state)
    {
        try
        {
            _loading = true;
            MudDataGridExtensions<UblApiKeyModel>.ApplyDefaultFilterColumns(state.FilterDefinitions, _table.RenderedColumns);
            _query.OrderBy = state.SortDefinitions.FirstOrDefault()?.SortBy ?? nameof(UblApiKeyModel.E1SoldTo);
            _query.SortDirection = state.SortDefinitions.FirstOrDefault()?.Descending ?? false ? nameof(SortDirection.Descending) : nameof(SortDirection.Ascending);
            _query.PageNumber = state.Page + 1;
            _query.PageSize = state.PageSize;
            _query.Filters = state.FilterDefinitions;

            var result = await _mediator.Send(_query);

            return new GridData<UblApiKeyModel>()
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

    private async Task OnDeleteSingle(UblApiKeyModel model)
    {
        await OnDelete(new[] { model.E1SoldTo }, count: 1);
    }


    private async Task OnDeleteChecked()
    {
        var e1SoldTosToDelete = _selectedUblApiKeys.Select(x => x.E1SoldTo).ToArray();
        await OnDelete(e1SoldTosToDelete, _selectedUblApiKeys.Count);
    }

    private async Task OnDelete(int[] e1SoldTos, int count)
    {
        var command = new DeleteUblApiKeysCommand(e1SoldTos);
        var parameters = new DialogParameters<ConfirmationDialog>
        {
            { x => x.Command, command },
            { x => x.Title, "Delete UnifiedPost API keys" },
            { x => x.Content, $"Are you sure you want to delete {count} UnifiedPost API keys?" },
            { x => x.SuccessMessage, $"{count} UnifiedPost API keys deleted" },
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
            _selectedUblApiKeys.Clear();
        }
    }

    private async Task OnCreate()
    {
        await ShowDetailsDialog(new UblApiKeyEditModel(), DialogState.Create);
    }

    private async Task OnOpen()
    {
        await OnOpen(_selectedUblApiKeys.First());
    }

    private async Task OnOpen(UblApiKeyModel? model)
    {
        var ublApiKey = _mapper.Map<UblApiKeyEditModel>(model);
        await ShowDetailsDialog(ublApiKey, DialogState.View);
    }

    private async Task ShowDetailsDialog(UblApiKeyEditModel? model, DialogState dialogState)
    {
        var parameters = new DialogParameters<UnifiedpostApiKeyDetailsDialog>
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
        var dialog = DialogService.Show<UnifiedpostApiKeyDetailsDialog>(string.Empty, parameters, options);
        var state = await dialog.Result;

        await _table.ReloadServerData();
    }
    #endregion
}