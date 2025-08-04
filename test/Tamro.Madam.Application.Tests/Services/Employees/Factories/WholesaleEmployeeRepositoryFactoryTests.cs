using Moq;
using NUnit.Framework;
using Shouldly;
using Tamro.Madam.Application.Services.Employees.Factories;
using Tamro.Madam.Models.General;
using Tamro.Madam.Repository.Repositories.Customers.Wholesale.Lv;

namespace Tamro.Madam.Application.Tests.Services.Employees.Factories;

[TestFixture]
public class WholesaleEmployeeRepositoryFactoryTests
{
    private Mock<IServiceProvider> _serviceProvider;

    private WholesaleEmployeeRepositoryFactory _wholesaleEmployeeRepositoryFactory;

    [SetUp]
    public void SetUp()
    {
        _serviceProvider = new Mock<IServiceProvider>();

        _wholesaleEmployeeRepositoryFactory = new WholesaleEmployeeRepositoryFactory(_serviceProvider.Object);
    }

    [TestCase(BalticCountry.LV, typeof(LvWholesaleEmployeeRepository))]
    public void Get_Gets_Correct_Repository(BalticCountry country, Type serviceType)
    {
        // Act
        _wholesaleEmployeeRepositoryFactory.Get(country);

        // Assert
        _serviceProvider.Verify(x => x.GetService(serviceType), Times.Once);
    }

    [Test]
    public void Get_UnsupportedCountry_ThrowsNotSupportedException()
    {
        // Arrange
        var country = (BalticCountry)999;

        // Act
        Action act = () => _wholesaleEmployeeRepositoryFactory.Get(country);

        // Assert
        act.ShouldThrow<NotSupportedException>();
    }
}
