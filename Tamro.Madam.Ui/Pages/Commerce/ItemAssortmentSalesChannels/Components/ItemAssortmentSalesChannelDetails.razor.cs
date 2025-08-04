using BlazorDownloadFile;
using Fluxor;
using MediatR;
using Microsoft.AspNetCore.Components;
using MudBlazor;
using Tamro.Madam.Application.Commands.Commerce.ItemAssortmentSalesChannels;
using Tamro.Madam.Application.Services.Files;
using Tamro.Madam.Application.Validation;
using Tamro.Madam.Common.Utils;
using Tamro.Madam.Models.Commerce.ItemAssortmentSalesChannels;
using Tamro.Madam.Models.General;
using Tamro.Madam.Ui.ComponentExtensions;
using Tamro.Madam.Ui.Store.Actions.Commerce.ItemAssortmentSalesChannels;
using Tamro.Madam.Ui.Store.State;
using Tamro.Madam.Ui.Store.State.Commerce.ItemAssortmentSalesChannels;
using Tamro.Madam.Ui.Utils;

namespace Tamro.Madam.Ui.Pages.Commerce.ItemAssortmentSalesChannels.Components;

public partial class ItemAssortmentSalesChannelDetails
{
    [Inject]
    private UserSettingsUtils _userSettings { get; set; }
    [Inject]
    private IDispatcher _dispatcher { get; set; }
    [Inject]
    private IValidationService _validator { get; set; }
    [Inject]
    private IMediator _mediator { get; set; }
    [Inject]
    private IExcelService _excelService { get; set; }
    [Inject]
    private IBlazorDownloadFileService _blazorDownloadFileService { get; set; }
    [Inject]
    private IState<UserProfileState> _userProfileState { get; set; }
    [Inject]
    private IState<ItemAssortmentSalesChannelState> _state { get; set; }

    private bool _saving;
    private MudForm? _form;
    private int _defaultPageSize;
    private HashSet<ItemAssortmentGridModel> _selectedAssortment = new();
    private MudDataGrid<ItemAssortmentGridModel> _assortmentGrid = new();

    protected override async Task OnInitializedAsync()
    {
        _defaultPageSize = _userSettings.GetRowsPerPageSetting();
        await _assortmentGrid.SetSortAsync(nameof(ItemAssortmentGridModel.ItemName), SortDirection.Ascending, x => x.ItemName);
    }

    protected override Task OnAfterRenderAsync(bool firstRender)
    {
        if (firstRender)
        {
            StateHasChanged();
        }
        return base.OnAfterRenderAsync(firstRender);
    }

    private async Task OnExportAssortment()
    {
        var mappers = new Dictionary<string, Func<ItemAssortmentGridModel, object?>>
        {
            { DisplayNameHelper.Get(typeof(ItemAssortmentGridModel), nameof(ItemAssortmentGridModel.ItemCode)), i => i.ItemCode },
            { DisplayNameHelper.Get(typeof(ItemAssortmentGridModel), nameof(ItemAssortmentGridModel.ItemName)), i => i.ItemName },
        };

        var fileContent = await _excelService.ExportAsync(_state.Value.Model.Assortment, mappers, $"item_assortment");
        await _blazorDownloadFileService.DownloadFile($"ItemAssortment_{_state.Value.Model.Name}.xlsx", fileContent, "application/octet-stream");
        Snackbar.Add("Item assortment exported successfully", Severity.Success);
    }

    private async Task OnResetAssortmentGrid()
    {
        await MudDataGridExtensions<ItemAssortmentGridModel>.ResetGridState(_assortmentGrid);
    }

    private async Task OnDeleteSingleAssortment(ItemAssortmentGridModel itemAssortment)
    {
        await OnDelete([itemAssortment]);
    }

    private async Task OnImportAssortment()
    {
        var parameters = new DialogParameters<ItemAssortmentImportDialog>();
        var options = new DialogOptions
        {
            CloseButton = true,
            Position = DialogPosition.TopCenter,
            MaxWidth = MaxWidth.Large,
            CloseOnEscapeKey = true,
            FullWidth = true,
        };

        var dialog = DialogService.Show<ItemAssortmentImportDialog>(string.Empty, parameters, options);
        await dialog.Result;
    }

    private async Task OnDeleteAssortment()
    {
        await OnDelete(_selectedAssortment);
    }

    private async Task OnDelete(HashSet<ItemAssortmentGridModel> itemAssortments)
    {
        _state.Value.Model.Assortment = _state.Value.Model.Assortment.Where(ia => !itemAssortments.Any(x => x.ItemCode == ia.ItemCode));
    }

    private async Task Submit()
    {
        try
        {
            _saving = true;
            await _form!.Validate();

            if (!_form.IsValid)
            {
                return;
            }

            _state.Value.Model.Country = _userProfileState.Value.UserProfile.Country ?? BalticCountry.LT;
            var result = await _mediator.Send(new UpsertItemAssortmentSalesChannelCommand(_state.Value.Model));

            if (result.Succeeded)
            {
                Snackbar.Add("Item assortment sales channel " + (_state.Value.Model.Id == default ? "created" : "updated"), Severity.Success);
                _dispatcher.Dispatch(new RefreshGridAction());
                OnClose();
            }
            else
            {
                Snackbar.Add(result.ErrorMessage, Severity.Error);
            }
        }
        finally
        {
            _saving = false;
        }
    }

    private void OnClose()
    {
        _dispatcher.Dispatch(new SetActiveFormAction(ItemAssortmentSalesChannelForm.Grid));
    }
}
