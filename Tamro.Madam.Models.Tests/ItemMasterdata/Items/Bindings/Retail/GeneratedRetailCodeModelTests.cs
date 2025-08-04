using Shouldly;
using NUnit.Framework;
using Tamro.Madam.Models.ItemMasterdata.Items.Bindings.Retail;

namespace Tamro.Madam.Models.Tests.ItemMasterdata.Items.Bindings.Retail;

[TestFixture]
public class GeneratedRetailCodeModelTests
{
    [Test]
    public async Task FullCode_ShouldBeComputed_Correctly()
    {
        // Arrange + act
        var model = new GeneratedRetailCodeModel()
        {
            CodePrefix = "TA",
            Code = 123,
        };

        // Assert
        model.FullCode.ShouldBe("TA123");
    }
}