using System.Reflection;
using AutoFixture;
using AutoMapper;
using NUnit.Framework;
using Shouldly;
using Tamro.Madam.Application.Profiles.Administration.Configuration;
using Tamro.Madam.Models.Administration.Configuration.Ubl;
using Tamro.Madam.Repository.Entities.Administration.Configuration.Ubl;

namespace Tamro.Madam.Application.Tests.Profiles.Administration.Configuration;
public class UblApiKeyProfileTests
{
    private Fixture _fixture;

    private IMapper _mapper;

    [SetUp]
    public void SetUp()
    {
        _fixture = new Fixture();

        _mapper = new Mapper(new MapperConfiguration(cfg => cfg.AddMaps(Assembly.GetAssembly(typeof(UblApiKeyProfile)))));
    }

    [Test]
    public void UblApiKey_To_UblApiKeyModel_MasksApiKey()
    {
        // Arrange
        var ublApiKey = _fixture.Create<UblApiKey>();

        // Act
        var result = _mapper.Map<UblApiKeyModel>(ublApiKey);

        // Assert
        result.ApiKey.ShouldBe("********-****-****-****-********");
    }

    [Test]
    public void UblApiKeyModel_To_UblApiKeyEditModel_MapsCorrectly()
    {
        // Arrange
        var ublKeyModel = _fixture.Create<UblApiKeyModel>();

        // Act
        var result = _mapper.Map<UblApiKeyEditModel>(ublKeyModel);

        // Assert
        result.ApiKey.ShouldBeNull();
        result.Customer.AddressNumber.ShouldBe(ublKeyModel.E1SoldTo);
        result.Customer.Name.ShouldBe(ublKeyModel.CustomerName);
    }

    [Test]
    public void UblApiKeyEditModel_To_UblApiKeyModel_MapsCorrectly()
    {
        // Arrange
        var ublKeyEditModel = _fixture.Create<UblApiKeyEditModel>();

        // Act
        var result = _mapper.Map<UblApiKeyModel>(ublKeyEditModel);

        // Assert
        result.ApiKey.ShouldBe(result.ApiKey);
        result.E1SoldTo.ShouldBe(ublKeyEditModel.Customer.AddressNumber);
        result.CustomerName.ShouldBe(ublKeyEditModel.Customer.Name);
    }
}
