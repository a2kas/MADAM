using Shouldly;
using NUnit.Framework;
using Tamro.Madam.Repository.Entities.ItemMasterdata.Items.SafetyStocks;
using TamroUtilities.EFCore.Models;

namespace Tamro.Madam.Repository.Tests.Entities.ItemMasterdata.Items.SafetyStocks;

[TestFixture]
public class SafetyStockItemTests
{
    [Test]
    public void SafetyStockItem_Implements_IAuditable()
    {
        // Act + Assert
        typeof(IAuditable).IsAssignableFrom(typeof(SafetyStockItem)).ShouldBeTrue();
    }
}
