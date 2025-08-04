using AutoMapper;
using Shouldly;
using NUnit.Framework;
using System.Reflection;
using Tamro.Madam.Application.Profiles.Commerce.ItemAssortmentSalesChannels;
using Tamro.Madam.Models.Commerce.ItemAssortmentSalesChannels;
using Tamro.Madam.Repository.Entities.Commerce.Assortment;
using Tamro.Madam.Repository.Entities.ItemMasterdata.Items.Bindings.Assortment;

namespace Tamro.Madam.Application.Tests.Profiles.Commerce.ItemAssortmentSalesChannels;

[TestFixture]
public class ItemAssortmentSalesChannelProfileTests
{
    private IMapper _mapper;

    [SetUp]
    public void SetUp()
    {
        _mapper = new Mapper(new MapperConfiguration(cfg => cfg.AddMaps(Assembly.GetAssembly(typeof(ItemAssortmentSalesChannelProfile)))));
    }

    [Test]
    public void ItemAssortmentSalesChannel_To_ItemAssortmentSalesChannelGridModel_MapsCorrectly()
    {
        // Arrange
        var itemAssortmentSalesChannel = new ItemAssortmentSalesChannel()
        {
            ItemAssortmentBindingMaps = new List<ItemAssortmentBindingMap>() { new(), new(), },
        };

        // Act
        var result = _mapper.Map<ItemAssortmentSalesChannelGridModel>(itemAssortmentSalesChannel);

        // Assert
        result.ItemsCount.ShouldBe(2);
    }
}