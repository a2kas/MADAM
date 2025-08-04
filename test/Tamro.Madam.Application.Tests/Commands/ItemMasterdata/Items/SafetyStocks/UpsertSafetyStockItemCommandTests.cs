using Shouldly;
using NUnit.Framework;
using Tamro.Madam.Application.Commands.ItemMasterdata.Items.SafetyStocks;
using Tamro.Madam.Models.ItemMasterdata.Items.SafetyStocks;

namespace Tamro.Madam.Application.Tests.Commands.ItemMasterdata.Items.SafetyStocks;

[TestFixture]
public class UpsertSafetyStockItemCommandTests
{
    [Test]
    public void Ctor_SetsValues()
    {
        // Arrange
        var request = new SafetyStockItemUpsertFormModel();

        // Act
        var cmd = new UpsertSafetyStockItemCommand(request);

        // Assert
        cmd.Model.ShouldBe(request);
    }
}