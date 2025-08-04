using Shouldly;
using NUnit.Framework;
using Tamro.Madam.Models.ItemMasterdata.Brands;
using Tamro.Madam.Models.ItemMasterdata.Items;
using Tamro.Madam.Ui.Store.Actions.ItemMasterdata.Brands;
using Tamro.Madam.Ui.Store.Reducers.ItemMasterdata;
using Tamro.Madam.Ui.Store.State.ItemMasterdata;

namespace Tamro.Madam.Ui.Tests.Store.Reducers;

[TestFixture]
public class BrandReducersTests
{
    [Test]
    public void SetBrandsToDeleteAction_SetsBrands()
    {
        // Arrange
        var action = new SetBrandsToDeleteAction()
        {
            Brands = new List<BrandModel>()
            {
                new()
                {
                    Name = "Brand",
                    Id = 1,
                }
            }
        };
        var state = new BrandState();

        // Act
        var result = BrandReducers.SetBrandsToDeleteAction(state, action);

        // Assert
        result.DeleteDialogState.Brands.ShouldBeEquivalentTo(action.Brands);
    }

    [Test]
    public void UpdateBrandAttachedItemsAction_MovesItemsToDifferentOverview_Correctly()
    {
        // Arrange
        var deleteDialogState = new BrandDeleteDialogModel() 
        { 
            Overviews = new List<BrandDeletionOverviewModel>()
            {
                new BrandDeletionOverviewModel()
                {
                    Brand = new BrandModel()
                    {
                        Id = 4,
                        Name = "BrandMovedFrom"
                    },
                    AttachedItems = new List<ItemModel>()
                    {
                        new ItemModel()
                        {
                            Id = 100,                          
                        },
                        new ItemModel()
                        {
                            Id = 101,
                        },
                    },
                },
                new BrandDeletionOverviewModel()
                {
                    Brand = new BrandModel()
                    {
                        Id = 5,
                        Name = "BrandMovedTo",
                    },
                    AttachedItems = new List<ItemModel>(),
                },
            }
        };
        var state = new BrandState(deleteDialogState);
        var action = new UpdateBrandAttachedItemsAction()
        {
            NewBrandId = 5,
            OldBrandId = 4,
            UpdatedItemIds = new List<int>() { 101, }
        };

        // Act
        var result = BrandReducers.UpdateBrandAttachedItemsAction(state, action);

        // Assert
        result.DeleteDialogState.Overviews.FirstOrDefault(x => x.Brand.Id == 4).AttachedItems.Count.ShouldBe(1);
        result.DeleteDialogState.Overviews.FirstOrDefault(x => x.Brand.Id == 4).AttachedItems[0].Id.ShouldBe(100);
        result.DeleteDialogState.Overviews.FirstOrDefault(x => x.Brand.Id == 5).AttachedItems.Count.ShouldBe(1);
        result.DeleteDialogState.Overviews.FirstOrDefault(x => x.Brand.Id == 5).AttachedItems[0].Id.ShouldBe(101);
    }

    [Test]
    public void ChangeBrandDeletionOverviewExpansionStateAction_ChangesIsExpanded()
    {
        // Arrange
        var deleteDialogState = new BrandDeleteDialogModel()
        {
            Overviews = new List<BrandDeletionOverviewModel>()
            {
                new BrandDeletionOverviewModel()
                {
                    Brand = new BrandModel()
                    {
                        Id = 4,
                        Name = "Brand1"
                    },
                    IsExpanded = true,
                },
                new BrandDeletionOverviewModel()
                {
                    Brand = new BrandModel()
                    {
                        Id = 5,
                        Name = "Brand2",
                    },
                    AttachedItems = new List<ItemModel>(),
                    IsExpanded = false
                },
            }
        };
        var state = new BrandState(deleteDialogState);
        var action = new ChangeBrandDeletionOverviewExpansionStateAction()
        {
            BrandId = 4,
        };

        // Act
        var result = BrandReducers.ChangeBrandDeletionOverviewExpansionStateAction(state, action);

        // Assert
        result.DeleteDialogState.Overviews.All(x => !x.IsExpanded).ShouldBeTrue();
    }
}