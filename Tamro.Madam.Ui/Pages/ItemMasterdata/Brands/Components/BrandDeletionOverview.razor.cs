using Fluxor;
using MediatR;
using Microsoft.AspNetCore.Components;
using MudBlazor;
using Tamro.Madam.Application.Commands.ItemMasterdata.Items;
using Tamro.Madam.Application.Queries.Brands.Clsf;
using Tamro.Madam.Models.ItemMasterdata.Brands;
using Tamro.Madam.Models.ItemMasterdata.Items;
using Tamro.Madam.Ui.Store.Actions.ItemMasterdata.Brands;

namespace Tamro.Madam.Ui.Pages.ItemMasterdata.Brands.Components;

public partial class BrandDeletionOverview
{
    [EditorRequired]
    [Parameter]
    public BrandDeletionOverviewModel Model { get; set; }
    private HashSet<ItemModel> _selectedItems = new();
    private MudAutocomplete<BrandClsfModel> _newBrandAutoComplete;
    [Inject]
    private IMediator _mediator { get; set; }
    [Inject]
    private IDispatcher _dispatcher { get; set; }

    private async Task OnChangeStateClick()
    {
        _dispatcher.Dispatch(new ChangeBrandDeletionOverviewExpansionStateAction()
        {
            BrandId = Model.Brand.Id,
        });
    }

    private async Task<IEnumerable<BrandClsfModel>> SearchBrands(string value, CancellationToken token)
    {
        try
        {
            var query = new BrandClsfQuery()
            {
                SearchTerm = value,
            };

            var result = await _mediator.Send(query);

            return result.Items.Where(x => x.Id != Model.Brand.Id);
        }
        catch (Exception)
        {
            Snackbar.Add("Failed to load brands", Severity.Error);
            return new List<BrandClsfModel>();
        }
    }

    private async Task ChangeBrand()
    {
        await _newBrandAutoComplete.Validate();
        if (_newBrandAutoComplete.ValidationErrors.Any())
        {
            return;
        }

        var cmd = new ChangeItemsBrandCommand(_selectedItems, Model.NewBrand);
        var result = await _mediator.Send(cmd);

        if (result.Succeeded)
        {
            _dispatcher.Dispatch(new UpdateBrandAttachedItemsAction()
            {
                UpdatedItemIds = _selectedItems.Select(x => x.Id),
                OldBrandId = Model.Brand.Id,
                NewBrandId = Model.NewBrand.Id ?? 0
            });
            _selectedItems = new();
            Snackbar.Add("Items updated to use another brand successfully", Severity.Success);
        }
        else
        {
            Snackbar.Add(result.ErrorMessage, Severity.Error);
        }
    }
}
