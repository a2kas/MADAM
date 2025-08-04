using Shouldly;
using NUnit.Framework;
using Tamro.Madam.Application.Commands.ItemMasterdata.Barcodes;
using Tamro.Madam.Models.ItemMasterdata.Barcodes;

namespace Tamro.Madam.Application.Tests.Commands.ItemMasterdata.Barcodes;

[TestFixture]
public class UpsertBarcodeCommandTests
{
    [Test]
    public void Ctor_OverWrites_RowVer()
    {
        // Arrange
        var barcodeModel = new BarcodeModel()
        {
            RowVer = null,
        };

        // Act
        var cmd = new UpsertBarcodeCommand(barcodeModel);

        // Assert
        cmd.Model.RowVer.ShouldNotBeNull();
    }
}