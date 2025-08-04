using Shouldly;
using NUnit.Framework;
using Tamro.Madam.Application.Access;
using Tamro.Madam.Models.Common.Dialog;
using Tamro.Madam.Models.ItemMasterdata.Items;
using Tamro.Madam.Models.State.General;
using Tamro.Madam.Ui.Store.State;
using Tamro.Madam.Ui.Store.State.ItemMasterdata;

namespace Tamro.Madam.Ui.Tests.Store.State.ItemMasterdata;

[TestFixture]
public class ItemStateTests
{
    [Test]
    public void Ctor_WithoutDialogState_SetsDialogStateAsEdit()
    {
        // Arrange
        var item = new ItemModel();

        // Act
        var state = new ItemState(item, new UserProfileState());

        // Assert
        state.DialogState.ShouldBe(DialogState.View);
    }

    [TestCase(Permissions.CanEditCoreMasterdata, DialogState.View, true, true)]
    [TestCase(Permissions.CanEditCoreMasterdata, DialogState.View, false, true)]
    [TestCase(Permissions.CanEditCoreMasterdata, DialogState.Create, true, false)]
    [TestCase(Permissions.CanEditCoreMasterdata, DialogState.Create, false, true)]
    [TestCase(Permissions.CanEditCoreMasterdata, DialogState.Copy, true, true)]
    [TestCase(Permissions.CanEditCoreMasterdata, DialogState.Copy, false, true)]
    [TestCase(Permissions.CanViewAudit, DialogState.View, true, false)]
    [TestCase(Permissions.CanViewAudit, DialogState.View, false, false)]
    [TestCase(Permissions.CanViewAudit, DialogState.Create, false, false)]
    [TestCase(Permissions.CanViewAudit, DialogState.Copy, true, false)]
    [TestCase(Permissions.CanViewAudit, DialogState.Copy, false, false)]
    public void IsEditable_ShouldBeTrue_OnlyHasPermissionAndIf_DialogStateIsEdit_Or_DialogStateIsCopy_Or_DialogStateIsCreateAndItemIsNotParallel(string permission, DialogState dialogState, bool isParallel, bool expectedIsEditable)
    {
        // Arrange
        var item = new ItemModel()
        {
            ParallelParentItemId = isParallel ? 1414 : default,
        };

        var userProfileStateModel = new UserProfileStateModel { Permissions = [permission] };
        var userProfileState = new UserProfileState(false, userProfileStateModel);

        // Act
        var testState = new ItemState(item, dialogState, userProfileState);

        // Assert
        testState.IsEditable.ShouldBe(expectedIsEditable);
    }

    [TestCase(DialogState.Create, false)]
    [TestCase(DialogState.Copy, false)]
    [TestCase(DialogState.View, true)]
    public void IsTabsActive_SholdBeTrue_OnlyIf_DialogStateIsEdit(DialogState dialogState, bool expectedIsTabsActive)
    {
        // Arrange
        var item = new ItemModel();

        // Act
        var testState = new ItemState(item, dialogState);

        // Assert
        testState.IsTabsActive.ShouldBe(expectedIsTabsActive);
    }

    [TestCase(Permissions.CanEditCoreMasterdata, DialogState.Create, true, false)]
    [TestCase(Permissions.CanEditCoreMasterdata, DialogState.Copy, true, true)]
    [TestCase(Permissions.CanEditCoreMasterdata, DialogState.View, true, false)]
    [TestCase(Permissions.CanEditCoreMasterdata, DialogState.Create, false, true)]
    [TestCase(Permissions.CanEditCoreMasterdata, DialogState.Copy, false, true)]
    [TestCase(Permissions.CanEditCoreMasterdata, DialogState.View, false, true)]
    [TestCase(Permissions.CanViewAudit, DialogState.Copy, true, false)]
    [TestCase(Permissions.CanViewAudit, DialogState.Create, false, false)]
    [TestCase(Permissions.CanViewAudit, DialogState.Copy, false, false)]
    [TestCase(Permissions.CanViewAudit, DialogState.View, false, false)]
    public void IsDescriptionEditable_ShouldBeTrue_HasPermissionAndStateIsEdit_And_ItemIsParallel_AndForAllCasesWhenItemIsNotParallel(string permission, DialogState dialogState, bool isParallel, bool expectedIsDescriptionEditable)
    {
        // Arrange
        var item = new ItemModel()
        {
            ParallelParentItemId = isParallel ? 1414 : default,
        };
        var userProfileStateModel = new UserProfileStateModel { Permissions = [permission] };
        var userProfileState = new UserProfileState(false, userProfileStateModel);

        // Act
        var testState = new ItemState(item, dialogState, userProfileState);

        // Assert
        testState.IsDescriptionEditable.ShouldBe(expectedIsDescriptionEditable);
    }
}