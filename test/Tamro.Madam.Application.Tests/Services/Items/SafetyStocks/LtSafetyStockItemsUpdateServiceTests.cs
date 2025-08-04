using AutoMapper;
using EFCore.BulkExtensions;
using Moq;
using NUnit.Framework;
using System.Linq.Expressions;
using System.Reflection;
using Tamro.Madam.Application.Profiles.ItemMasterdata.Items.SafetyStocks;
using Tamro.Madam.Application.Services.Items.SafetyStocks;
using Tamro.Madam.Application.Services.Items.SafetyStocks.Factory;
using Tamro.Madam.Models.General;
using Tamro.Madam.Models.ItemMasterdata.Items.SafetyStocks;
using Tamro.Madam.Repository.Common;
using Tamro.Madam.Repository.Entities.ItemMasterdata.Items.SafetyStocks;
using Tamro.Madam.Repository.Entities.Wholesale;
using Tamro.Madam.Repository.Repositories.ItemMasterdata.Items.SafetyStocks;

namespace Tamro.Madam.Application.Tests.Services.Items.SafetyStocks;

[TestFixture]
public class LtSafetyStockItemsUpdateServiceTests
{
    private MockRepository _mockRepository;

    private Mock<ISafetyStockWholesaleRepositoryFactory> _safetyStockWholesaleRepositoryFactory;
    private Mock<ISafetyStockItemRepository> _safetyStockItemRepository;
    private IMapper _mapper;

    private LtSafetyStockItemsUpdateService _ltSafetyStockItemsUpdateService;

    [SetUp]
    public void SetUp()
    {
        _mockRepository = new MockRepository(MockBehavior.Loose);

        _safetyStockWholesaleRepositoryFactory = _mockRepository.Create<ISafetyStockWholesaleRepositoryFactory>();
        _safetyStockItemRepository = _mockRepository.Create<ISafetyStockItemRepository>();
        _mapper = new Mapper(new MapperConfiguration(cfg => cfg.AddMaps(Assembly.GetAssembly(typeof(SafetyStockProfile)))));

        _ltSafetyStockItemsUpdateService = new LtSafetyStockItemsUpdateService(_safetyStockWholesaleRepositoryFactory.Object, _safetyStockItemRepository.Object, _mapper);
    }

    [Test]
    public async Task Update_ItemsPreviouslyNotExistingInSafetyStock_AreCreatedCorrectly()
    {
        //Arrange
        var ltSafetyStockWholesaleRepository = new Mock<ISafetyStockWholesaleRepository>();
        _safetyStockWholesaleRepositoryFactory.Setup(x => x.Get(BalticCountry.LT)).Returns(ltSafetyStockWholesaleRepository.Object);
        ltSafetyStockWholesaleRepository.Setup(x => x.GetSafetyStockItems()).ReturnsAsync(new List<WholesaleSafetyStockItem>()
        {
            new()
            {
                ItemNo = "W111",
                ItemGroup = "PSY",
            },
        });
        ltSafetyStockWholesaleRepository.Setup(x => x.GetSafetyStockExistingItems()).ReturnsAsync(new List<WholesaleSafetyStockItem>()
        {
            new()
            {
                ItemNo = "W112",
                ItemGroup = "Narc",
            }
        });
        _safetyStockItemRepository.Setup(x => x.GetMany(It.IsAny<Expression<Func<SafetyStockItem, bool>>>(),
            It.IsAny<List<IncludeOperation<SafetyStockItem>>>(), false, It.IsAny<CancellationToken>())).ReturnsAsync(new List<SafetyStockItemModel>());

        // Act
        await _ltSafetyStockItemsUpdateService.Update();

        // Assert
        _safetyStockItemRepository.Verify(x => x.UpsertGraph(It.Is<SafetyStockItemModel>(y => y.ItemNo == "W111" && y.CheckDays == 30 && y.SafetyStockConditions.Count() == 1 &&
            y.SafetyStockConditions.All(ssc => ssc.RestrictionLevel == SafetyStockRestrictionLevel.PharmacyChainGroup && ssc.SafetyStockPharmacyChainGroup == PharmacyGroup.All && ssc.CanBuy))), Times.Exactly(1));
        _safetyStockItemRepository.Verify(x => x.UpsertGraph(It.Is<SafetyStockItemModel>(y => y.ItemNo == "W112" && y.CheckDays == 10 && y.SafetyStockConditions.Count() == 1 &&
            y.SafetyStockConditions.All(ssc => ssc.RestrictionLevel == SafetyStockRestrictionLevel.PharmacyChainGroup && ssc.SafetyStockPharmacyChainGroup == PharmacyGroup.All && ssc.CanBuy))), Times.Exactly(1));
    }

