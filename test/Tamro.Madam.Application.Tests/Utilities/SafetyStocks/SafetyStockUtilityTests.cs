using NUnit.Framework;
using Shouldly;
using Tamro.Madam.Application.Utilities.SafetyStocks;

namespace Tamro.Madam.Application.Tests.Utilities.SafetyStocks;

[TestFixture]
public class SafetyStockUtilityTests
{
    [TestCase("NAR", 30)]
    [TestCase("PSY", 30)]
    [TestCase("YYY", 10)]
    [TestCase("", 10)]
    [TestCase(null, 10)]
    public void GetCheckDays_CalculatesCheckDaysCorrectly(string? group, int expectedCheckDays)
    {
        // Act
        var result = SafetyStockUtility.GetCheckDays(group);

        // Assert
        result.ShouldBe(expectedCheckDays);
    }
}
