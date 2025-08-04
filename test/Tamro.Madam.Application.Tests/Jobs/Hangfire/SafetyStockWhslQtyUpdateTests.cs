using Shouldly;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using System.Linq.Expressions;
using Tamro.Madam.Application.Jobs.Hangfire;
using Tamro.Madam.Application.Services.Items.Wholesale.Factories;
using Tamro.Madam.Models.General;
using Tamro.Madam.Models.ItemMasterdata.Items.SafetyStocks;
using Tamro.Madam.Models.ItemMasterdata.Items.Wholesale;
using Tamro.Madam.Repository.Common;
using Tamro.Madam.Repository.Entities.ItemMasterdata.Items.SafetyStocks;
using Tamro.Madam.Repository.Repositories.ItemMasterdata.Items.SafetyStocks;
using Tamro.Madam.Repository.Repositories.ItemMasterdata.Items.Wholesale;
using TamroUtilities.Hangfire.Models;

namespace Tamro.Madam.Application.Tests.Jobs.Hangfire;

[TestFixture]
public class SafetyStockWhslQtyUpdateTests
{
    private MockRepository _mockRepository;

    private Mock<IWholesaleItemAvailabilityRepositoryFactory> _wholesaleItemAvailabilityRepositoryFactory;
    private Mock<IWholesaleItemAvailabilityRepository> _eeWholesaleItemAvailabilityRepository;
    private Mock<IWholesaleItemAvailabilityRepository> _ltWholesaleItemAvailabilityRepository;
    private Mock<IWholesaleItemAvailabilityRepository> _lvWholesaleItemAvailabilityRepository;
    private Mock<ISafetyStockItemRepository> _safetyStockItemRepository;
    private Mock<ISafetyStockRepository> _safetyStockRepository;
    private Mock<ILogger<SafetyStockWhslQtyUpdate>> _logger;

    private SafetyStockWhslQtyUpdate _safetyStockWhslQtyUpdate;

    [SetUp]
    public void SetUp()
    {
        _mockRepository = new MockRepository(MockBehavior.Loose);

        _wholesaleItemAvailabilityRepositoryFactory = _mockRepository.Create<IWholesaleItemAvailabilityRepositoryFactory>();
        _eeWholesaleItemAvailabilityRepository = _mockRepository.Create<IWholesaleItemAvailabilityRepository>();
        _ltWholesaleItemAvailabilityRepository = _mockRepository.Create<IWholesaleItemAvailabilityRepository>();
        _lvWholesaleItemAvailabilityRepository = _mockRepository.Create<IWholesaleItemAvailabilityRepository>();
        _wholesaleItemAvailabilityRepositoryFactory.Setup(x => x.Get(BalticCountry.EE)).Returns(_eeWholesaleItemAvailabilityRepository.Object);
        _wholesaleItemAvailabilityRepositoryFactory.Setup(x => x.Get(BalticCountry.LT)).Returns(_ltWholesaleItemAvailabilityRepository.Object);
        _wholesaleItemAvailabilityRepositoryFactory.Setup(x => x.Get(BalticCountry.LV)).Returns(_lvWholesaleItemAvailabilityRepository.Object);
        _safetyStockItemRepository = _mockRepository.Create<ISafetyStockItemRepository>();
        _safetyStockRepository = _mockRepository.Create<ISafetyStockRepository>();
        _logger = _mockRepository.Create<ILogger<SafetyStockWhslQtyUpdate>>();

        _safetyStockWhslQtyUpdate = new SafetyStockWhslQtyUpdate(
           _wholesaleItemAvailabilityRepositoryFactory.Object,
           _safetyStockItemRepository.Object,
           _safetyStockRepository.Object,
           _logger.Object);
    }

    [Test]
    public void SafetyStockWhslQtyUpdate_IsHangfireJob()
    {
        // Assert
        _safetyStockWhslQtyUpdate.ShouldBeAssignableTo<HangfireJobBase>();
    }

    [Test]
    public async Task SafetyStockWhslQtyUpdate_SynchronizesQuantities_Correctly()
    {
        // Arrange
        var lvQties = new List<ItemAvailabilityModel>()
        {
            new ItemAvailabilityModel() 
            {
                ItemNo = "Lv1",
                AvailableQuantity = 25,
                Country = BalticCountry.LV,
            },
            new ItemAvailabilityModel() 
            {
                ItemNo = "Xx2",
                AvailableQuantity = 5,
                Country = BalticCountry.LV,
            },
        };
        _lvWholesaleItemAvailabilityRepository.Setup(x => x.GetAll()).ReturnsAsync(lvQties);
        var eeQties = new List<ItemAvailabilityModel>()
        {
            new ItemAvailabilityModel()
            {
                ItemNo = "Xx2",
                AvailableQuantity = 10,
                Country = BalticCountry.EE,
            }
        };
        _eeWholesaleItemAvailabilityRepository.Setup(x => x.GetAll()).ReturnsAsync(eeQties);
        var safetyStocks = new List<SafetyStockItemModel>()
        {
            new SafetyStockItemModel()
            {
                Id = 1,
                ItemNo = "Lv1",
                Country = BalticCountry.LV,
                SafetyStock = new SafetyStockModel()
                {
                    Id = 1,
                    WholesaleQuantity = 0,
                    RetailQuantity = 5,
                },
            },
            new SafetyStockItemModel()
            {
                Id = 2,
                ItemNo = "Xx2",
                Country = BalticCountry.EE,
            }
        };
        _safetyStockItemRepository.Setup(x => x.GetMany(It.IsAny<Expression<Func<SafetyStockItem, bool>>>(), It.IsAny<List<IncludeOperation<SafetyStockItem>>?>(), false, It.IsAny<CancellationToken>())).ReturnsAsync(safetyStocks);

        // Act
        await _safetyStockWhslQtyUpdate.JobToRun(null, null);

        // Assert
        _safetyStockRepository.Verify(x => x.UpsertBulkRange(It.Is<IEnumerable<SafetyStockModel>>(y => y.Count() == 2 &&
           y.First().WholesaleQuantity == 25 && y.First().RetailQuantity == 5 &&
           y.Last().WholesaleQuantity == 10 && y.Last().RetailQuantity == default)), Times.Once);
    }

    [Test]
    public async Task SafetyStockWhslQtyUpdate_ItemNotAvailable_SetsWhslQtyTo0()
    {
        // Arrange
        var eeQties = new List<ItemAvailabilityModel>();
        var safetyStocks = new List<SafetyStockItemModel>()
        {
            new SafetyStockItemModel()
            {
                Id = 1,
                ItemNo = "Lv1",
                Country = BalticCountry.LV,
                SafetyStock = new SafetyStockModel()
                {
                    Id = 1,
                    WholesaleQuantity = 10,
                    RetailQuantity = 5,
                },
            },
        };
        _eeWholesaleItemAvailabilityRepository.Setup(x => x.GetAll()).ReturnsAsync(eeQties);
        _safetyStockItemRepository.Setup(x => x.GetMany(It.IsAny<Expression<Func<SafetyStockItem, bool>>>(), It.IsAny<List<IncludeOperation<SafetyStockItem>>?>(), false, It.IsAny<CancellationToken>())).ReturnsAsync(safetyStocks);

        // Act
        await _safetyStockWhslQtyUpdate.JobToRun(null, null);

        // Assert
        _safetyStockRepository.Verify(x => x.UpsertBulkRange(It.Is<IEnumerable<SafetyStockModel>>(y => y.First().Id == 1 && y.First().WholesaleQuantity == 0 && y.First().RetailQuantity == 5)), Times.Once);
    }
}