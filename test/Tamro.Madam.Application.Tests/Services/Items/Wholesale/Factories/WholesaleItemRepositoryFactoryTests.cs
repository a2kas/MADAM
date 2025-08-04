using Shouldly;
using Moq;
using NUnit.Framework;
using Tamro.Madam.Application.Services.Items.Wholesale.Factories;
using Tamro.Madam.Models.General;
using Tamro.Madam.Repository.Repositories.ItemMasterdata.Items.Wholesale.Ee;
using Tamro.Madam.Repository.Repositories.ItemMasterdata.Items.Wholesale.Lt;
using Tamro.Madam.Repository.Repositories.ItemMasterdata.Items.Wholesale.Lv;

namespace Tamro.Madam.Application.Tests.Services.Items.Wholesale.Factories;

[TestFixture]
public class WholesaleItemRepositoryFactoryTests
{
    private MockRepository _mockRepository;

    private Mock<IServiceProvider> _serviceProvider;

    private WholesaleItemRepositoryFactory _wholesaleItemRepositoryFactory;

    [SetUp]
    public void SetUp()
    {
        _mockRepository = new MockRepository(MockBehavior.Loose);

        _serviceProvider = _mockRepository.Create<IServiceProvider>();

        _wholesaleItemRepositoryFactory = new WholesaleItemRepositoryFactory(_serviceProvider.Object);
    }

    [TestCase(BalticCountry.LV, typeof(LvWholesaleItemRepository))]
    [TestCase(BalticCountry.LT, typeof(LtWholesaleItemRepository))]
    [TestCase(BalticCountry.EE, typeof(EeWholesaleItemRepository))]
    public void Get_Gets_Correct_Repository(BalticCountry country, Type serviceType)
    {
        // Act
        _wholesaleItemRepositoryFactory.Get(country);

        // Assert
        _serviceProvider.Verify(x => x.GetService(serviceType), Times.Once);
    }

    [Test]
    public void Get_UnsupportedCountry_ThrowsNotSupportedException()
    {
        // Act
        var act = () => _wholesaleItemRepositoryFactory.Get((BalticCountry)999);

        // Assert
        act.ShouldThrow<NotSupportedException>();
    }
}