using AutoMapper;
using Shouldly;
using NUnit.Framework;
using System.Reflection;
using Tamro.Madam.Application.Profiles.Commerce.ItemAssortmentSalesChannels;
using Tamro.Madam.Models.Commerce.ItemAssortmentSalesChannels;
using Tamro.Madam.Repository.Entities.ItemMasterdata.Items;
using Tamro.Madam.Repository.Entities.ItemMasterdata.Items.Bindings;
using Tamro.Madam.Repository.Entities.ItemMasterdata.Items.Bindings.Assortment;

namespace Tamro.Madam.Application.Tests.Profiles.Commerce.ItemAssortmentSalesChannels;

[TestFixture]
public class ItemAssortmentBindingMapProfileTests
{
    private IMapper _mapper;

    [SetUp]
    public void SetUp()
    {
        _mapper = new Mapper(new MapperConfiguration(cfg => cfg.AddMaps(Assembly.GetAssembly(typeof(ItemAssortmentBindingMapProfile)))));
    }

    [Test]
    public void ItemAssortmentBindingMap_To_ItemAssortmentGridModel_MapsCorrectly()
    {
        // Arrange
        var source = new ItemAssortmentBindingMap()
        {
            ItemBinding = new ItemBinding()
            {
                LocalId = "1055",
                Item = new Item()
                {
                    ItemName = "Ibumetin",
                },
            },
        };

        // Act
        var destination = _mapper.Map<ItemAssortmentGridModel>(source);

        // Assert
        destination.ItemName.ShouldBe("Ibumetin");
        destination.ItemCode.ShouldBe("1055");
    }
}