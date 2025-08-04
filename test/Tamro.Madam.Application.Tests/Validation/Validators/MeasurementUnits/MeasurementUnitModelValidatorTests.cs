using AutoFixture;
using Shouldly;
using NUnit.Framework;
using Tamro.Madam.Application.Tests.TestExtensions.Validation;
using Tamro.Madam.Application.Validation.Validators.MeasurementUnits;
using Tamro.Madam.Models.ItemMasterdata.MeasurementUnits;

namespace Tamro.Madam.Application.Tests.Validation.Validators.MeasurementUnits;

[TestFixture]
public class MeasurementUnitModelValidatorTests : BaseValidatorTests<MeasurementUnitModelValidator>
{
    [Test]
    public void MeasurementUnitModel_IsValid_Validates()
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
    public void MeasurementUnitModel_NameIsEmpty_IsNotValid(string name)
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
    public void MeasurementUnitModel_LengthExceeds51Characters_IsNotValid()
    {
        var model = CreateValidModel();
        model.Name = new string(_fixture.CreateMany<char>(51).ToArray());

        // Act
        var result = _validator.Validate(model);

        // Assert
        result.IsValid.ShouldBeFalse();
    }

    private MeasurementUnitModel CreateValidModel()
    {
        return new MeasurementUnitModel()
        {
            Name = "Kg",
        };
    }
}
