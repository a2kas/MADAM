using Shouldly;
using NUnit.Framework;
using Tamro.Madam.Application.Commands.ItemMasterdata.Items.Bindings;
using Tamro.Madam.Models.ItemMasterdata.Items.Bindings;
using Tamro.Madam.Models.State.General;

namespace Tamro.Madam.Application.Tests.Commands.ItemMasterdata.Items.Bindings;

[TestFixture]
public class UpsertItemBindingCommandTests
{
    [Test]
    public void Ctor_Sets_EditedBy()
    {
        // Act
        var cmd = new UpsertItemBindingCommand(new ItemBindingModel(), new UserProfileStateModel() { DisplayName = "Reksas From Prienai", });

        // Assert
        cmd.Model.EditedBy.ShouldBe("Reksas From Prienai");
    }
}