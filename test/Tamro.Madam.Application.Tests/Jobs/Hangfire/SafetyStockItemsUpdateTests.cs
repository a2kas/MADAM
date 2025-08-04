using Shouldly;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using Tamro.Madam.Application.Jobs.Hangfire;
using Tamro.Madam.Application.Services.Items.SafetyStocks;
using TamroUtilities.Hangfire.Models;

namespace Tamro.Madam.Application.Tests.Jobs.Hangfire;

[TestFixture]
public class SafetyStockItemsUpdateTests
{
    private MockRepository _mockRepository;

    private SafetyStockItemsUpdate _safetyStockItemsUpdate;

    [SetUp]
    public void SetUp()
    {
        _mockRepository = new MockRepository(MockBehavior.Loose);

        _safetyStockItemsUpdate = new SafetyStockItemsUpdate();
    }

    [Test]
    public void SafetyStockItemsUpdate_IsHangfireJob()
    {
        // Assert
        _safetyStockItemsUpdate.ShouldBeAssignableTo<HangfireJobBase>();
    }

    [Test]
    public async Task SafetyStockItemsUpdate_UpdatesAllSafetyStockSources()
    {
        // Arrange
        var ltSafetyStockUpdateMock = _mockRepository.Create<ISafetyStockItemsUpdateService>();
        var lvSafetyStockUpdateMock = _mockRepository.Create<ISafetyStockItemsUpdateService>();
        var safetyStockItemsUpdateServices = new List<ISafetyStockItemsUpdateService>() { ltSafetyStockUpdateMock.Object, lvSafetyStockUpdateMock.Object, };
        var logger = _mockRepository.Create<ILogger<SafetyStockItemsUpdate>>().Object;
        var safetyStockItemsUpdate = new SafetyStockItemsUpdate(safetyStockItemsUpdateServices, logger);

        // Act
        await safetyStockItemsUpdate.JobToRun(null, null);

        // Assert
        ltSafetyStockUpdateMock.Verify(x => x.Update(), Times.Once);
        lvSafetyStockUpdateMock.Verify(x => x.Update(), Times.Once);
    }

    [Test]
    public async Task SafetyStockItemsUpdate_CleansUpAllSafetyStockSources()
    {
        // Arrange
        var ltSafetyStockUpdateMock = _mockRepository.Create<ISafetyStockItemsUpdateService>();
        var lvSafetyStockUpdateMock = _mockRepository.Create<ISafetyStockItemsUpdateService>();
        var safetyStockItemsUpdateServices = new List<ISafetyStockItemsUpdateService>() { ltSafetyStockUpdateMock.Object, lvSafetyStockUpdateMock.Object, };
        var logger = _mockRepository.Create<ILogger<SafetyStockItemsUpdate>>().Object;
        var safetyStockItemsUpdate = new SafetyStockItemsUpdate(safetyStockItemsUpdateServices, logger);

        // Act
        await safetyStockItemsUpdate.JobToRun(null, null);

        // Assert
        ltSafetyStockUpdateMock.Verify(x => x.Cleanup(), Times.Once);
        lvSafetyStockUpdateMock.Verify(x => x.Cleanup(), Times.Once);
    }
}