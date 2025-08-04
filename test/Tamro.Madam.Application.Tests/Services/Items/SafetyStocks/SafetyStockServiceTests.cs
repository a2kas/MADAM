using Shouldly;
using Moq;
using NUnit.Framework;
using Tamro.Madam.Application.Services.Items.SafetyStocks;
using Tamro.Madam.Application.Services.Items.SafetyStocks.Factory;
using Tamro.Madam.Application.Services.Items.Wholesale.Factories;
using Tamro.Madam.Models.General;
using Tamro.Madam.Models.ItemMasterdata.Items.Wholesale;
using Tamro.Madam.Repository.Entities.Wholesale;
using Tamro.Madam.Repository.Repositories.ItemMasterdata.Items.SafetyStocks;
using Tamro.Madam.Repository.Repositories.ItemMasterdata.Items.Wholesale;

namespace Tamro.Madam.Application.Tests.Services.Items.SafetyStocks;

[TestFixture]
public class SafetyStockServiceTests
{
    private MockRepository _mockRepository;

    private Mock<IWholesaleItemAvailabilityRepositoryFactory> _wholesaleItemAvailabilityRepositoryFactory;
    private Mock<ISafetyStockWholesaleRepositoryFactory> _safetyStockWholesaleRepositoryFactory;

    private SafetyStockService _safetyStockService;

    [SetUp]
    public void SetUp()
    {
        _mockRepository = new MockRepository(MockBehavior.Loose);

        _wholesaleItemAvailabilityRepositoryFactory = _mockRepository.Create<IWholesaleItemAvailabilityRepositoryFactory>();
        _safetyStockWholesaleRepositoryFactory = _mockRepository.Create<ISafetyStockWholesaleRepositoryFactory>();

        _safetyStockService = new SafetyStockService(_wholesaleItemAvailabilityRepositoryFactory.Object, _safetyStockWholesaleRepositoryFactory.Object);
    }

    [Test]
    public async Task GetSafetyStock_Gets()
    {
        // Arrange
        var country = BalticCountry.LV;
        var itemNo2 = "WD-40";
        var whslItemAvailabilityRepository = new Mock<IWholesaleItemAvailabilityRepository>();
        _wholesaleItemAvailabilityRepositoryFactory.Setup(x => x.Get(country)).Returns(whslItemAvailabilityRepository.Object);
        var safetyStockWholesaleRepository = new Mock<ISafetyStockWholesaleRepository>();
        _safetyStockWholesaleRepositoryFactory.Setup(x => x.Get(country)).Returns(safetyStockWholesaleRepository.Object);
        whslItemAvailabilityRepository.Setup(x => x.Get(It.IsAny<List<string>>())).ReturnsAsync(new List<ItemAvailabilityModel>()
        {
            new()
            {
                AvailableQuantity = 5,
            }
        });
        safetyStockWholesaleRepository.Setup(x => x.GetRetailQtyByItemNo(itemNo2, 7)).ReturnsAsync(new WholesaleSafetyStockItemRetailQty()
        {
            RtlTransQty = 10,
        });

        // Act
        var result = await _safetyStockService.GetSafetyStock(itemNo2, 7, country);

        // Assert
        result.WholesaleQuantity.ShouldBe(5);
        result.RetailQuantity.ShouldBe(10);
    }
}