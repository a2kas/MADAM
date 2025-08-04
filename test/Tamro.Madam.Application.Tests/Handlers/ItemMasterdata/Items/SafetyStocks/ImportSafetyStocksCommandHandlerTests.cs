using AutoMapper;
using Shouldly;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using System.Linq.Expressions;
using System.Reflection;
using Tamro.Madam.Application.Commands.ItemMasterdata.Items.SafetyStocks;
using Tamro.Madam.Application.Handlers.ItemMasterdata.Items.SafetyStocks;
using Tamro.Madam.Application.Infrastructure.Session;
using Tamro.Madam.Application.Profiles.ItemMasterdata.Items.SafetyStocks;
using Tamro.Madam.Application.Services.Items.SafetyStocks;
using Tamro.Madam.Models.General;
using Tamro.Madam.Models.ItemMasterdata.Items.SafetyStocks;
using Tamro.Madam.Repository.Common;
using Tamro.Madam.Repository.Entities.ItemMasterdata.Items.SafetyStocks;
using Tamro.Madam.Repository.Repositories.ItemMasterdata.Items.SafetyStocks;

namespace Tamro.Madam.Application.Tests.Handlers.ItemMasterdata.Items.SafetyStocks;

[TestFixture]
public class ImportSafetyStocksCommandHandlerTests
{
    private MockRepository _mockRepository;

    private Mock<ISafetyStockItemRepository> _safetyStockItemRepository;
    private Mock<ISafetyStockService> _safetyStockService;
    private Mock<IUserContext> _userContext;
    private Mock<ILogger<ImportSafetyStocksCommandHandler>> _logger;
    private IMapper _mapper;

    private ImportSafetyStocksCommandHandler _importSafetyStocksCommandHandler;

    [SetUp]
    public void SetUp()
    {
        _mockRepository = new MockRepository(MockBehavior.Loose);

        _safetyStockItemRepository = _mockRepository.Create<ISafetyStockItemRepository>();
        _safetyStockService = _mockRepository.Create<ISafetyStockService>();

        _userContext = _mockRepository.Create<IUserContext>();
        _logger = _mockRepository.Create<ILogger<ImportSafetyStocksCommandHandler>>();
        _mapper = new Mapper(new MapperConfiguration(cfg => cfg.AddMaps(Assembly.GetAssembly(typeof(SafetyStockProfile)))));

        _importSafetyStocksCommandHandler = new ImportSafetyStocksCommandHandler(_safetyStockItemRepository.Object, _safetyStockService.Object, _userContext.Object, _logger.Object, _mapper);
    }

    [Test]
    public async Task Handle_NewRestrictionForExistingItemIsAdded_HandlesCorrectly()
    {
        // Arrange
        var request = new ImportSafetyStocksCommand(
        [
            new SafetyStockGridDataModel()
            {
                ItemNo = "WD-40",
                SafetyStockPharmacyChainGroup = nameof(PharmacyGroup.Benu),
                CheckDays = 30,
                ItemName = "WD Superflex",
                Comment = "Yy",
            }
        ], BalticCountry.LV);
        _safetyStockItemRepository.Setup(x => x.GetMany(It.IsAny<Expression<Func<SafetyStockItem, bool>>>(), It.IsAny<List<IncludeOperation<SafetyStockItem>>?>(), false, It.IsAny<CancellationToken>()))
            .ReturnsAsync(new List<SafetyStockItemModel>()
            {
                new()
                {
                    Id = 1,
                    ItemNo = "WD-40",
                    SafetyStockConditions =
                    [
                        new SafetyStockConditionModel()
                        {
                            SafetyStockPharmacyChainGroup = PharmacyGroup.NonBenu,
                        },
                        new SafetyStockConditionModel()
                        {
                            SafetyStockPharmacyChainGroup = PharmacyGroup.All,
                        }
                    ],
                }
            });
        _userContext.Setup(x => x.DisplayName).Returns("Ted Bundy");


        // Act
        var result = await _importSafetyStocksCommandHandler.Handle(request, CancellationToken.None);

        // Assert
        _safetyStockItemRepository.Verify(x => x.UpsertGraph(It.Is<SafetyStockItemModel>(y => y.CheckDays == 30 && y.SafetyStockConditions.Count() == 2 &&
            y.SafetyStockConditions.Last().RestrictionLevel == SafetyStockRestrictionLevel.PharmacyChainGroup &&
            !y.SafetyStockConditions.Last().CanBuy &&
            y.SafetyStockConditions.Last().Comment == "Yy" &&
            y.SafetyStockConditions.Last().SafetyStockItemId == 1 &&
            y.SafetyStockConditions.Last().SafetyStockPharmacyChainId == default &&
            y.SafetyStockConditions.Last().SafetyStockPharmacyChainGroup == PharmacyGroup.Benu &&
            y.SafetyStockConditions.Last().User == "Ted Bundy"
            )), Times.Once);
        result.Succeeded.ShouldBeTrue();
        result.Data.Count().ShouldBe(1);
        result.Data.First().IsImported.ShouldBe(true);
        result.Data.First().Message.ShouldBe("Item restrictions extended");
    }

