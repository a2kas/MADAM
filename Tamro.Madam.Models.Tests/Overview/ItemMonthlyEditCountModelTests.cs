using Shouldly;
using NUnit.Framework;
using Tamro.Madam.Models.Overview;

namespace Tamro.Madam.Models.Tests.Overview;

[TestFixture]
public class ItemMonthlyEditCountModelTests
{
    [TestCase(2001, 3, "Mar.01")]
    [TestCase(2025, 12, "Dec.25")]
    public void Label_Get_ReturnsCorrectly(int year, int month, string expectedLabel)
    {
        // Act
        var model = new ItemMonthlyEditCountModel()
        {
            Year = year,
            Month = month,
        };

        // Assert
        model.Label.ShouldBe(expectedLabel);
    }
}