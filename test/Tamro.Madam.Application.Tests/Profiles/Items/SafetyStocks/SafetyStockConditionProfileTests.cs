using AutoMapper;
using Shouldly;
using NUnit.Framework;
using System.Reflection;
using Tamro.Madam.Application.Profiles.ItemMasterdata.Items.SafetyStocks;
using Tamro.Madam.Models.General;
using Tamro.Madam.Models.ItemMasterdata.Items.SafetyStocks;
using Tamro.Madam.Repository.Entities.ItemMasterdata.Items.SafetyStocks;

namespace Tamro.Madam.Application.Tests.Profiles.Items.SafetyStocks;

[TestFixture]
public class SafetyStockConditionProfileTests
{
    private IMapper _mapper;

    [SetUp]
    public void SetUp()
    {
        _mapper = new Mapper(new MapperConfiguration(cfg => cfg.AddMaps(Assembly.GetAssembly(typeof(SafetyStockConditionProfile)))));
    }

    [Test]
    public void SafetyStockCondition_To_SafetyStockConditionModel_MapsCorrectly()
    {
        // Arrange
        var src = new SafetyStockCondition()
        {
            SafetyStockItem = new SafetyStockItem()
            {
                CheckDays = 15,
            }
        };

        // Act
        var dest = _mapper.Map<SafetyStockConditionModel>(src);

        // Assert
        dest.CheckDays.ShouldBe(15);
    }

    [TestCase(1, null, SafetyStockRestrictionLevel.PharmacyChain)]
    [TestCase(null, PharmacyGroup.All, SafetyStockRestrictionLevel.PharmacyChainGroup)]
    public void SafetyStockUpsertModel_To_SafetyStockConditionModel_MapsRestrictionLevelCorrectly(int? pharmacyChainId,
        PharmacyGroup? pharmacyGroup, SafetyStockRestrictionLevel expectedSafetyStockRestrictionLevel)
    {
        // Arrange
        var src = new SafetyStockConditionUpsertModel()
        {
            PharmacyGroup = pharmacyGroup,
            PharmacyChainId = pharmacyChainId,
        };

        // Act
        var dest = _mapper.Map<SafetyStockConditionModel>(src);

        // Assert
        dest.RestrictionLevel.ShouldBe(expectedSafetyStockRestrictionLevel);
    }

    [Test]
    public void SafetyStockUpsertModel_To_SafetyStockConditionModel_MapsCorrectly()
    {
        // Arrange
        var src = new SafetyStockConditionUpsertModel()
        {
            PharmacyChainId = 1,
            PharmacyGroup = PharmacyGroup.Benu,
        };

        // Act
        var dest = _mapper.Map<SafetyStockConditionModel>(src);

        // Assert
        dest.SafetyStockPharmacyChainId.ShouldBe(1);
        dest.SafetyStockPharmacyChainGroup.ShouldBe(PharmacyGroup.Benu);
    }
}