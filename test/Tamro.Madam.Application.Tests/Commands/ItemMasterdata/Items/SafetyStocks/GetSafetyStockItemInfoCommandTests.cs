using Shouldly;
using NUnit.Framework;
using Tamro.Madam.Application.Commands.ItemMasterdata.Items.SafetyStocks;
using Tamro.Madam.Models.General;

namespace Tamro.Madam.Application.Tests.Commands.ItemMasterdata.Items.SafetyStocks;

[TestFixture]
public class GetSafetyStockItemInfoCommandTests
{
    [Test]
    public void Ctor_SetsValues()
    {
        // Arrange
        const string itemNumber = "W223";
        var country = BalticCountry.EE;

        // Act
        var cmd = new GetSafetyStockItemInfoCommand(itemNumber, country);

        // Assert
        cmd.ItemNo.ShouldBeEquivalentTo(itemNumber);
        cmd.Country.ShouldBe(country);
    }
}