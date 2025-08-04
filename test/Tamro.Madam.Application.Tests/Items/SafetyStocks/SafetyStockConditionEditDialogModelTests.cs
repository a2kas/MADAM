using Shouldly;
using NUnit.Framework;
using Tamro.Madam.Models.ItemMasterdata.Items.SafetyStocks;

namespace Tamro.Madam.Application.Tests.Items.SafetyStocks;

[TestFixture]
public class SafetyStockConditionEditDialogModelTests
{
    [Test]
    public void Item_IsSet_Correctly()
    {
        // Arrange + act
        var model = new SafetyStockConditionEditDialogModel()
        {
            ItemNo = "1",
            ItemName = "Ibumetin",
        };

        // Assert
        model.Item.ShouldBe("1 - Ibumetin");
    }
}