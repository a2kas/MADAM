using Shouldly;
using Moq;
using NUnit.Framework;
using Tamro.Madam.Application.Services.Items.SafetyStocks.Factory;
using Tamro.Madam.Models.General;
using Tamro.Madam.Repository.Repositories.ItemMasterdata.Items.SafetyStocks.Lt;

namespace Tamro.Madam.Application.Tests.Services.Items.SafetyStocks.Factory;

[TestFixture]
public class SafetyStockWholesaleRepositoryFactoryTests
{
    private MockRepository _mockRepository;

    private Mock<IServiceProvider> _serviceProvider;

    private SafetyStockWholesaleRepositoryFactory _safetyStockWholesaleRepositoryFactory;

    [SetUp]
    public void SetUp()
    {
        _mockRepository = new MockRepository(MockBehavior.Loose);

        _serviceProvider = _mockRepository.Create<IServiceProvider>();

        _safetyStockWholesaleRepositoryFactory = new SafetyStockWholesaleRepositoryFactory(_serviceProvider.Object);
    }

    [TestCase(BalticCountry.LT, typeof(LtSafetyStockWholesaleRepository))]
    public void Get_Gets_Correct_Repository(BalticCountry country, Type serviceType)
    {
        // Act
        _safetyStockWholesaleRepositoryFactory.Get(country);

        // Assert
        _serviceProvider.Verify(x => x.GetService(serviceType), Times.Once);
    }

    [TestCase(BalticCountry.LV)]
    [TestCase(BalticCountry.EE)]
    public void Get_UnsupportedCountry_ThrowsNotSupportedException(BalticCountry country)
    {
        // Act
        var act = () => _safetyStockWholesaleRepositoryFactory.Get(country);

        // Assert
        act.ShouldThrow<NotSupportedException>();
    }
}