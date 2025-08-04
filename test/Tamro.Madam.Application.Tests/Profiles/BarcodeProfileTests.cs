using System.Reflection;
using AutoFixture;
using AutoMapper;
using Shouldly;
using NUnit.Framework;
using Tamro.Madam.Application.Profiles.ItemMasterdata;
using Tamro.Madam.Models.ItemMasterdata.Barcodes;
using Tamro.Madam.Repository.Entities.ItemMasterdata.Barcodes;
using Tamro.Madam.Repository.Entities.ItemMasterdata.Items;

namespace Tamro.Madam.Application.Tests.Profiles;

[TestFixture]
public class BarcodeProfileTests
{
    private IMapper _mapper;
    private Fixture _fixture;

    [SetUp]
    public void SetUp()
    {
        _mapper = new Mapper(new MapperConfiguration(cfg => cfg.AddMaps(Assembly.GetAssembly(typeof(BarcodeProfile)))));
        _fixture = new Fixture();
    }

    [Test]
    public void Map_Barcode_To_BarcodeGridModel_MapsCorrectly()
    {
        // Arrange
        const string itemName = "Ibumetin";
        const int itemId = 5;
        var entity = new Barcode()
        {
            Item = new Item()
            {
                Id = itemId,
                ItemName = itemName,
            }
        };

        // Act
        var result = _mapper.Map<BarcodeGridModel>(entity);

        // Assert
        result.ItemName.ShouldBe(itemName);
        result.ItemId.ShouldBe(itemId);
    }

    [Test]
    public void Map_BarcodeModel_To_Barcode_MapsCorrectly()
    {
        // Arrange
        var barcodeModel = _fixture.Create<BarcodeModel>();
        barcodeModel.Item.Id = 55;

        // Act
        var barcode = _mapper.Map<Barcode>(barcodeModel);

        // Assert
        barcode.ItemId.ShouldBe(55);
        barcode.Item.ShouldBe(default);
    }
}
