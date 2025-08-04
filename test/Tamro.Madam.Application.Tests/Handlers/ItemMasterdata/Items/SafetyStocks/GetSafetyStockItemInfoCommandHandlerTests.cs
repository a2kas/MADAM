using AutoMapper;
using Moq;
using NUnit.Framework;
using System.Reflection;
using Tamro.Madam.Application.Commands.ItemMasterdata.Items.SafetyStocks;
using Tamro.Madam.Application.Handlers.ItemMasterdata.Items.SafetyStocks;
using Tamro.Madam.Application.Profiles.ItemMasterdata.Items.SafetyStocks;
using Tamro.Madam.Application.Services.Items.SafetyStocks.Factory;
using Tamro.Madam.Models.General;
using Tamro.Madam.Repository.Entities.Wholesale;
using Tamro.Madam.Repository.Repositories.ItemMasterdata.Items.SafetyStocks;

namespace Tamro.Madam.Application.Tests.Handlers.ItemMasterdata.Items.SafetyStocks;

[TestFixture]
public class GetSafetyStockItemInfoCommandHandlerTests
{
    private MockRepository _mockRepository;

    private Mock<ISafetyStockWholesaleRepositoryFactory> _safetyStockWholesaleRepositoryFactory;
    private IMapper _mapper;

    private GetSafetyStockItemInfoCommandHandler _getSafetyStockItemInfoCommandHandler;

    [SetUp]
    public void SetUp()
    {
        _mockRepository = new MockRepository(MockBehavior.Loose);

        _safetyStockWholesaleRepositoryFactory = _mockRepository.Create<ISafetyStockWholesaleRepositoryFactory>();
        _mapper = new Mapper(new MapperConfiguration(cfg => cfg.AddMaps(Assembly.GetAssembly(typeof(SafetyStockProfile)))));

        _getSafetyStockItemInfoCommandHandler = new GetSafetyStockItemInfoCommandHandler(_safetyStockWholesaleRepositoryFactory.Object, _mapper);
    }

    [Test]
    public async Task Handle_GetsSafetyStockItemByItemNo_ForTheProvidedCountry()
    {
        // Arrange
        const string itemNo = "123";
        var request = new GetSafetyStockItemInfoCommand(itemNo, BalticCountry.LV);
        var safetyStockWholesaleRepository = new Mock<ISafetyStockWholesaleRepository>();
        _safetyStockWholesaleRepositoryFactory.Setup(x => x.Get(BalticCountry.LV)).Returns(safetyStockWholesaleRepository.Object);
        safetyStockWholesaleRepository.Setup(x => x.GetSafetyStockItemByItemNo(itemNo)).ReturnsAsync(new WholesaleSafetyStockItem()
        {
            ItemGroup = "NAR",
        });

        // Act
        await _getSafetyStockItemInfoCommandHandler.Handle(request, CancellationToken.None);

        // Assert
        safetyStockWholesaleRepository.Verify(x => x.GetSafetyStockItemByItemNo(itemNo));
        safetyStockWholesaleRepository.Verify(x => x.GetRetailQtyByItemNo(itemNo, 30));
    }
}