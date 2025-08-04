using AutoFixture;
using Shouldly;
using NUnit.Framework;
using Tamro.Madam.Application.Tests.TestExtensions.Validation;
using Tamro.Madam.Application.Validation.Validators.Forms;
using Tamro.Madam.Models.ItemMasterdata.Forms;

namespace Tamro.Madam.Application.Tests.Validation.Validators.Forms;

[TestFixture]
public class FormModelValidatorTests : BaseValidatorTests<FormModelValidator>
{
    [Test]
    public void FormModel_IsValid_Validates()
    {
        // Arrange
        var model = CreateValidModel();

        // Act
        var result = _validator.Validate(model);

        // Assert
        result.IsValid.ShouldBeTrue();
    }

    [TestCase("")]
    [TestCase(" ")]
    public void FormModel_NameIsEmpty_IsNotValid(string name)
    {
        // Arrange
        var model = CreateValidModel();
        model.Name = name;

        // Act
        var result = _validator.Validate(model);

        // Assert
        result.IsValid.ShouldBeFalse();
    }

    [Test]
    public void FormModel_LengthExceeds50Characters_IsNotValid()
    {
        var model = CreateValidModel();
        model.Name = new string(_fixture.CreateMany<char>(51).ToArray());

        // Act
        var result = _validator.Validate(model);

        // Assert
        result.IsValid.ShouldBeFalse();
    }

    private static FormModel CreateValidModel()
    {
        return new FormModel()
        {
            Name = "Retard capsules",
        };
    }
}
