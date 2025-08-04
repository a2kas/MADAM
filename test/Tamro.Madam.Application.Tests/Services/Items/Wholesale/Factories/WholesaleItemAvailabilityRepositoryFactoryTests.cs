using AutoMapper;
using Shouldly;
using Moq;
using NUnit.Framework;
using Tamro.Madam.Application.Services.Items.Wholesale.Factories;
using Tamro.Madam.Models.General;
using Tamro.Madam.Repository.Context.Wholesale;
using Tamro.Madam.Repository.Repositories.ItemMasterdata.Items.Wholesale.Ee;
using Tamro.Madam.Repository.Repositories.ItemMasterdata.Items.Wholesale.Lt;
using Tamro.Madam.Repository.Repositories.ItemMasterdata.Items.Wholesale.Lv;

namespace Tamro.Madam.Application.Tests.Services.Items.Wholesale.Factories;

[TestFixture]
public class WholesaleItemAvailabilityRepositoryFactoryTests
{
    private MockRepository _mockRepository;

    private Mock<IServiceProvider> _serviceProvider;

    private WholesaleItemAvailabilityRepositoryFactory _wholesaleItemAvailabilityRepositoryFactory;

    [SetUp]
    public void SetUp()
    {
        _mockRepository = new MockRepository(MockBehavior.Loose);

        _serviceProvider = _mockRepository.Create<IServiceProvider>();

        _wholesaleItemAvailabilityRepositoryFactory = new WholesaleItemAvailabilityRepositoryFactory(_serviceProvider.Object);
    }

    [Test]
    public void Get_ShouldReturn_LtWholesaleItemAvailabilityRepository_WhenCountryIsLT()
    {
        // Arrange
        var whRawLtDatabaseContextMock = new Mock<IWhRawLtDatabaseContext>();
        var mapperMock = new Mock<IMapper>();
        var ltRepositoryMock = new Mock<LtWholesaleItemAvailabilityRepository>(whRawLtDatabaseContextMock.Object, mapperMock.Object);

        _serviceProvider
            .Setup(sp => sp.GetService(typeof(LtWholesaleItemAvailabilityRepository)))
            .Returns(ltRepositoryMock.Object);

        // Act
        var result = _wholesaleItemAvailabilityRepositoryFactory.Get(BalticCountry.LT);

        // Assert
        result.ShouldBe(ltRepositoryMock.Object);
    }


    [Test]
    public void Get_ShouldReturn_EeWholesaleItemAvailabilityRepository_WhenCountryIsEE()
    {
        // Arrange
        var whRawEeDatabaseContextMock = new Mock<IWhRawEeDatabaseContext>();
        var mapperMock = new Mock<IMapper>();
        var eeRepositoryMock = new Mock<EeWholesaleItemAvailabilityRepository>(whRawEeDatabaseContextMock.Object, mapperMock.Object);

        _serviceProvider
            .Setup(sp => sp.GetService(typeof(EeWholesaleItemAvailabilityRepository)))
            .Returns(eeRepositoryMock.Object);

        // Act
        var result = _wholesaleItemAvailabilityRepositoryFactory.Get(BalticCountry.EE);

        // Assert
        result.ShouldBe(eeRepositoryMock.Object);
    }

    [Test]
    public void Get_ShouldReturn_LvWholesaleItemAvailabilityRepository_WhenCountryIsLV()
    {
        // Arrange
        var whRawLvDatabaseContextMock = new Mock<IWhRawLvDatabaseContext>();
        var mapperMock = new Mock<IMapper>();
        var lvRepositoryMock = new Mock<LvWholesaleItemAvailabilityRepository>(whRawLvDatabaseContextMock.Object, mapperMock.Object);

        _serviceProvider
            .Setup(sp => sp.GetService(typeof(LvWholesaleItemAvailabilityRepository)))
            .Returns(lvRepositoryMock.Object);

        // Act
        var result = _wholesaleItemAvailabilityRepositoryFactory.Get(BalticCountry.LV);

        // Assert
        result.ShouldBe(lvRepositoryMock.Object);
    }

    [Test]
    public void Get_ShouldThrowNotSupportedException_WhenCountryIsUnsupported()
    {
        // Arrange
        var unsupportedCountry = (BalticCountry)999;

        // Act
        Action act = () => _wholesaleItemAvailabilityRepositoryFactory.Get(unsupportedCountry);

        // Assert
        act.ShouldThrow<NotSupportedException>();
    }

}