using AutoFixture;
using Shouldly;
using NUnit.Framework;
using Tamro.Madam.Application.Tests.TestExtensions.Validation;
using Tamro.Madam.Application.Validation.Validators.Producers;
using Tamro.Madam.Models.ItemMasterdata.Producers;

namespace Tamro.Madam.Application.Tests.Validation.Validators.Producers;

[TestFixture]
public class ProducerModelValidatorTests : BaseValidatorTests<ProducerModelValidator>
{
    [Test]
    public void ProducerModel_IsValid_Validates()
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
    public void ProducerModel_NameIsEmpty_IsNotValid(string name)
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
    public void ProducerModel_LengthExceeds51Characters_IsNotValid()
    {
        var model = CreateValidModel();
        model.Name = new string(_fixture.CreateMany<char>(51).ToArray());

        // Act
        var result = _validator.Validate(model);

        // Assert
        result.IsValid.ShouldBeFalse();
    }

    private ProducerModel CreateValidModel()
    {
        return new ProducerModel()
        {
            Name = "Grindex",
        };
    }
}
