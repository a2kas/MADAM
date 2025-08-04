using Shouldly;
using NUnit.Framework;
using Tamro.Madam.Models.Sales.Sabis;

namespace Tamro.Madam.Models.Tests.Sales.Sabis;

[TestFixture]
public class SksContractModelTests
{
    [Test]
    public void LastEdited_IsReturned_Correctly()
    {
        // Arrange + act
        var model = new SksContractModel()
        {
            EditedAt = new DateTime(2024, 1, 1, 1, 0, 0),
            EditedBy = "Ted Bundy",
        };

        // Assert
        model.LastEdited.ShouldBe("2024-01-01 01:00:00 by Ted Bundy");
    }
}