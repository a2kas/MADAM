using Fluxor;
using Tamro.Madam.Models.ItemMasterdata.Brands;
using Tamro.Madam.Ui.Store.Actions.ItemMasterdata.Brands;
using Tamro.Madam.Ui.Store.State.ItemMasterdata;

namespace Tamro.Madam.Ui.Store.Reducers.ItemMasterdata;

public class BrandReducers
{
    [ReducerMethod]
    public static BrandState SetBrandsToDeleteAction(BrandState state, SetBrandsToDeleteAction action)
    {
        var newState = new BrandState(state.DeleteDialogState);
        newState.DeleteDialogState.Brands = action.Brands;

        return newState;
    }

    [ReducerMethod]
    public static BrandState SetBrandsDeleteOverviewAction(BrandState state, SetBrandsDeleteOverviewAction action)
    {
        var newState = new BrandState(state.DeleteDialogState);
        newState.DeleteDialogState.Overviews = action.DeletionOverviews;

        return newState;
    }

    [ReducerMethod]
    public static BrandState UpdateBrandAttachedItemsAction(BrandState state, UpdateBrandAttachedItemsAction action)
    {
        var overviewDeletedFrom = state.DeleteDialogState.Overviews.FirstOrDefault(x => x.Brand.Id == action.OldBrandId);
        var overviewAddedTo = state.DeleteDialogState.Overviews.FirstOrDefault(x => x.Brand.Id == action.NewBrandId);
        if (overviewAddedTo != null)
        {
            overviewAddedTo.AttachedItems.AddRange(overviewDeletedFrom.AttachedItems.Where(x => action.UpdatedItemIds.Contains(x.Id)));
        }

        overviewDeletedFrom.AttachedItems = overviewDeletedFrom.AttachedItems.Where(x => !action.UpdatedItemIds.Contains(x.Id)).ToList();

        var updatedOverviews = state.DeleteDialogState.Overviews
            .Where(overview => overview.Brand.Id != action.OldBrandId && overview.Brand.Id != action.NewBrandId)
            .Append(overviewDeletedFrom);

        if (overviewAddedTo != null)
        {
            updatedOverviews = updatedOverviews.Append(overviewAddedTo);
        }

        return new BrandState(new BrandDeleteDialogModel()
        {
            Brands = state.DeleteDialogState.Brands,
            Overviews = updatedOverviews
        });
    }

    [ReducerMethod]
    public static BrandState ChangeBrandDeletionOverviewExpansionStateAction(BrandState state, ChangeBrandDeletionOverviewExpansionStateAction action)
    {
        var overview = state.DeleteDialogState.Overviews.FirstOrDefault(x => x.Brand.Id == action.BrandId);
        overview.IsExpanded = !overview.IsExpanded;

        var updatedOverviews = state.DeleteDialogState.Overviews
            .Where(overview => overview.Brand.Id != action.BrandId)
            .Append(overview)
            .ToList();

        return new BrandState(new BrandDeleteDialogModel()
        {
            Brands = state.DeleteDialogState.Brands,
            Overviews = updatedOverviews
        });
    }
}
