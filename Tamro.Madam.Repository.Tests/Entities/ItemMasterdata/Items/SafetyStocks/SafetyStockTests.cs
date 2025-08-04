using Shouldly;
using NUnit.Framework;
using Tamro.Madam.Repository.Entities.ItemMasterdata.Items.SafetyStocks;
using TamroUtilities.EFCore.Models;

namespace Tamro.Madam.Repository.Tests.Entities.ItemMasterdata.Items.SafetyStocks;

[TestFixture]
public class SafetyStockTests
{
    [Test]
    public void SafetyStock_Implements_IAuditable()
    {
        // Act + Assert
        typeof(IAuditable).IsAssignableFrom(typeof(SafetyStock)).ShouldBeTrue();
    }
}
