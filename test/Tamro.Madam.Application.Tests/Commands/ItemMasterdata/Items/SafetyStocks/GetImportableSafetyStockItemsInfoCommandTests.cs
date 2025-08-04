using Shouldly;
using NUnit.Framework;
using Tamro.Madam.Application.Commands.ItemMasterdata.Items.SafetyStocks;
using Tamro.Madam.Models.General;

namespace Tamro.Madam.Application.Tests.Commands.ItemMasterdata.Items.SafetyStocks;

[TestFixture]
public class GetImportableSafetyStockItemsInfoCommandTests
{
    [Test]
    public void Ctor_SetsValues()
    {
        // Arrange
        var itemNumbers = new string[] { "1234", "2234", };
        var country = BalticCountry.EE;

        // Act
        var cmd = new GetImportableSafetyStockItemsInfoCommand(itemNumbers, country);

        // Assert
        cmd.ItemNumbers.ShouldBeEquivalentTo(itemNumbers);
        cmd.Country.ShouldBe(country);
    }
}