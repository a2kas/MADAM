using BlazorDownloadFile;
using Fluxor;
using MediatR;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using MudBlazor;
using Tamro.Madam.Application.Commands.Commerce.ItemAssortmentSalesChannels;
using Tamro.Madam.Application.Queries.Commerce.ItemAssortmentSalesChannels;
using Tamro.Madam.Application.Services.Files;
using Tamro.Madam.Common.Utils;
using Tamro.Madam.Models.Commerce.ItemAssortmentSalesChannels;
using Tamro.Madam.Repository.Entities.Commerce.Assortment;
using Tamro.Madam.Ui.ComponentExtensions;
using Tamro.Madam.Ui.Components.Audit;
using Tamro.Madam.Ui.Components.Dialogs;
using Tamro.Madam.Ui.Store.Actions.Commerce.ItemAssortmentSalesChannels;
using Tamro.Madam.Ui.Store.State;
using Tamro.Madam.Ui.Utils;

namespace Tamro.Madam.Ui.Pages.Commerce.ItemAssortmentSalesChannels.Components;

public partial class ItemAssortmentSalesChannelsDataGrid
{
    #region Properties
    private int _defaultPageSize;
    private bool _loading;
    private HashSet<ItemAssortmentSalesChannelGridModel> _selectedItemAssortmentSalesChannels = new();
    private ItemAssortmentSalesChannelQuery _query { get; set; } = new();
    private MudDataGrid<ItemAssortmentSalesChannelGridModel> _table = new();
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
    private IActionSubscriber _actionSubscriber { get; set; }
    [Inject]
    private IDispatcher _dispatcher { get; set; }
    [Inject]
    private IState<UserProfileState> _userProfileState { get; set; }
    #endregion

    #region Events
    protected override async Task OnInitializedAsync()
    {
        _defaultPageSize = _userSettings.GetRowsPerPageSetting();
        _actionSubscriber?.SubscribeToAction<RefreshGridAction>(this, _ => OnReset());
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
        await MudDataGridExtensions<ItemAssortmentSalesChannelGridModel>.ResetGridState(_table);
    }

    private async Task OnCreate()
    {
        _dispatcher.Dispatch(new SetActiveFormAction(ItemAssortmentSalesChannelForm.Details));
    }

    private async Task OnOpen()
    {
        await OnOpen(_selectedItemAssortmentSalesChannels.First());
    }

    private async Task OnOpen(ItemAssortmentSalesChannelGridModel itemAssortmentSalesChannel)
    {
        if (itemAssortmentSalesChannel == null)
        {
            return;
        }

        _loading = true;

        var command = new GetItemAssortmentSalesChannelCommand(itemAssortmentSalesChannel.Id);
        var result = await _mediator.Send(command);

        _loading = false;

        if (result.Succeeded)
        {
            _dispatcher.Dispatch(new SetActiveFormAction(ItemAssortmentSalesChannelForm.Details, result.Data));
        }
        else
        {
            Snackbar.Add(result.ErrorMessage, Severity.Error);
        }
    }

    private async Task OnDeleteSingle(ItemAssortmentSalesChannelGridModel itemAssortmentSalesChannel)
    {
        await OnDelete([itemAssortmentSalesChannel]);
    }

    private async Task OnDeleteChecked()
    {
        await OnDelete(_selectedItemAssortmentSalesChannels);
    }

    private async Task OnDelete(HashSet<ItemAssortmentSalesChannelGridModel> itemAssortmentSalesChannels)
    {
        var command = new DeleteItemAssortmentSalesChannelCommand(itemAssortmentSalesChannels.Select(x => x.Id).ToArray());
        var parameters = new DialogParameters<ConfirmationDialog>
        {
            { x => x.Command, command },
            { x => x.Title, "Delete item assortment sales channels" },
            { x => x.Content, $"Are you sure you want to delete {itemAssortmentSalesChannels.Count} item assortment sales channels?" },
            { x => x.SuccessMessage, $"{itemAssortmentSalesChannels.Count} item assortment sales channels deleted" },
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
            _selectedItemAssortmentSalesChannels.Clear();
        }
    }

