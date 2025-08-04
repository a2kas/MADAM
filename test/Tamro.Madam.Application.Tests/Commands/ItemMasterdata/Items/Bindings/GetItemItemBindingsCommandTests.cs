using Shouldly;
using NUnit.Framework;
using Tamro.Madam.Application.Commands.ItemMasterdata.Items.Bindings;

namespace Tamro.Madam.Application.Tests.Commands.ItemMasterdata.Items.Bindings;

[TestFixture]
public class GetItemItemBindingsCommandTests
{
    [Test]
    public void GetItemItemBindingsCommand_Ctor_Sets_ItemId()
    {
        // Arrange
        const int itemId = 4;

        // Act
        var cmd = new GetItemItemBindingsCommand(4);

        // Assert
        cmd.ItemId.ShouldBe(itemId);
    }
}