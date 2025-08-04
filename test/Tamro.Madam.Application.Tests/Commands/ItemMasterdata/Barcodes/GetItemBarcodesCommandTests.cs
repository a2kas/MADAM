using Shouldly;
using NUnit.Framework;
using Tamro.Madam.Application.Commands.ItemMasterdata.Barcodes;

namespace Tamro.Madam.Application.Tests.Commands.ItemMasterdata.Barcodes;

[TestFixture]
public class GetItemBarcodesCommandTests
{
    [Test]
    public void GetItemBarcodesCommand_Ctor_Sets_ItemId()
    {
        // Arrange
        const int itemId = 4;

        // Act
        var cmd = new GetItemBarcodesCommand(4);

        // Assert
        cmd.ItemId.ShouldBe(itemId);
    }
}