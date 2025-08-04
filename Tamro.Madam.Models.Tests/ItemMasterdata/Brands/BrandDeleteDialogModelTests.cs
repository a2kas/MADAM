using Shouldly;
using NUnit.Framework;
using Tamro.Madam.Models.ItemMasterdata.Brands;
using Tamro.Madam.Models.ItemMasterdata.Items;

namespace Tamro.Madam.Models.Tests.ItemMasterdata.Brands;

[TestFixture]
public class BrandDeleteDialogModelTests
{
    [Test]
    public void BrandDeleteDialogModel_Overviews_AreOrderedByBrandName()
    {
        // Arrange
        var overviews = new List<BrandDeletionOverviewModel>()
        {
            new()
            {
                Brand = new()
                {
                    Name = "B",
                },
            },
            new()
            {
                Brand = new()
                {
                    Name = "A",
                }
            },
            new()
            {
                Brand = new()
                {
                    Name = "C",
                },
            },
        };
        var model = new BrandDeleteDialogModel();

        // Act
        model.Overviews = overviews;

        // Assert
        model.Overviews.ElementAt(0).Brand.Name.ShouldBe("A");
        model.Overviews.ElementAt(1).Brand.Name.ShouldBe("B");
        model.Overviews.ElementAt(2).Brand.Name.ShouldBe("C");
    }

    [Test]
    public void BrandDeleteDialogModel_WhenAnyOverviewIsDeletable_IsDeleteButtonEnabledShouldBeTrue()
    {
        // Arrange
        var overviews = new List<BrandDeletionOverviewModel>()
        {
            new(),
            new()
            {
                AttachedItems = new List<ItemModel>()
                {
                    new()
                }
            },
        };
        var model = new BrandDeleteDialogModel();

        // Act
        model.Overviews = overviews;

        // Assert
        model.IsDeleteButtonEnabled.ShouldBeTrue();
    }

    [Test]
    public void BrandDeleteDialogModel_WhenNoOverviewIsDeletable_IsDeleteButtonEnabledShouldBeFalse()
    {
        // Arrange
        var overviews = new List<BrandDeletionOverviewModel>()
        {
            new()
            {
                AttachedItems = new List<ItemModel>()
                {
                    new(),
                }
            },
        };
        var model = new BrandDeleteDialogModel();

        // Act
        model.Overviews = overviews;

        // Assert
        model.IsDeleteButtonEnabled.ShouldBeFalse();
    }
}