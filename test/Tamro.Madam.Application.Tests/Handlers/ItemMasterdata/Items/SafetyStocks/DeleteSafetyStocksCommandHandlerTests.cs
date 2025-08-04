using Shouldly;
using Moq;
using NUnit.Framework;
using System.Linq.Expressions;
using Tamro.Madam.Application.Commands.ItemMasterdata.Items.SafetyStocks;
using Tamro.Madam.Application.Handlers.ItemMasterdata.Items.SafetyStocks;
using Tamro.Madam.Repository.Entities.ItemMasterdata.Items.SafetyStocks;
using Tamro.Madam.Repository.Repositories.ItemMasterdata.Items.SafetyStocks;

namespace Tamro.Madam.Application.Tests.Handlers.ItemMasterdata.Items.SafetyStocks;

[TestFixture]
public class DeleteSafetyStocksCommandHandlerTests
{
    private MockRepository _mockRepository;

    private Mock<ISafetyStockItemRepository> _safetyStockItemRepository;

    private DeleteSafetyStocksCommandHandler _deleteSafetyStocksCommandHandler;

    [SetUp]
    public void SetUp()
    {
        _mockRepository = new MockRepository(MockBehavior.Loose);

        _safetyStockItemRepository = _mockRepository.Create<ISafetyStockItemRepository>();

        _deleteSafetyStocksCommandHandler = new DeleteSafetyStocksCommandHandler(_safetyStockItemRepository.Object);
    }

    [Test]
    public async Task Handle_DeletesSafetyStockItems_AndReturnsSuccess()
    {
        // Arrange
        var cmd = new DeleteSafetyStocksCommand(new List<int>() { 4, 6, });

        // Act
        var result = await _deleteSafetyStocksCommandHandler.Handle(cmd, CancellationToken.None);

        // Assert
        result.Succeeded.ShouldBeTrue();
        _safetyStockItemRepository.Verify(x => x.DeleteMany(It.IsAny<Expression<Func<SafetyStockItem, bool>>>(), CancellationToken.None), Times.Once);
    }
}
