using AutoFixture;
using AutoMapper;
using Shouldly;
using Moq;
using NUnit.Framework;
using System.Linq.Expressions;
using System.Reflection;
using Tamro.Madam.Application.Commands.ItemMasterdata.Items.SafetyStocks;
using Tamro.Madam.Application.Handlers.ItemMasterdata.Items.SafetyStocks;
using Tamro.Madam.Application.Profiles.ItemMasterdata.Items.Wholesale;
using Tamro.Madam.Application.Services.Items.SafetyStocks;
using Tamro.Madam.Models.General;
using Tamro.Madam.Models.ItemMasterdata.Items.SafetyStocks;
using Tamro.Madam.Repository.Common;
using Tamro.Madam.Repository.Entities.ItemMasterdata.Items.SafetyStocks;
using Tamro.Madam.Repository.Repositories.ItemMasterdata.Items.SafetyStocks;

namespace Tamro.Madam.Application.Tests.Handlers.ItemMasterdata.Items.SafetyStocks;

[TestFixture]
public class UpsertSafetyStockItemCommandHandlerTests
{
    private MockRepository _mockRepository;

    private Mock<ISafetyStockItemRepository> _safetyStockItemRepository;
    private Mock<ISafetyStockService> _safetyStockService;
    private IMapper _mapper;

    private UpsertSafetyStockItemCommandHandler _upsertSafetyStockItemCommandHandler;
    private Fixture _fixture;

    [SetUp]
    public void SetUp()
    {
        _mockRepository = new MockRepository(MockBehavior.Loose);

        _safetyStockItemRepository = _mockRepository.Create<ISafetyStockItemRepository>();
        _safetyStockService = _mockRepository.Create<ISafetyStockService>();
        _mapper = new Mapper(new MapperConfiguration(cfg => cfg.AddMaps(Assembly.GetAssembly(typeof(WholesaleItemProfile)))));

        _upsertSafetyStockItemCommandHandler = new UpsertSafetyStockItemCommandHandler(_safetyStockItemRepository.Object, _safetyStockService.Object, _mapper);
        _fixture = new Fixture();
    }

    [Test]
    public async Task Handle_SafetyStockItemExists_AndHasConditionForAllPharmacyGroups_CommandFails()
    {
        // Arrange
        var request = _fixture.Create<UpsertSafetyStockItemCommand>();
        var existingSafetyStockItem = new SafetyStockItemModel()
        {
            SafetyStockConditions =
            [
                new()
                {
                    RestrictionLevel = SafetyStockRestrictionLevel.PharmacyChainGroup,
                    SafetyStockPharmacyChainGroup = PharmacyGroup.All,
                },
            ],
        };
        _safetyStockItemRepository.Setup(x => x.GetMany(It.IsAny<Expression<Func<SafetyStockItem, bool>>>(),
            It.Is<List<IncludeOperation<SafetyStockItem>>>(y => y.Count() == 2), false, It.IsAny<CancellationToken>())).ReturnsAsync(new List<SafetyStockItemModel>() { existingSafetyStockItem, });

        // Act
        var result = await _upsertSafetyStockItemCommandHandler.Handle(request, CancellationToken.None);

        // Assert
        result.Succeeded.ShouldBeFalse();
        result.ErrorMessage.ShouldBe("Safety stock condition record for item already exists");
    }

    [Test]
    public async Task Handle_SafetyStockItemExists_AndPharmacyChainsAreDuplicated_CommandFails()
    {
        // Arrange
        var request = _fixture.Create<UpsertSafetyStockItemCommand>();
        request.Model.RestrictionLevel = SafetyStockRestrictionLevel.PharmacyChain;
        request.Model.PharmacyChains =
        [
            new()
            {
                Id = 1,
                DisplayName = "Benu",
            },
            new()
            {
                Id = 2,
                DisplayName = "Medikona",
            },
            new()
            {
                Id = 3,
                DisplayName = "Camelia"
            }
        ];
        var existingSafetyStockItem = new SafetyStockItemModel()
        {
            SafetyStockConditions =
            [
                new()
                {
                    RestrictionLevel = SafetyStockRestrictionLevel.PharmacyChain,
                    SafetyStockPharmacyChainId = 1,
                },
                new()
                {
                    RestrictionLevel = SafetyStockRestrictionLevel.PharmacyChain,
                    SafetyStockPharmacyChainId = 3,
                },
            ],
        };
        _safetyStockItemRepository.Setup(x => x.GetMany(It.IsAny<Expression<Func<SafetyStockItem, bool>>>(),
            It.Is<List<IncludeOperation<SafetyStockItem>>>(y => y.Count() == 2), false, It.IsAny<CancellationToken>())).ReturnsAsync(new List<SafetyStockItemModel>() { existingSafetyStockItem, });

        // Act
        var result = await _upsertSafetyStockItemCommandHandler.Handle(request, CancellationToken.None);

        // Assert
        result.Succeeded.ShouldBeFalse();
        result.ErrorMessage.ShouldBe($"Item '{request.Model.Item.DisplayName}' already has safety stock condition set for the following pharmacies: Benu, Camelia");
    }

