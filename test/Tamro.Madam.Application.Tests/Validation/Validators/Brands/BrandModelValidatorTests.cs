using AutoFixture;
using Shouldly;
using NUnit.Framework;
using Tamro.Madam.Application.Tests.TestExtensions.Validation;
using Tamro.Madam.Application.Validation.Validators.Brands;
using Tamro.Madam.Models.ItemMasterdata.Brands;

namespace Tamro.Madam.Application.Tests.Validation.Validators.Brands;

[TestFixture]
public class BrandModelValidatorTests : BaseValidatorTests<BrandModelValidator>
{
    [Test]
    public void BrandModel_IsValid_Validates()
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
    public void BrandModel_NameIsEmpty_IsNotValid(string name)
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
    public void BrandModel_LengthExceeds100Characters_IsNotValid()
    {
        var model = CreateValidModel();
        model.Name = new string(_fixture.CreateMany<char>(101).ToArray());

        // Act
        var result = _validator.Validate(model);

        // Assert
        result.IsValid.ShouldBeFalse();
    }

    private BrandModel CreateValidModel()
    {
        return new BrandModel()
        {
            Name = "Livsane",
        };
    }
}
