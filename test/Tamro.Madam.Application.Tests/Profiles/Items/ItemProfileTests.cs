using System.Reflection;
using AutoMapper;
using Shouldly;
using NUnit.Framework;
using Tamro.Madam.Application.Profiles.ItemMasterdata.Items;
using Tamro.Madam.Models.ItemMasterdata.Atcs;
using Tamro.Madam.Models.ItemMasterdata.Brands;
using Tamro.Madam.Models.ItemMasterdata.Forms;
using Tamro.Madam.Models.ItemMasterdata.Items;
using Tamro.Madam.Models.ItemMasterdata.MeasurementUnits;
using Tamro.Madam.Models.ItemMasterdata.Nicks;
using Tamro.Madam.Models.ItemMasterdata.Producers;
using Tamro.Madam.Models.ItemMasterdata.Requestors;
using Tamro.Madam.Repository.Entities.Atcs;
using Tamro.Madam.Repository.Entities.Brands;
using Tamro.Madam.Repository.Entities.ItemMasterdata.Forms;
using Tamro.Madam.Repository.Entities.ItemMasterdata.Items;
using Tamro.Madam.Repository.Entities.ItemMasterdata.MeasurementUnits;
using Tamro.Madam.Repository.Entities.ItemMasterdata.Nicks;
using Tamro.Madam.Repository.Entities.ItemMasterdata.Producers;

namespace Tamro.Madam.Application.Tests.Profiles.Items;

[TestFixture]
public class ItemProfileTests
{
    private IMapper _mapper;

    [SetUp]
    public void SetUp()
    {
        _mapper = new Mapper(new MapperConfiguration(cfg => cfg.AddMaps(Assembly.GetAssembly(typeof(ItemProfile)))));
    }

    [Test]
    public void Map_Item_To_ItemGridModel_MapsCorrectly()
    {
        // Arrange
        var item = new Item()
        {
            Producer = new Producer()
            {
                Name = "P",
            },
            Brand = new Brand()
            {
                Name = "B",
            },
            Form = new Form()
            {
                Name = "F",
            },
            Atc = new Atc()
            {
                Name = "A",
                Value = "W",
            },
            SupplierNick = new Nick()
            {
                Name = "N",
            },
            MeasurementUnit = new MeasurementUnit()
            {
                Name = "MU",
            },
        };

        // Act
        var itemGridModel = _mapper.Map<ItemGridModel>(item);

        // Assert
        itemGridModel.Producer.ShouldBe("P");
        itemGridModel.Brand.ShouldBe("B");
        itemGridModel.Form.ShouldBe("F");
        itemGridModel.AtcName.ShouldBe("A");
        itemGridModel.AtcCode.ShouldBe("W");
        itemGridModel.SupplierNick.ShouldBe("N");
        itemGridModel.MeasurementUnit.ShouldBe("MU");
    }

    [Test]
    public void Map_Item_To_ItemClsfModel_MapsCorrectly()
    {
        // Arrange
        const string itemName = "Ibumetin";
        var entity = new Item()
        {
            ItemName = itemName,
        };

        // Act
        var clsf = _mapper.Map<ItemClsfModel>(entity);

        // Assert
        clsf.Name.ShouldBe(itemName);
    }

    [Test]
    public void Map_ItemModel_To_Item_MapsCorrectly()
    {
        // Arrange
        var itemModel = new ItemModel()
        {
            Producer = new ProducerClsfModel()
            {
                Id = 1,
            },
            Brand = new BrandClsfModel()
            {
                Id = 2,
            },
            Form = new FormClsfModel()
            {
                Id = 3,
            },
            Atc = new AtcClsfModel()
            {
                Id = 4,
            },
            SupplierNick = new NickClsfModel()
            {
                Id = 5,
            },
            MeasurementUnit = new MeasurementUnitClsfModel()
            {
                Id = 6,
            },
            Requestor = new RequestorClsfModel()
            {
                Id = 7,
            }
        };

        // Act
        var entity = _mapper.Map<Item>(itemModel);

        // Assert
        entity.ProducerId.ShouldBe(1);
        entity.BrandId.ShouldBe(2);
        entity.FormId.ShouldBe(3);
        entity.AtcId.ShouldBe(4);
        entity.SupplierNickId.ShouldBe(5);
        entity.MeasurementUnitId.ShouldBe(6);
        entity.RequestorId.ShouldBe(7);
    }
}
