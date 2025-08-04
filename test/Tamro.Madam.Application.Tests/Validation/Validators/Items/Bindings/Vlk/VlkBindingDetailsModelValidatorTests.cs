using Shouldly;
using NUnit.Framework;
using Tamro.Madam.Application.Tests.TestExtensions.Validation;
using Tamro.Madam.Application.Validation.Validators.Items.Bindings.Vlk;
using Tamro.Madam.Models.ItemMasterdata.Items.Bindings.Vlk;
using Tamro.Madam.Models.ItemMasterdata.Items.Bindings;

namespace Tamro.Madam.Application.Tests.Validation.Validators.Items.Bindings.Vlk;

[TestFixture]
public class VlkBindingDetailsModelValidatorTests : BaseValidatorTests<VlkBindingDetailsModelValidator>
{
    [Test]
    public void VlkBindingDetailsModel_IsValid_Validates()
    {
        // Arrange
        var model = CreateValidModel();

        // Act
        var result = _validator.Validate(model);

        // Assert
        result.IsValid.ShouldBeTrue();
    }

    [TestCase(null)]
    [TestCase(0)]
    public void VlkBindingDetailsModel_NpakId7NotValid_Validates(int? npakId7)
    {
        // Arrange
        var model = CreateValidModel();
        model.NpakId7 = npakId7;

        // Act
        var result = _validator.Validate(model);

        // Assert
        result.IsValid.ShouldBeFalse();
    }

    [TestCase]
    public void VlkBindingDetailsModel_ItemBindingNotValid_Validates()
    {
        // Arrange
        var model = CreateValidModel();
        model.ItemBinding = null;

        // Act
        var result = _validator.Validate(model);

        // Assert
        result.IsValid.ShouldBeFalse();
    }

    private static VlkBindingDetailsModel CreateValidModel()
    {
        return new VlkBindingDetailsModel()
        {
            NpakId7 = 22,
            ItemBinding = new ItemBindingClsfModel()
            {
                Id = 2,
                ItemNo2 = "WD-40",
            },
        };
    }
}