    [Test]
    public async Task Handle_SafetyStockItemExists_AndPharmacyGroupsAreDuplicated_CommandFails()
    {
        // Arrange
        var request = _fixture.Create<UpsertSafetyStockItemCommand>();
        request.Model.RestrictionLevel = SafetyStockRestrictionLevel.PharmacyChainGroup;
        request.Model.PharmacyGroups =
        [
            PharmacyGroup.Benu,
        ];
        var existingSafetyStockItem = new SafetyStockItemModel()
        {
            SafetyStockConditions =
            [
                new()
                {
                    RestrictionLevel = SafetyStockRestrictionLevel.PharmacyChainGroup,
                    SafetyStockPharmacyChainGroup = PharmacyGroup.Benu,
                },
                new()
                {
                    RestrictionLevel = SafetyStockRestrictionLevel.PharmacyChainGroup,
                    SafetyStockPharmacyChainGroup = PharmacyGroup.NonBenu,
                },
            ],
        };
        _safetyStockItemRepository.Setup(x => x.GetMany(It.IsAny<Expression<Func<SafetyStockItem, bool>>>(),
            It.Is<List<IncludeOperation<SafetyStockItem>>>(y => y.Count() == 2), false, It.IsAny<CancellationToken>())).ReturnsAsync(new List<SafetyStockItemModel>() { existingSafetyStockItem, });

        // Act
        var result = await _upsertSafetyStockItemCommandHandler.Handle(request, CancellationToken.None);

        // Assert
        result.Succeeded.ShouldBeFalse();
        result.ErrorMessage.ShouldBe($"Item '{request.Model.Item.DisplayName}' already has safety stock condition set for the following pharmacy groups: Benu");
    }

    [Test]
    public async Task Handle_SafetyStockItemDoesNotExist_PharmacyChainRestrictionsAreCreated_UpsertsGraphCorrectly()
    {
        // Arrange
        var request = _fixture.Create<UpsertSafetyStockItemCommand>();
        request.Model.RestrictionLevel = SafetyStockRestrictionLevel.PharmacyChain;
        request.Model.PharmacyChains =
        [
            new()
            {
                Id = 1,
                DisplayName = "Benu",
            },
            new()
            {
                Id = 2,
                DisplayName = "Medikona",
            },
        ];
        request.Model.ItemInfo.ItemGroup = "PSY";
        _safetyStockService.Setup(x => x.GetSafetyStock(It.IsAny<string>(), 30, It.IsAny<BalticCountry>())).ReturnsAsync(new SafetyStockModel() { RetailQuantity = 10, WholesaleQuantity = 50, });

        // Act
        var result = await _upsertSafetyStockItemCommandHandler.Handle(request, CancellationToken.None);

        // Assert
        result.Succeeded.ShouldBeTrue();
        _safetyStockItemRepository.Verify(x => x.UpsertGraph(It.Is<SafetyStockItemModel>(y =>
            y.Substance == request.Model.ItemInfo.Substance && y.CheckDays == 30 &&
            y.SafetyStock.RetailQuantity == 10 &&
            y.SafetyStock.WholesaleQuantity == 50 &&
            y.SafetyStockConditions.Count() == 2 &&
            y.SafetyStockConditions.All(ssc =>
                !ssc.CanBuy &&
                ssc.CheckDays == 30 &&
                ssc.Comment == request.Model.Comment &&
                ssc.RestrictionLevel == SafetyStockRestrictionLevel.PharmacyChain &&
                ssc.SafetyStockPharmacyChainId != default)
        )), Times.Once);
    }

