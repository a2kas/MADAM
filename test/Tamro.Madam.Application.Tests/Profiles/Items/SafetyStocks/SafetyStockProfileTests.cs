using AutoMapper;
using Shouldly;
using NUnit.Framework;
using System.Reflection;
using Tamro.Madam.Application.Profiles.ItemMasterdata.Items.SafetyStocks;
using Tamro.Madam.Models.General;
using Tamro.Madam.Models.ItemMasterdata.Items.SafetyStocks;

namespace Tamro.Madam.Application.Tests.Profiles.Items.SafetyStocks;

[TestFixture]
public class SafetyStockProfileTests
{
    private IMapper _mapper;

    [SetUp]
    public void SetUp()
    {
        _mapper = new Mapper(new MapperConfiguration(cfg => cfg.AddMaps(Assembly.GetAssembly(typeof(SafetyStockProfile)))));
    }

    [Test]
    public void SafetyStockGridDataModel_To_SafetyStockUpsertModel_MapsCorrectly()
    {
        // Arrange
        var src = new SafetyStockGridDataModel()
        {
            SafetyStockPharmacyChainGroup = nameof(PharmacyGroup.Benu),
            PharmacyChainDisplayName = "Test",
        };

        // Act
        var dest = _mapper.Map<SafetyStockConditionUpsertModel>(src);

        // Assert
        dest.PharmacyGroup.ShouldBe(PharmacyGroup.Benu);
        dest.PharmacyChainName.ShouldBe("Test");
    }

    [Test]
    public void SafetyStockUpsertModel_To_SafetyStockGridDataModel_MapsCorrectly()
    {
        // Arrange
        var src = new SafetyStockConditionUpsertModel()
        {
            PharmacyGroup = PharmacyGroup.Benu,
            PharmacyChainName = "TestPharmacyChain",
        };

        // Act
        var dest = _mapper.Map<SafetyStockGridDataModel>(src);

        // Assert
        dest.SafetyStockPharmacyChainGroup.ShouldBe(nameof(PharmacyGroup.Benu));
        dest.PharmacyChainDisplayName.ShouldBe("TestPharmacyChain");
    }
}