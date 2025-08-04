using Shouldly;
using NUnit.Framework;
using Tamro.Madam.Models.Customers.Wholesale.Clsf;

namespace Tamro.Madam.Models.Tests.Customers.Wholesale.Clsf;

[TestFixture]
public class WholesaleCustomerClsfModelTests
{

    [Test]
    public void DisplayName_IsRetrieved_Correctly()
    {
        // Arrange + act
        var model = new WholesaleCustomerClsfModel()
        {
            AddressNumber = 2,
            Name = "DZ",
        };

        // Assert
        model.DisplayName.ShouldBe("2 - DZ");
    }
}