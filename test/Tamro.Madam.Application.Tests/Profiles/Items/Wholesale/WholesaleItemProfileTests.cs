using AutoMapper;
using Shouldly;
using NUnit.Framework;
using System.Reflection;
using Tamro.Madam.Application.Profiles.ItemMasterdata.Items.Wholesale;
using Tamro.Madam.Models.ItemMasterdata.Items.Wholesale;
using Tamro.Madam.Models.ItemMasterdata.Items.Wholesale.Clsf;
using Tamro.Madam.Repository.Entities.Wholesale.Ee;
using Tamro.Madam.Repository.Entities.Wholesale.Lt;
using Tamro.Madam.Repository.Entities.Wholesale.Lv;

namespace Tamro.Madam.Application.Tests.Profiles.Items.Wholesale;

[TestFixture]
public class WholesaleItemProfileTests
{
    private IMapper _mapper;

    [SetUp]
    public void SetUp()
    {
        _mapper = new Mapper(new MapperConfiguration(cfg => cfg.AddMaps(Assembly.GetAssembly(typeof(WholesaleItemProfile)))));
    }

    [Test]
    public void LtWholesaleItem_To_WholesaleItemClsfModel_MapsCorrectly()
    {
        // Arrange
        var ltWholesaleItem = new LtWholesaleItem()
        {
            ItemNo = "12345",
            ItemDescription = "Paracetamol",
        };

        // Act
        var result = _mapper.Map<WholesaleItemClsfModel>(ltWholesaleItem);

        // Assert
        result.Name.ShouldBe("Paracetamol");
        result.ItemNo.ShouldBe("12345");
    }

    [Test]
    public void LvWholesaleItem_To_WholesaleItemClsfModel_MapsCorrectly()
    {
        // Arrange
        var lvWholesaleItem = new LvWholesaleItem()
        {
            ItemNo = "12345",
            ItemDescription = "Paracetamol",
        };

        // Act
        var result = _mapper.Map<WholesaleItemClsfModel>(lvWholesaleItem);

        // Assert
        result.Name.ShouldBe("Paracetamol");
        result.ItemNo.ShouldBe("12345");
    }

    [Test]
    public void EeWholesaleItem_To_WholesaleItemClsfModel_MapsCorrectly()
    {
        // Arrange
        var eeWholesaleItem = new EeWholesaleItem()
        {
            ItemNo = "12345 ",
            ItemDescription = "Paracetamol ",
        };

        // Act
        var result = _mapper.Map<WholesaleItemClsfModel>(eeWholesaleItem);

        // Assert
        result.Name.ShouldBe("Paracetamol");
        result.ItemNo.ShouldBe("12345");
    }

    [Test]
    public void EeWholesaleItem_To_WholesaleItemModel_MapsCorrectly()
    {
        // Arrange
        var eeWholesaleItem = new EeWholesaleItem()
        {
            ItemNo = " 12345   ",
            ItemDescription = " Paracetamol ",
        };

        // Act
        var result = _mapper.Map<WholesaleItemModel>(eeWholesaleItem);

        // Assert
        result.ItemDescription.ShouldBe("Paracetamol");
        result.ItemNo.ShouldBe("12345");
    }
}