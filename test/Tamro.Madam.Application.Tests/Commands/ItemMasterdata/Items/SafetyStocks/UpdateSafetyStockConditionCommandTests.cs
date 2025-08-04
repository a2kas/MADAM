using Shouldly;
using NUnit.Framework;
using Tamro.Madam.Application.Commands.ItemMasterdata.Items.SafetyStocks;
using Tamro.Madam.Models.ItemMasterdata.Items.SafetyStocks;

namespace Tamro.Madam.Application.Tests.Commands.ItemMasterdata.Items.SafetyStocks;

[TestFixture]
public class UpdateSafetyStockConditionCommandTests
{
    [Test]
    public void Ctor_SetsModel()
    {
        // Arrange
        var request = new SafetyStockConditionUpsertModel();

        // Act
        var cmd = new UpdateSafetyStockConditionCommand(request);

        // Assert
        cmd.Model.ShouldBe(request);
    }
}