using Moq;
using NUnit.Framework;
using Shouldly;
using Tamro.Madam.Application.Services.Customers.Factories;
using Tamro.Madam.Models.General;
using Tamro.Madam.Repository.Repositories.Customers.Wholesale.Ee;
using Tamro.Madam.Repository.Repositories.Customers.Wholesale.Lt;
using Tamro.Madam.Repository.Repositories.Customers.Wholesale.Lv;

namespace Tamro.Madam.Application.Tests.Services.Customers.Factories;

[TestFixture]
public class WholesaleCustomerRepositoryFactoryTests
{
    private MockRepository _mockRepository;

    private Mock<IServiceProvider> _serviceProvider;

    private WholesaleCustomerRepositoryFactory _wholesaleCustomerRepositoryFactory;

    [SetUp]
    public void SetUp()
    {
        _mockRepository = new MockRepository(MockBehavior.Loose);

        _serviceProvider = _mockRepository.Create<IServiceProvider>();

        _wholesaleCustomerRepositoryFactory = new WholesaleCustomerRepositoryFactory(_serviceProvider.Object);
    }

    [TestCase(BalticCountry.LV, typeof(LvWholesaleCustomerRepository))]
    [TestCase(BalticCountry.LT, typeof(LtWholesaleCustomerRepository))]
    [TestCase(BalticCountry.EE, typeof(EeWholesaleCustomerRepository))]
    public void Get_Gets_Correct_Repository(BalticCountry country, Type serviceType)
    {
        // Act
        _wholesaleCustomerRepositoryFactory.Get(country);

        // Assert
        _serviceProvider.Verify(x => x.GetService(serviceType), Times.Once);
    }

    [Test]
    public void Get_UnsupportedCountry_ThrowsNotSupportedException()
    {
        // Arrange
        var country = (BalticCountry)999;

        // Act
        Action act = () => _wholesaleCustomerRepositoryFactory.Get(country);

        // Assert
        act.ShouldThrow<NotSupportedException>();
    }
}