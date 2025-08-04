using Shouldly;
using NUnit.Framework;
using Tamro.Madam.Models.ItemMasterdata.Brands;
using Tamro.Madam.Models.ItemMasterdata.Items;

namespace Tamro.Madam.Models.Tests.ItemMasterdata.Brands;

[TestFixture]
public class BrandDeletionOverviewModelTests
{
    [Test]
    public void BrandDeletionOverviewModel_WhenHasAttachedItems_ShouldNotBeDeletable()
    {
        // Arrange
        var attachedItems = new List<ItemModel>()
        {
            new(),
        };

        // Act
        var model = new BrandDeletionOverviewModel()
        {
            AttachedItems = attachedItems,
        };

        // Assert
        model.IsDeletable.ShouldBeFalse();
    }

    [Test]
    public void BrandDeleteOverviewModel_WhenHasNoAttachedItems_ShouldBeDeletable()
    {
        // Act
        var model = new BrandDeletionOverviewModel();

        // Assert
        model.IsDeletable.ShouldBeTrue();
    }
}