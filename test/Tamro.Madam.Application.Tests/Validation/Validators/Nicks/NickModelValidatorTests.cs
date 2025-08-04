using AutoFixture;
using Shouldly;
using NUnit.Framework;
using Tamro.Madam.Application.Tests.TestExtensions.Validation;
using Tamro.Madam.Application.Validation.Validators.Nicks;
using Tamro.Madam.Models.ItemMasterdata.Nicks;

namespace Tamro.Madam.Application.Tests.Validation.Validators.Nicks;

[TestFixture]
public class NickModelValidatorTests : BaseValidatorTests<NickModelValidator>
{
    [Test]
    public void NickModel_IsValid_Validates()
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
    public void NickModel_NameIsEmpty_IsNotValid(string name)
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
    public void NickModel_LengthExceeds51Characters_IsNotValid()
    {
        var model = CreateValidModel();
        model.Name = new string(_fixture.CreateMany<char>(51).ToArray());

        // Act
        var result = _validator.Validate(model);

        // Assert
        result.IsValid.ShouldBeFalse();
    }

    private NickModel CreateValidModel()
    {
        return new NickModel()
        {
            Name = "Livsane",
        };
    }
}