    [Test]
    public async Task Update_ItemsAlreadyExistingInSafetyStock_AreUpdatedCorrectly()
    {
        // Arrange
        var ltSafetyStockWholesaleRepository = new Mock<ISafetyStockWholesaleRepository>();
        _safetyStockWholesaleRepositoryFactory.Setup(x => x.Get(BalticCountry.LT)).Returns(ltSafetyStockWholesaleRepository.Object);
        ltSafetyStockWholesaleRepository.Setup(x => x.GetSafetyStockItems()).ReturnsAsync(new List<WholesaleSafetyStockItem>()
        {
            new()
            {
                ItemNo = "W111",
                ItemGroup = "PSY",
            },
            new()
            {
                ItemNo = "W112",
                ItemGroup = "Narc",
            }
        });
        _safetyStockItemRepository.Setup(x => x.GetMany(It.IsAny<Expression<Func<SafetyStockItem, bool>>>(),
            It.IsAny<List<IncludeOperation<SafetyStockItem>>>(), false, It.IsAny<CancellationToken>())).ReturnsAsync(new List<SafetyStockItemModel>()
            {
                new()
                {
                    Id = 2,
                    ItemNo = "W111",
                    SafetyStock = new()
                    {
                        RetailQuantity = 1,
                    },
                },
                new()
                {
                    Id = 3,
                    ItemNo = "W112",
                    SafetyStock = new()
                    {
                        RetailQuantity = 0,
                    },
                },
            });

        // Act 
        await _ltSafetyStockItemsUpdateService.Update();

        // Assert
        _safetyStockItemRepository.Verify(x => x.UpsertBulkRange(It.Is<List<SafetyStockItemModel>>(
            y => y.Count == 2 &&
            y.Any(t => t.ItemNo == "W111") &&
            y.Any(t => t.ItemNo == "W112")),
            It.Is<BulkConfig>(y => y.IncludeGraph)),
            Times.Exactly(2));
    }

    [Test]
    public async Task Update_ItemsAlreadyExistingInSafetyStock_And_WholesaleItemGroupHasChanged_ChangesCheckDays()
    {
        // Arrange
        var ltSafetyStockWholesaleRepository = new Mock<ISafetyStockWholesaleRepository>();
        _safetyStockWholesaleRepositoryFactory.Setup(x => x.Get(BalticCountry.LT)).Returns(ltSafetyStockWholesaleRepository.Object);
        ltSafetyStockWholesaleRepository.Setup(x => x.GetSafetyStockItems()).ReturnsAsync(new List<WholesaleSafetyStockItem>()
        {
            new()
            {
                ItemNo = "W111",
                ItemGroup = "PSY",
            },
        });
        _safetyStockItemRepository.Setup(x => x.GetMany(It.IsAny<Expression<Func<SafetyStockItem, bool>>>(),
            It.IsAny<List<IncludeOperation<SafetyStockItem>>>(), false, It.IsAny<CancellationToken>())).ReturnsAsync(new List<SafetyStockItemModel>()
            {
                new()
                {
                    Id = 25,
                    ItemNo = "W111",
                    ItemGroup = "TEST",
                    SafetyStock = new()
                    {
                        RetailQuantity = 1,
                    },
                    SafetyStockConditions =
                    [
                        new()
                        {
                            CheckDays = 2,
                        }
                    ]
                },
            });

        // Act 
        await _ltSafetyStockItemsUpdateService.Update();

        // Assert
        _safetyStockItemRepository.Verify(x => x.UpsertBulkRange(It.Is<List<SafetyStockItemModel>>(
            y => y.Count == 1 &&
            y.Any(t => t.ItemNo == "W111") &&
            y[0].CheckDays == 30),
            It.Is<BulkConfig>(y => y.IncludeGraph)),
            Times.Exactly(2));
    }

