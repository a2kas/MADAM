using Shouldly;
using NUnit.Framework;
using Tamro.Madam.Application.Commands.ItemMasterdata.Items;
using Tamro.Madam.Models.ItemMasterdata.Items;
using Tamro.Madam.Models.State.General;

namespace Tamro.Madam.Application.Tests.Commands.ItemMasterdata.Items;

[TestFixture]
public class UpsertItemCommandTests
{
    [Test]
    public void Ctor_Sets_EditedAt()
    {
        // Act
        var cmd = new UpsertItemCommand(new ItemModel(), new UserProfileStateModel());

        // Assert
        cmd.Model.EditedAt.ShouldNotBeNull();
    }

    [Test]
    public void Ctor_Sets_EditedBy()
    {
        // Act
        var cmd = new UpsertItemCommand(new ItemModel(), new UserProfileStateModel() { DisplayName = "Reksis", });

        // Assert
        cmd.Model.EditedBy.ShouldBe("Reksis");
    }
}