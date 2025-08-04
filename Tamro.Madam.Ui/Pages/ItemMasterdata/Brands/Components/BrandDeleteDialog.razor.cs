using Fluxor;
using MediatR;
using Microsoft.AspNetCore.Components;
using MudBlazor;
using Tamro.Madam.Application.Commands.Brands;
using Tamro.Madam.Application.Commands.ItemMasterdata.Brands;
using Tamro.Madam.Models.ItemMasterdata.Brands;
using Tamro.Madam.Ui.Store.Actions.ItemMasterdata.Brands;
using Tamro.Madam.Ui.Store.State.ItemMasterdata;

namespace Tamro.Madam.Ui.Pages.ItemMasterdata.Brands.Components;

public partial class BrandDeleteDialog
{
    private bool _isLoading;
    [CascadingParameter]
    private IMudDialogInstance _dialog { get; set; } = default!;

    [Inject]
    private IState<BrandState> _brandState { get; set; }
    [Inject]
    private IMediator _mediator { get; set; }
    [Inject]
    private IDispatcher _dispatcher { get; set; }
    [Inject]
    private IActionSubscriber _actionSubscriber { get; set; }

    protected override async Task OnInitializedAsync()
    {
        _actionSubscriber?.SubscribeToAction<UpdateBrandAttachedItemsAction>(this, _ => StateHasChanged());
        _actionSubscriber?.SubscribeToAction<ChangeBrandDeletionOverviewExpansionStateAction>(this, _ => StateHasChanged());
        _actionSubscriber?.SubscribeToAction<SetBrandsDeleteOverviewAction>(this, _ => StateHasChanged());

        _isLoading = true;
        await UpdateBrandsDeletionOverview(_brandState.Value.DeleteDialogState.Brands.ToList());
        _isLoading = false;
    }

    private async Task OnDelete()
    {
        var deletableIds = _brandState.Value.DeleteDialogState.Overviews?.Where(x => x.IsDeletable).Select(x => x.Brand.Id).ToList();
        var result = await _mediator.Send(new DeleteBrandsCommand(deletableIds));
        if (result.Succeeded)
        {
            Snackbar.Add("Brands deleted successfully", Severity.Success);
            var deletableBrands = _brandState.Value.DeleteDialogState.Brands.Where(x => !deletableIds.Contains(x.Id));
            if (deletableBrands.Any())
            {
                _dispatcher.Dispatch(new SetBrandsToDeleteAction()
                {
                    Brands = deletableBrands,
                });
                await UpdateBrandsDeletionOverview(deletableBrands.ToList());
            }
            else
            {
                _dialog.Close(DialogResult.Ok(true));
            }
        }
        else
        {
            Snackbar.Add(result.ErrorMessage, Severity.Error);
        }
    }

    private async Task UpdateBrandsDeletionOverview(List<BrandModel> brands)
    {
        var brandsDeletionOverviewResult = await _mediator.Send(new GetBrandsDeletionOverviewCommand(brands));
        if (brandsDeletionOverviewResult.Succeeded)
        {
            _dispatcher.Dispatch(new SetBrandsDeleteOverviewAction()
            {
                DeletionOverviews = brandsDeletionOverviewResult.Data,
            });
        }
        else
        {
            Snackbar.Add("Failed to load deletion overview", Severity.Error);
        }
    }

    private void Cancel() => _dialog.Cancel();
}
