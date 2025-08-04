using Shouldly;
using MudBlazor;
using NUnit.Framework;
using Tamro.Madam.Ui.Components.Dialogs;

namespace Tamro.Madam.Ui.Tests.Components.Dialogs;

[TestFixture]
public class ConfirmationDialogTests
{
    [Test]
    public void ConfirmationDialog_DefaultValuesSetsToDeleteConfirmation()
    {
        // Arrange + Act
        var dialog = new ConfirmationDialog();

        // Assert
        dialog.SubmitIcon.ShouldBe(Icons.Material.Filled.Delete);
        dialog.SubmitText.ShouldBe("Delete");
        dialog.SubmitColor.ShouldBe(Color.Error);
        dialog.SubmitVariant.ShouldBe(Variant.Filled);
    }
}
