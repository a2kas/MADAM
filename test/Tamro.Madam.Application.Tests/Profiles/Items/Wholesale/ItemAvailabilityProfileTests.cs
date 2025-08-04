using AutoMapper;
using Shouldly;
using NUnit.Framework;
using System.Reflection;
using Tamro.Madam.Application.Profiles.ItemMasterdata.Items.Wholesale;
using Tamro.Madam.Models.General;
using Tamro.Madam.Models.ItemMasterdata.Items.Wholesale;
using Tamro.Madam.Repository.Entities.Wholesale.Ee;
using Tamro.Madam.Repository.Entities.Wholesale.Lt;
using Tamro.Madam.Repository.Entities.Wholesale.Lv;

namespace Tamro.Madam.Application.Tests.Profiles.Items.Wholesale;

[TestFixture]
public class ItemAvailabilityProfileTests
{
    private IMapper _mapper;

    [SetUp]
    public void SetUp()
    {
        _mapper = new Mapper(new MapperConfiguration(cfg => cfg.AddMaps(Assembly.GetAssembly(typeof(ItemAvailabilityProfile)))));
    }

    [Test]
    public void LtItemAvailability_To_ItemAvailabilityModel_MapsCorrectly()
    {
        // Arrange
        var src = new LtItemAvailability();

        // Act
        var dest = _mapper.Map<ItemAvailabilityModel>(src);

        // Assert
        dest.Country.ShouldBe(BalticCountry.LT);
    }

    [Test]
    public void LvItemAvailability_To_ItemAvailabilityModel_MapsCorrectly()
    {
        // Arrange
        var src = new LvItemAvailability();

        // Act
        var dest = _mapper.Map<ItemAvailabilityModel>(src);

        // Assert
        dest.Country.ShouldBe(BalticCountry.LV);
    }

    [Test]
    public void EeItemAvailability_To_ItemAvailabilityModel_MapsCorrectly()
    {
        // Arrange
        var src = new EeItemAvailability();

        // Act
        var dest = _mapper.Map<ItemAvailabilityModel>(src);

        // Assert
        dest.Country.ShouldBe(BalticCountry.EE);
    }
}