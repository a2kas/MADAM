using Moq;
using NUnit.Framework;
using Shouldly;
using Tamro.Madam.Application.Services.Sales.HeldOrders;
using Tamro.Madam.Application.Services.Sales.HeldOrders.Factories;
using Tamro.Madam.Models.General;

namespace Tamro.Madam.Application.Tests.Services.Sales.HeldOrders.Factories;

[TestFixture]
public class E1HeldOrderEmailGeneratorFactoryTests
{
    private Mock<IServiceProvider> _serviceProvider;
    private E1HeldOrderEmailGeneratorFactory _e1HeldOrderEmailGeneratorFactory;

    [SetUp]
    public void SetUp()
    {
        _serviceProvider = new Mock<IServiceProvider>();

        _e1HeldOrderEmailGeneratorFactory = new E1HeldOrderEmailGeneratorFactory(_serviceProvider.Object);
    }

    [TestCase(BalticCountry.LV, typeof(LvHeldOrderEmailGenerator))]
    public void Get_Gets_Correct_EmailGenerator(BalticCountry country, Type serviceType)
    {
        // Act
        _e1HeldOrderEmailGeneratorFactory.Get(country);

        // Assert
        _serviceProvider.Verify(x => x.GetService(serviceType), Times.Once);
    }

    [Test]
    public void Get_UnsupportedCountry_ThrowsNotSupportedException()
    {
        // Arrange
        var country = (BalticCountry)999;

        // Act
        Action act = () => _e1HeldOrderEmailGeneratorFactory.Get(country);

        // Assert
        act.ShouldThrow<NotSupportedException>();
    }
}
