using Shouldly;
using NUnit.Framework;
using Tamro.Madam.Repository.Entities.ItemMasterdata.Items.SafetyStocks;
using TamroUtilities.EFCore.Models;

namespace Tamro.Madam.Repository.Tests.Entities.ItemMasterdata.Items.SafetyStocks;

[TestFixture]
public class SafetyStockConditionTests
{
    [Test]
    public void SafetyStockCondition_Implements_IAuditable()
    {
        // Act + Assert
        typeof(IAuditable).IsAssignableFrom(typeof(SafetyStockCondition)).ShouldBeTrue();
    }
}
