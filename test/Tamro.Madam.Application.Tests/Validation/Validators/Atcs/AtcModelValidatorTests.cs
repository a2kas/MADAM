using AutoFixture;
using Shouldly;
using NUnit.Framework;
using Tamro.Madam.Application.Tests.TestExtensions.Validation;
using Tamro.Madam.Application.Validation.Validators.Atcs;
using Tamro.Madam.Models.ItemMasterdata.Atcs;

namespace Tamro.Madam.Application.Tests.Validation.Validators.Atcs;

[TestFixture]
public class AtcModelValidatorTests : BaseValidatorTests<AtcModelValidator>
{
    [Test]
    public void AtcModel_IsValid_Validates()
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
    public void AtcModel_NameIsEmpty_IsNotValid(string name)
    {
        // Arrange
        var model = CreateValidModel();
        model.Name = name;

        // Act
        var result = _validator.Validate(model);

        // Assert
        result.IsValid.ShouldBeFalse();
    }

    [TestCase("")]
    [TestCase(" ")]
    public void AtcModel_ValueIsEmpty_IsNotValid(string value)
    {
        // Arrange
        var model = CreateValidModel();
        model.Value = value;

        // Act
        var result = _validator.Validate(model);

        // Assert
        result.IsValid.ShouldBeFalse();
    }

    [Test]
    public void AtcModel_NameLengthExceeds250Characters_IsNotValid()
    {
        var model = CreateValidModel();
        model.Name = new string(_fixture.CreateMany<char>(251).ToArray());

        // Act
        var result = _validator.Validate(model);

        // Assert
        result.IsValid.ShouldBeFalse();
    }

    [Test]
    public void AtcModel_ValueLengthExceeds50Characters_IsNotValid()
    {
        var model = CreateValidModel();
        model.Value = new string(_fixture.CreateMany<char>(51).ToArray());

        // Act
        var result = _validator.Validate(model);

        // Assert
        result.IsValid.ShouldBeFalse();
    }

    private static AtcModel CreateValidModel()
    {
        return new AtcModel()
        {
            Name = "X",
            Value = "Y",
        };
    }
}
