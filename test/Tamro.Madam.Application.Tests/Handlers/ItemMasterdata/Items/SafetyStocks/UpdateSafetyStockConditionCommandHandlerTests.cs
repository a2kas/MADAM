using AutoMapper;
using Shouldly;
using Moq;
using NUnit.Framework;
using System.Linq.Expressions;
using System.Reflection;
using Tamro.Madam.Application.Commands.ItemMasterdata.Items.SafetyStocks;
using Tamro.Madam.Application.Handlers.ItemMasterdata.Items.SafetyStocks;
using Tamro.Madam.Application.Profiles.ItemMasterdata.Items.SafetyStocks;
using Tamro.Madam.Application.Services.Items.SafetyStocks.Factory;
using Tamro.Madam.Application.Validation;
using Tamro.Madam.Models.General;
using Tamro.Madam.Models.ItemMasterdata.Items.SafetyStocks;
using Tamro.Madam.Models.ItemMasterdata.Items.SafetyStocks;
using Tamro.Madam.Repository.Common;
using Tamro.Madam.Repository.Entities.ItemMasterdata.Items.SafetyStocks;
using Tamro.Madam.Repository.Entities.Wholesale;
using Tamro.Madam.Repository.Repositories.ItemMasterdata.Items.SafetyStocks;

namespace Tamro.Madam.Application.Tests.Handlers.ItemMasterdata.Items.SafetyStocks;

[TestFixture]
public class UpdateSafetyStockConditionCommandHandlerTests
{
    private MockRepository _mockRepository;

    private Mock<ISafetyStockConditionRepository> _safetyStockConditionRepository;
    private Mock<ISafetyStockItemRepository> _safetyStockItemRepository;
    private Mock<ISafetyStockWholesaleRepositoryFactory> _safetyStockWholesaleRepositoryFactory;
    private Mock<IHandlerValidator> _validator;
    private IMapper _mapper;

    private UpdateSafetyStockConditionCommandHandler _updateSafetyStockConditionCommandHandler;

    [SetUp]
    public void SetUp()
    {
        _mockRepository = new MockRepository(MockBehavior.Loose);

        _safetyStockConditionRepository = _mockRepository.Create<ISafetyStockConditionRepository>();
        _safetyStockItemRepository = _mockRepository.Create<ISafetyStockItemRepository>();
        _safetyStockWholesaleRepositoryFactory = _mockRepository.Create<ISafetyStockWholesaleRepositoryFactory>();
        _validator = _mockRepository.Create<IHandlerValidator>();
        _mapper = new Mapper(new MapperConfiguration(cfg => cfg.AddMaps(Assembly.GetAssembly(typeof(SafetyStockProfile)))));

        _updateSafetyStockConditionCommandHandler = new UpdateSafetyStockConditionCommandHandler(_safetyStockConditionRepository.Object, _safetyStockItemRepository.Object,
            _safetyStockWholesaleRepositoryFactory.Object, _validator.Object, _mapper);
    }

    [Test]
    public async Task Handle_Validates()
    {
        // Arrange
        var ssum = new SafetyStockConditionUpsertModel();
        var cmd = new UpdateSafetyStockConditionCommand(ssum);

        // Act
        await _updateSafetyStockConditionCommandHandler.Handle(cmd, CancellationToken.None);

        // Assert
        _validator.Verify(x => x.Validate(ssum), Times.Once);
    }

    [Test]
    public async Task Handle_SafetyStockConditionExists_UpdatesExistingSafetyStockCondition()
    {
        // Arrange
        var cmd = new UpdateSafetyStockConditionCommand(new SafetyStockConditionUpsertModel()
        {
            Id = 6,
            CheckDays = 12,
        });
        _safetyStockConditionRepository.Setup(x => x.Get(6)).ReturnsAsync(new SafetyStockConditionModel()
        {
            SafetyStockItemId = 8,
            CheckDays = 12,
        });

        // Act
        await _updateSafetyStockConditionCommandHandler.Handle(cmd, CancellationToken.None);

        // Assert
        _safetyStockConditionRepository.Verify(x => x.Upsert(It.Is<SafetyStockConditionModel>(y => y.SafetyStockItemId == 8)), Times.Once);
    }

    [Test]
    public async Task Handle_SafetyStockConditionDoesNotExists_ReturnsError()
    {
        // Arrange
        var cmd = new UpdateSafetyStockConditionCommand(new SafetyStockConditionUpsertModel()
        {
            Id = 0,
            CheckDays = 12,
        });

        // Act
        var result = await _updateSafetyStockConditionCommandHandler.Handle(cmd, CancellationToken.None);

        // Assert
        result.Succeeded.ShouldBeFalse();
    }

    [TestCase(6, 6, 0)]
    [TestCase(6, 7, 1)]
    public async Task Handle_SafetyStockItemAndRetailQtyShouldBeUpdatedIfCheckDaysHasChanged(int existingCheckDays, int updatedCheckDays, int expectedRetailQuantityUpdateAmount)
    {
        // Arrange
        var cmd = new UpdateSafetyStockConditionCommand(new SafetyStockConditionUpsertModel()
        {
            Id = 6,
            CheckDays = updatedCheckDays,
        });
        _safetyStockConditionRepository.Setup(x => x.Get(6)).ReturnsAsync(new SafetyStockConditionModel()
        {
            SafetyStockItemId = 1001,
            CheckDays = existingCheckDays,
        });
        _safetyStockItemRepository.Setup(x => x.Get(It.IsAny<Expression<Func<SafetyStockItem, bool>>>(), It.IsAny<List<IncludeOperation<SafetyStockItem>>>(), true, It.IsAny<CancellationToken>()))
            .ReturnsAsync(new SafetyStockItemModel()
            {
                Country = BalticCountry.LT,
                ItemNo = "WD-40",
                SafetyStock = new SafetyStockModel()
                {
                    Id = 25,
                    RetailQuantity = 101,
                },
            });
        var safetyStockWholesaleRepository = new Mock<ISafetyStockWholesaleRepository>();
        _safetyStockWholesaleRepositoryFactory.Setup(x => x.Get(BalticCountry.LT)).Returns(safetyStockWholesaleRepository.Object);
        safetyStockWholesaleRepository.Setup(x => x.GetRetailQtyByItemNo("WD-40", updatedCheckDays)).ReturnsAsync(new WholesaleSafetyStockItemRetailQty()
        {
            RtlTransQty = 51,
        });

        // Act
        await _updateSafetyStockConditionCommandHandler.Handle(cmd, CancellationToken.None);

        // Assert
        _safetyStockItemRepository.Verify(x => x.UpsertGraph(It.Is<SafetyStockItemModel>(y => y.CheckDays == updatedCheckDays && y.SafetyStock.RetailQuantity == 51)), Times.Exactly(expectedRetailQuantityUpdateAmount));
    }
}