    [Test]
    public async Task Update_ItemsAlreadyExistingInSafetyStock_And_WholesaleItemGroupHasNotChanged_DoesNotChangeCheckDays()
    {
        // Arrange
        var ltSafetyStockWholesaleRepository = new Mock<ISafetyStockWholesaleRepository>();
        _safetyStockWholesaleRepositoryFactory.Setup(x => x.Get(BalticCountry.LT)).Returns(ltSafetyStockWholesaleRepository.Object);
        ltSafetyStockWholesaleRepository.Setup(x => x.GetSafetyStockItems()).ReturnsAsync(new List<WholesaleSafetyStockItem>()
        {
            new()
            {
                ItemNo = "W111",
                ItemGroup = "PSY",
            },
        });
        _safetyStockItemRepository.Setup(x => x.GetMany(It.IsAny<Expression<Func<SafetyStockItem, bool>>>(),
            It.IsAny<List<IncludeOperation<SafetyStockItem>>>(), false, It.IsAny<CancellationToken>())).ReturnsAsync(new List<SafetyStockItemModel>()
            {
                new()
                {
                    Id = 4,
                    ItemNo = "W111",
                    ItemGroup = "PSY",
                    CheckDays = 2,
                    SafetyStock = new()
                    {
                        RetailQuantity = 1,
                    },
                },
            });

        // Act 
        await _ltSafetyStockItemsUpdateService.Update();

        // Assert
        _safetyStockItemRepository.Verify(x => x.UpsertBulkRange(It.Is<List<SafetyStockItemModel>>(
            y => y.Count == 1 &&
            y.Any(t => t.ItemNo == "W111") &&
            y[0].CheckDays == 2),
            It.Is<BulkConfig>(y => y.IncludeGraph)),
            Times.Exactly(2));
    }


    [Test]
    public async Task Update_ItemsAlreadyExistingInSafetyStock_UpdatesRetailQty()
    {
        // Arrange
        var ltSafetyStockWholesaleRepository = new Mock<ISafetyStockWholesaleRepository>();
        _safetyStockWholesaleRepositoryFactory.Setup(x => x.Get(BalticCountry.LT)).Returns(ltSafetyStockWholesaleRepository.Object);
        ltSafetyStockWholesaleRepository.Setup(x => x.GetSafetyStockItems()).ReturnsAsync(new List<WholesaleSafetyStockItem>()
        {
            new()
            {
                ItemNo = "W111",
                ItemGroup = "PSY",
            },
        });
        _safetyStockItemRepository.Setup(x => x.GetMany(It.IsAny<Expression<Func<SafetyStockItem, bool>>>(),
            It.IsAny<List<IncludeOperation<SafetyStockItem>>>(), false, It.IsAny<CancellationToken>())).ReturnsAsync(new List<SafetyStockItemModel>()
            {
                new()
                {
                    Id = 4,
                    ItemNo = "W111",
                    ItemGroup = "PSY",
                    SafetyStock = new()
                    {
                        RetailQuantity = 1,
                    },
                    CheckDays = 2,
                },
            });
        ltSafetyStockWholesaleRepository.Setup(x => x.GetRetailQty()).ReturnsAsync(new List<WholesaleSafetyStockItemRetailQty>()
        {
            new WholesaleSafetyStockItemRetailQty()
            {
                SafetyStockItemId = 4,
                RtlTransQty = 15,
            }
        });

        // Act 
        await _ltSafetyStockItemsUpdateService.Update();

        // Assert
        _safetyStockItemRepository.Verify(x => x.UpsertBulkRange(It.Is<List<SafetyStockItemModel>>(
            y => y.Count == 1 &&
            y.Any(t => t.ItemNo == "W111") &&
            y[0].SafetyStock.RetailQuantity == 15 &&
            y[0].CheckDays == 2),
            It.Is<BulkConfig>(y => y.IncludeGraph)),
            Times.Exactly(2));
    }

    [Test]
    public async Task Cleanup_RemovesItemsWithoutConditions()
    {
        // Act
        await _ltSafetyStockItemsUpdateService.Cleanup();

        // Assert
        _safetyStockItemRepository.Verify(x => x.DeleteMany(It.Is<Expression<Func<SafetyStockItem, bool>>>(y => y.ToString().Contains("AndAlso Not(x.SafetyStockConditions.Any())")), CancellationToken.None), Times.Once);
    }
}