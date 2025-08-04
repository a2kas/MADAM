using Shouldly;
using Moq;
using NUnit.Framework;
using Tamro.Madam.Application.Commands.ItemMasterdata.Items.SafetyStocks;
using Tamro.Madam.Application.Handlers.ItemMasterdata.Items.SafetyStocks;
using Tamro.Madam.Repository.Repositories.ItemMasterdata.Items.SafetyStocks;

namespace Tamro.Madam.Application.Tests.Handlers.ItemMasterdata.Items.SafetyStocks;

[TestFixture]
public class DeleteSateyStockConditionsCommandHandlerTests
{
    private MockRepository _mockRepository;

    private Mock<ISafetyStockConditionRepository> _safetyStockConditionRepository;

    private DeleteSafetyStockConditionsCommandHandler _deleteSafetyStockConditionsCommandHandler;

    [SetUp]
    public void SetUp()
    {
        _mockRepository = new MockRepository(MockBehavior.Loose);

        _safetyStockConditionRepository = _mockRepository.Create<ISafetyStockConditionRepository>();

        _deleteSafetyStockConditionsCommandHandler = new DeleteSafetyStockConditionsCommandHandler(_safetyStockConditionRepository.Object);
    }

    [Test]
    public async Task Handle_DeletesSafetyStockConditions_AndReturnsSuccess()
    {
        // Arrange
        var cmd = new DeleteSafetyStockConditionsCommand([4, 6]);

        // Act
        var result = await _deleteSafetyStockConditionsCommandHandler.Handle(cmd, CancellationToken.None);

        // Assert
        result.Succeeded.ShouldBeTrue();
        _safetyStockConditionRepository.Verify(x => x.DeleteMany(It.Is<int[]>(x => x.Contains(4) & x.Contains(6)), CancellationToken.None), Times.Once);
    }
}
