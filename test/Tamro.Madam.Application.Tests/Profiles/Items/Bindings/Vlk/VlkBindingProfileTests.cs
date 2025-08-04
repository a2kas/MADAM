using System.Reflection;
using AutoMapper;
using Shouldly;
using NUnit.Framework;
using Tamro.Madam.Application.Profiles.ItemMasterdata.Items.Bindings.Vlk;
using Tamro.Madam.Models.ItemMasterdata.Items.Bindings;
using Tamro.Madam.Models.ItemMasterdata.Items.Bindings.Vlk;
using Tamro.Madam.Repository.Entities.ItemMasterdata.Items;
using Tamro.Madam.Repository.Entities.ItemMasterdata.Items.Bindings;
using Tamro.Madam.Repository.Entities.ItemMasterdata.Items.Bindings.Vlk;

namespace Tamro.Madam.Application.Tests.Profiles.Items.Bindings.Vlk;

[TestFixture]
public class VlkBindingProfileTests
{
    private IMapper _mapper;

    [SetUp]
    public void SetUp()
    {
        _mapper = new Mapper(new MapperConfiguration(cfg => cfg.AddMaps(Assembly.GetAssembly(typeof(VlkBindingProfile)))));
    }

    [Test]
    public void VlkBinding_To_VlkBindingGridModel_MapsCorrectly()
    {
        // Arrange
        var vlkBinding = new VlkBinding()
        {
            ItemBinding = new ItemBinding()
            {
                LocalId = "WD-40",
                Item = new Item()
                {
                    ItemName = "TestItem"
                },
            },
        };

        // Act
        var vlkBindingGridModel = _mapper.Map<VlkBindingGridModel>(vlkBinding);

        // Assert
        vlkBindingGridModel.ItemNo2.ShouldBe("WD-40");
        vlkBindingGridModel.ItemName.ShouldBe("TestItem");
    }

    [Test]
    public void VlkBindingGridModel_To_VlkBindingDetailsModel_MapsCorrectly()
    {
        // Arrange
        var vlkBindingGridModel = new VlkBindingGridModel()
        {
            ItemNo2 = "WD-40",
            ItemName = "TestItem",
            ItemBindingId = 2,
        };

        // Act
        var vlkBindingDetailsModel = _mapper.Map<VlkBindingDetailsModel>(vlkBindingGridModel);

        // Assert
        vlkBindingDetailsModel.ItemBinding.Id.ShouldBe(2);
        vlkBindingDetailsModel.ItemBinding.ItemNo2.ShouldBe("WD-40");
        vlkBindingDetailsModel.ItemBinding.Name.ShouldBe("TestItem");
    }

    [Test]
    public void VlkBindingDetailsModel_To_VlkBinding_MapsCorrectly()
    {
        // Arrange
        var vlkBindingDetailsModel = new VlkBindingDetailsModel()
        {
            ItemBinding = new ItemBindingClsfModel()
            {
                Id = 1,
            },
        };

        // Act
        var vlkBinding = _mapper.Map<VlkBinding>(vlkBindingDetailsModel);

        // Assert
        vlkBinding.ItemBindingId.ShouldBe(1);
        vlkBinding.ItemBinding.ShouldBeNull();
    }
}