    [Test]
    public async Task Handle_ItemAlreadyHasSuchRestrictionSet_HandlesCorrectly()
    {
        // Arrange
        var request = new ImportSafetyStocksCommand(
        [
            new SafetyStockGridDataModel()
            {
                ItemNo = "WD-40",
                SafetyStockPharmacyChainGroup = nameof(PharmacyGroup.Benu),
                CheckDays = 30,
                ItemName = "WD Superflex",
                Comment = "Yy",
            }
        ], BalticCountry.LV);
        _safetyStockItemRepository.Setup(x => x.GetMany(It.IsAny<Expression<Func<SafetyStockItem, bool>>>(), It.IsAny<List<IncludeOperation<SafetyStockItem>>?>(), false, It.IsAny<CancellationToken>()))
            .ReturnsAsync(new List<SafetyStockItemModel>()
            {
                new()
                {
                    Id = 1,
                    ItemNo = "WD-40",
                    SafetyStockConditions =
                    [
                        new SafetyStockConditionModel()
                        {
                            SafetyStockPharmacyChainGroup = PharmacyGroup.Benu,
                        },
                    ],
                }
            });

        // Act
        var result = await _importSafetyStocksCommandHandler.Handle(request, CancellationToken.None);

        // Assert
        _safetyStockItemRepository.Verify(x => x.UpsertGraph(It.IsAny<SafetyStockItemModel>()), Times.Never);
        result.Succeeded.ShouldBeTrue();
        result.Data.Count().ShouldBe(1);
        result.Data.First().IsImported.ShouldBe(false);
        result.Data.First().Message.ShouldBe("Item already has such restriction set");
    }

    [Test]
    public async Task Handle_NewItemIsImported_HandlesCorrectly()
    {
        // Arrange
        var request = new ImportSafetyStocksCommand(
        [
            new SafetyStockGridDataModel()
            {
                ItemNo = "WD-40",
                PharmacyChainId = 42,
                CheckDays = 7,
                ItemName = "WD Superflex",
                Comment = "Yy",
                ItemGroup = "PSY",
            }
        ], BalticCountry.LV);
        _safetyStockItemRepository.Setup(x => x.GetMany(It.IsAny<Expression<Func<SafetyStockItem, bool>>>(), It.IsAny<List<IncludeOperation<SafetyStockItem>>?>(), false, It.IsAny<CancellationToken>()))
            .ReturnsAsync(new List<SafetyStockItemModel>());
        _userContext.Setup(x => x.DisplayName).Returns("Ted Bundy");
        _safetyStockService.Setup(x => x.GetSafetyStock("WD-40", 30, BalticCountry.LV)).ReturnsAsync(new SafetyStockModel() { RetailQuantity = 10, WholesaleQuantity = 50, });

        // Act
        var result = await _importSafetyStocksCommandHandler.Handle(request, CancellationToken.None);

        // Assert
        _safetyStockItemRepository.Verify(x => x.UpsertGraph(It.Is<SafetyStockItemModel>(y => y.CheckDays == 7 && y.SafetyStockConditions.Count() == 1 &&
            y.SafetyStockConditions.First().RestrictionLevel == SafetyStockRestrictionLevel.PharmacyChain &&
            !y.SafetyStockConditions.Last().CanBuy &&
            y.SafetyStockConditions.Last().Comment == "Yy" &&
            y.SafetyStockConditions.Last().SafetyStockItemId == default &&
            y.SafetyStockConditions.Last().SafetyStockPharmacyChainId == 42 &&
            y.SafetyStockConditions.Last().SafetyStockPharmacyChainGroup == default &&
            y.SafetyStockConditions.Last().User == "Ted Bundy" &&
            y.SafetyStock.RetailQuantity == 10 &&
            y.SafetyStock.WholesaleQuantity == 50
            )), Times.Once);
        result.Succeeded.ShouldBeTrue();
        result.Data.Count().ShouldBe(1);
        result.Data.First().IsImported.ShouldBe(true);
        result.Data.First().Message.ShouldBe("Item added to safety stock and restrictions applied");
    }

    [Test]
    public async Task Handle_OneOfItemsFails_OtherItemShouldBeProcessed()
    {
        // Arrange
        var request = new ImportSafetyStocksCommand(
        [
            new SafetyStockGridDataModel()
            {
                ItemNo = "WD-40",
                PharmacyChainId = 42,
                CheckDays = 7,
                ItemName = "WD Superflex",
                Comment = "Yy",
            },
            new SafetyStockGridDataModel()
            {
                ItemNo = "WD-41",
                PharmacyChainId = 43,
                CheckDays = 30,
                ItemName = "WSD",
            }
        ], BalticCountry.LV);
        _safetyStockItemRepository.Setup(x => x.GetMany(It.IsAny<Expression<Func<SafetyStockItem, bool>>>(), It.IsAny<List<IncludeOperation<SafetyStockItem>>?>(), false, It.IsAny<CancellationToken>()))
            .ReturnsAsync(new List<SafetyStockItemModel>());
        _safetyStockItemRepository.Setup(x => x.UpsertGraph(It.Is<SafetyStockItemModel>(y => y.ItemNo == "WD-40"))).ThrowsAsync(new ArgumentOutOfRangeException());

        // Act
        var result = await _importSafetyStocksCommandHandler.Handle(request, CancellationToken.None);

        // Assert
        _safetyStockItemRepository.Verify(x => x.UpsertGraph(It.IsAny<SafetyStockItemModel>()), Times.Exactly(2));
        result.Data.Count().ShouldBe(2);
        result.Data.First().IsImported.ShouldBeFalse();
        result.Data.First().Message.ShouldBe("Failed to import due to unknown reason");
    }
}