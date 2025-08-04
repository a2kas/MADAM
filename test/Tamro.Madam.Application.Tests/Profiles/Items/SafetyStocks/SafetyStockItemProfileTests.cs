using AutoMapper;
using Shouldly;
using NUnit.Framework;
using System.Reflection;
using Tamro.Madam.Application.Profiles.ItemMasterdata.Items.SafetyStocks;
using Tamro.Madam.Models.ItemMasterdata.Items.SafetyStocks;
using Tamro.Madam.Repository.Entities.Wholesale;

namespace Tamro.Madam.Application.Tests.Profiles.Items.SafetyStocks;

[TestFixture]
public class SafetyStockItemProfileTests
{
    private IMapper _mapper;

    [SetUp]
    public void SetUp()
    {
        _mapper = new Mapper(new MapperConfiguration(cfg => cfg.AddMaps(Assembly.GetAssembly(typeof(SafetyStockItemProfile)))));
    }

    [TestCase("NAR", 30)]
    [TestCase("PSY", 30)]
    [TestCase("Regular", 10)]
    public void Map_WholesaleSafetyStockItem_To_SafetyStockGridDataModel_MapsCorrectly(string itemGroup, int expectedCheckDays)
    {
        // Arrange
        var src = new WholesaleSafetyStockItem()
        {
            ItemGroup = itemGroup,
        };

        // Act
        var result = _mapper.Map<SafetyStockGridDataModel>(src);

        // Assert
        result.CheckDays.ShouldBe(expectedCheckDays);
    }
}