    private async Task OnExport()
    {
        try
        {
            _loading = true;
            var query = new ItemAssortmentSalesChannelQuery()
            {
                Filters = _table.FilterDefinitions,
                OrderBy = _table.SortDefinitions.Values.FirstOrDefault()?.SortBy ?? nameof(ItemAssortmentSalesChannelGridModel.Name),
                SortDirection = _table.SortDefinitions.Values.FirstOrDefault()?.Descending ?? false ? SortDirection.Descending.ToString() : SortDirection.Ascending.ToString(),
                PageNumber = 1,
                PageSize = int.MaxValue,
                SearchTerm = _query.SearchTerm,
                Country = _userProfileState.Value.UserProfile.Country,
            };
            var exportableItemAssortmentSalesChannels = await _mediator.Send(query);

            var mappers = new Dictionary<string, Func<ItemAssortmentSalesChannelGridModel, object?>>
            {
                { DisplayNameHelper.Get(typeof(ItemAssortmentSalesChannelGridModel), nameof(ItemAssortmentSalesChannelGridModel.Name)), b => b.Name },
                { DisplayNameHelper.Get(typeof(ItemAssortmentSalesChannelGridModel), nameof(ItemAssortmentSalesChannelGridModel.ItemsCount)), b => b.ItemsCount },
            };
            var fileContent = await _excelService.ExportAsync(exportableItemAssortmentSalesChannels.Items, mappers, "item_assortment_sales_channels");
            await _blazorDownloadFileService.DownloadFile("ItemAssortmentSalesChannels.xlsx", fileContent, "application/octet-stream");
            Snackbar.Add("Item assortment sales channels exported successfully", Severity.Success);
        }
        catch (Exception)
        {
            Snackbar.Add("Failed to export item assortment sales channels", Severity.Error);
        }
        finally
        {
            _loading = false;
        }
    }

    private async Task OnHistoryView()
    {
        await OnHistoryView(_selectedItemAssortmentSalesChannels.First());
    }

    private async Task OnHistoryView(ItemAssortmentSalesChannelGridModel? itemAssortmentSalesChannel)
    {
        await ShowHistoryDialog(itemAssortmentSalesChannel.Id);
    }

    private async Task ShowHistoryDialog(int id)
    {
        var parameters = new DialogParameters<EntityHistory>
        {
            { x => x.EntityTypeName, nameof(ItemAssortmentSalesChannel) },
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

        await DialogService.ShowAsync<EntityHistory>($"Item assortment sales channel {id} history", parameters, options);
    }

    private async Task<GridData<ItemAssortmentSalesChannelGridModel>> DataSource(GridState<ItemAssortmentSalesChannelGridModel> state)
    {
        try
        {
            _loading = true;
            _query.OrderBy = state.SortDefinitions.FirstOrDefault()?.SortBy ?? nameof(ItemAssortmentSalesChannel.Name);
            _query.SortDirection = state.SortDefinitions.FirstOrDefault()?.Descending ?? true ? nameof(SortDirection.Ascending) : nameof(SortDirection.Descending);
            _query.PageNumber = state.Page + 1;
            _query.PageSize = state.PageSize;
            _query.Filters = state.FilterDefinitions;
            _query.Country = _userProfileState.Value.UserProfile.Country;

            var result = await _mediator.Send(_query);

            return new GridData<ItemAssortmentSalesChannelGridModel>()
            {
                TotalItems = result.TotalItems,
                Items = result.Items,
            };
        }
        catch (Exception)
        {
            Snackbar.Add("Failed to load item assortment sales channels", Severity.Error);
            return new();
        }
        finally
        {
            _loading = false;
        }
    }
    #endregion 
}
