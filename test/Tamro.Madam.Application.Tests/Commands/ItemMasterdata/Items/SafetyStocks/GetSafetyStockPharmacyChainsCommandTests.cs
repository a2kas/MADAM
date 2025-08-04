using Shouldly;
using NUnit.Framework;
using Tamro.Madam.Application.Commands.ItemMasterdata.Items.SafetyStocks;
using Tamro.Madam.Models.General;

namespace Tamro.Madam.Application.Tests.Commands.ItemMasterdata.Items.SafetyStocks;

[TestFixture]
public class GetSafetyStockPharmacyChainsCommandTests
{
    [Test]
    public void Ctor_SetsValues()
    {
        // Arrange
        var country = BalticCountry.EE;
        var isActive = true;

        // Act
        var cmd = new GetSafetyStockPharmacyChainsCommand(country, isActive);

        // Assert
        cmd.BalticCountry.ShouldBe(country);
        cmd.IsActive.ShouldBe(isActive);
    }
}