    [Test]
    public async Task Handle_SafetyStockItemExists_PharmacyChainRestrictionsAreCreated_UpsertsGraphCorrectly()
    {
        // Arrange
        var request = _fixture.Create<UpsertSafetyStockItemCommand>();
        request.Model.RestrictionLevel = SafetyStockRestrictionLevel.PharmacyChain;
        request.Model.PharmacyChains =
        [
            new()
            {
                Id = 2,
                DisplayName = "Medikona",
            },
        ];
        request.Model.ItemInfo.ItemGroup = "dasdadf";
        var existingSafetyStockItem = new SafetyStockItemModel()
        {
            SafetyStockConditions =
            [
                new()
                {
                    RestrictionLevel = SafetyStockRestrictionLevel.PharmacyChain,
                    SafetyStockPharmacyChainId = 1,
                },
            ],
        };
        _safetyStockItemRepository.Setup(x => x.GetMany(It.IsAny<Expression<Func<SafetyStockItem, bool>>>(),
            It.Is<List<IncludeOperation<SafetyStockItem>>>(y => y.Count() == 2), false, It.IsAny<CancellationToken>())).ReturnsAsync(new List<SafetyStockItemModel>() { existingSafetyStockItem, });

        // Act
        var result = await _upsertSafetyStockItemCommandHandler.Handle(request, CancellationToken.None);

        // Assert
        result.Succeeded.ShouldBeTrue();
        _safetyStockItemRepository.Verify(x => x.UpsertGraph(It.Is<SafetyStockItemModel>(y =>
            y.SafetyStockConditions.Count() == 2 &&
            !y.SafetyStockConditions.Last().CanBuy &&
            y.SafetyStockConditions.Last().CheckDays == 10 &&
            y.SafetyStockConditions.Last().SafetyStockPharmacyChainId != null
        )), Times.Once);
    }

    [Test]
    public async Task Handle_SafetyStockItemDoesNotExist_PharmacyChainGroupRestrictionsAreCreated_UpsertsGraphCorrectly()
    {
        // Arrange
        var request = _fixture.Create<UpsertSafetyStockItemCommand>();
        request.Model.RestrictionLevel = SafetyStockRestrictionLevel.PharmacyChainGroup;
        request.Model.PharmacyGroups =
        [
            PharmacyGroup.Benu,
            PharmacyGroup.NonBenu,
        ];
        request.Model.ItemInfo.ItemGroup = "PSY";

        // Act
        var result = await _upsertSafetyStockItemCommandHandler.Handle(request, CancellationToken.None);

        // Assert
        result.Succeeded.ShouldBeTrue();
        _safetyStockItemRepository.Verify(x => x.UpsertGraph(It.Is<SafetyStockItemModel>(y =>
            y.Substance == request.Model.ItemInfo.Substance &&
            y.SafetyStockConditions.Count() == 2 &&
            y.SafetyStockConditions.All(ssc =>
                !ssc.CanBuy &&
                ssc.CheckDays == 30 &&
                ssc.Comment == request.Model.Comment &&
                ssc.RestrictionLevel == SafetyStockRestrictionLevel.PharmacyChainGroup &&
                ssc.SafetyStockPharmacyChainGroup != default &&
                ssc.SafetyStockPharmacyChainId == default)
        )), Times.Once);
    }

    [Test]
    public async Task Handle_SafetyStockItemExists_PharmacyChainGroupRestrictionsAreCreated_UpsertsGraphCorrectly()
    {
        // Arrange
        var request = _fixture.Create<UpsertSafetyStockItemCommand>();
        request.Model.RestrictionLevel = SafetyStockRestrictionLevel.PharmacyChainGroup;
        request.Model.PharmacyGroups =
        [
            PharmacyGroup.Benu,
        ];
        request.Model.ItemInfo.ItemGroup = "dasdadf";
        var existingSafetyStockItem = new SafetyStockItemModel()
        {
            SafetyStockConditions =
            [
                new()
                {
                    RestrictionLevel = SafetyStockRestrictionLevel.PharmacyChainGroup,
                    SafetyStockPharmacyChainGroup = PharmacyGroup.NonBenu,
                },
            ],
        };
        _safetyStockItemRepository.Setup(x => x.GetMany(It.IsAny<Expression<Func<SafetyStockItem, bool>>>(),
            It.Is<List<IncludeOperation<SafetyStockItem>>>(y => y.Count() == 2), false, It.IsAny<CancellationToken>())).ReturnsAsync(new List<SafetyStockItemModel>() { existingSafetyStockItem, });

        // Act
        var result = await _upsertSafetyStockItemCommandHandler.Handle(request, CancellationToken.None);

        // Assert
        result.Succeeded.ShouldBeTrue();
        _safetyStockItemRepository.Verify(x => x.UpsertGraph(It.Is<SafetyStockItemModel>(y =>
            y.SafetyStockConditions.Count() == 2 &&
            !y.SafetyStockConditions.Last().CanBuy &&
            y.SafetyStockConditions.Last().CheckDays == 10 &&
            y.SafetyStockConditions.Last().SafetyStockPharmacyChainId == null &&
            y.SafetyStockConditions.Last().SafetyStockPharmacyChainGroup == PharmacyGroup.Benu
        )), Times.Once);
    }
}