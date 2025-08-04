using AutoMapper;
using Moq;
using NUnit.Framework;
using Tamro.Madam.Application.Commands.ItemMasterdata.Items.SafetyStocks;
using Tamro.Madam.Application.Handlers.ItemMasterdata.Items.SafetyStocks;
using Tamro.Madam.Application.Services.Items.SafetyStocks.Factory;
using Tamro.Madam.Models.General;
using Tamro.Madam.Repository.Repositories.ItemMasterdata.Items.SafetyStocks;

namespace Tamro.Madam.Application.Tests.Handlers.ItemMasterdata.Items.SafetyStocks;

[TestFixture]
public class GetImportableSafetyStockItemsInfoCommandHandlerTests
{
    private MockRepository _mockRepository;

    private Mock<ISafetyStockWholesaleRepositoryFactory> _safetyStockWholesaleRepositoryFactory;
    private Mock<IMapper> _mapper;

    private GetImportableSafetyStockItemsInfoCommandHandler _getImportableSafetyStockItemsInfoCommandHandler;

    [SetUp]
    public void SetUp()
    {
        _mockRepository = new MockRepository(MockBehavior.Loose);

        _safetyStockWholesaleRepositoryFactory = _mockRepository.Create<ISafetyStockWholesaleRepositoryFactory>();
        _mapper = _mockRepository.Create<IMapper>();

        _getImportableSafetyStockItemsInfoCommandHandler = new GetImportableSafetyStockItemsInfoCommandHandler(_safetyStockWholesaleRepositoryFactory.Object, _mapper.Object);
    }

    [Test]
    public async Task Handle_GetsImportedItemInformation()
    {
        // Arrange
        const BalticCountry country = BalticCountry.LV;
        var itemNumbers = new string[] { "2033", "2044" };
        var request = new GetImportableSafetyStockItemsInfoCommand(itemNumbers, country);
        var safetyStockWholesaleRepository = new Mock<ISafetyStockWholesaleRepository>();
        _safetyStockWholesaleRepositoryFactory.Setup(x => x.Get(country)).Returns(safetyStockWholesaleRepository.Object);

        // Act
        await _getImportableSafetyStockItemsInfoCommandHandler.Handle(request, CancellationToken.None);

        // Assert
        safetyStockWholesaleRepository.Verify(x => x.GetSafetyStockImportedItems(itemNumbers), Times.Once);
    }
}