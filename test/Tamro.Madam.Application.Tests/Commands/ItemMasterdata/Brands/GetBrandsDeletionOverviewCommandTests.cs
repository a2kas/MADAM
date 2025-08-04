using AutoFixture;
using Shouldly;
using NUnit.Framework;
using Tamro.Madam.Application.Commands.ItemMasterdata.Brands;
using Tamro.Madam.Models.ItemMasterdata.Brands;

namespace Tamro.Madam.Application.Tests.Commands.ItemMasterdata.Brands;

[TestFixture]
public class GetBrandsDeletionOverviewCommandTests
{
    private Fixture _fixture;

    [SetUp]
    public void SetUp()
    {
        _fixture = new Fixture();
    }
    
    [Test]
    public void GetBrandsDeletionOverviewCommand_Ctor_Sets_Brands()
    {
        // Arrange
        var brands = _fixture.CreateMany<BrandModel>(2).ToList();

        // Act
        var cmd = new GetBrandsDeletionOverviewCommand(brands);

        // Assert
        cmd.Brands.ShouldBeEquivalentTo(brands);
    }
}