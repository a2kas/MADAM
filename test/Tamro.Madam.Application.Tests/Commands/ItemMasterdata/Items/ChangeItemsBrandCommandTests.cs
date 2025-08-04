using AutoFixture;
using Shouldly;
using NUnit.Framework;
using Tamro.Madam.Application.Commands.ItemMasterdata.Items;
using Tamro.Madam.Models.ItemMasterdata.Brands;
using Tamro.Madam.Models.ItemMasterdata.Items;

namespace Tamro.Madam.Application.Tests.Commands.ItemMasterdata.Items;

[TestFixture]
public class ChangeItemsBrandCommandTests
{
    private Fixture _fixture;

    [SetUp]
    public void SetUp()
    {
        _fixture = new Fixture();
    }

    [Test]
    public void ChangeItemsBrandCommand_Ctor_Sets_Properties()
    {
        // Arrange
        var items = _fixture.CreateMany<ItemModel>(2);
        var brand = _fixture.Create<BrandClsfModel>();

        // Act
        var cmd = new ChangeItemsBrandCommand(items, brand);

        // Assert
        cmd.Items.ShouldBeEquivalentTo(items);
        cmd.NewBrand.ShouldBeEquivalentTo(brand);
    }
}