using Shouldly;
using Moq;
using NUnit.Framework;
using Tamro.Madam.Application.Services.Sales.CanceledOrderLines;
using Tamro.Madam.Application.Services.Sales.CanceledOrderLines.Factories;
using Tamro.Madam.Models.General;

namespace Tamro.Madam.Application.Tests.Services.Sales.CanceledOrderLines.Factories;

[TestFixture]
public class CanceledOrderLinesEmailGeneratorFactoryTests
{
    private MockRepository _mockRepository;
    private Mock<IServiceProvider> _serviceProvider;
    private CanceledOrderLinesEmailGeneratorFactory _canceledOrderLinesEmailGeneratorFactory;

    [SetUp]
    public void SetUp()
    {
        _mockRepository = new MockRepository(MockBehavior.Loose);
        _serviceProvider = _mockRepository.Create<IServiceProvider>();
        _canceledOrderLinesEmailGeneratorFactory = new CanceledOrderLinesEmailGeneratorFactory(_serviceProvider.Object);
    }

    [TestCase(BalticCountry.LV, typeof(LvCanceledOrderLinesEmailGenerator))]
    [TestCase(BalticCountry.LT, typeof(LtCanceledOrderLinesEmailGenerator))]
    [TestCase(BalticCountry.EE, typeof(EeCanceledOrderLinesEmailGenerator))]
    public void Get_Gets_Correct_EmailGenerator(BalticCountry country, Type serviceType)
    {
        // Act
        _canceledOrderLinesEmailGeneratorFactory.Get(country);

        // Assert
        _serviceProvider.Verify(x => x.GetService(serviceType), Times.Once);
    }

    [Test]
    public void Get_UnsupportedCountry_ThrowsNotSupportedException()
    {
        // Arrange
        var country = (BalticCountry)999;

        // Act
        Action act = () => _canceledOrderLinesEmailGeneratorFactory.Get(country);

        // Assert
        act.ShouldThrow<NotSupportedException>();
    }
}
