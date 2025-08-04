using Shouldly;
using NUnit.Framework;
using Tamro.Madam.Application.Tests.TestExtensions.Validation;
using Tamro.Madam.Application.Validation.Validators.Items.Bindings.Retail;
using Tamro.Madam.Common.Constants;
using Tamro.Madam.Models.General;
using Tamro.Madam.Models.ItemMasterdata.Items.Bindings.Retail;

namespace Tamro.Madam.Application.Tests.Validation.Validators.Items.Bindings.Retail;

[TestFixture]
public class GenerateRetailCodesModelValidatorTests : BaseValidatorTests<GenerateRetailCodesModelValidator>
{
    [Test]
    public void GenerateRetailCodesModel_IsValid_Validates()
    {
        // Arrange
        var model = CreateValidModel();

        // Act
        var result = _validator.Validate(model);

        // Assert
        result.IsValid.ShouldBeTrue();
    }

    [TestCase(BalticCountry.EE)]
    [TestCase(BalticCountry.LT)]
    public void GenerateRetailCodesModel_CountryIsNotLv_IsInvalid(BalticCountry country)
    {
        // Arrange
        var model = CreateValidModel();
        model.Country.Value = country;

        // Act
        var result = _validator.Validate(model);

        // Assert
        result.IsValid.ShouldBeFalse();
    }

    [Test]
    public void GenerateRetailCodesModel_CodePrefixIsNotLvRetailPrefix_IsInvalid()
    {
        // Arrange
        var model = CreateValidModel();
        model.CodePrefix = "YSYS";

        // Act
        var result = _validator.Validate(model);

        // Assert
        result.IsValid.ShouldBeFalse();
    }

    [TestCase(-1, false)]
    [TestCase(0, false)]
    [TestCase(1, true)]
    [TestCase(25, true)]
    [TestCase(50, true)]
    [TestCase(65, false)]
    public void GenerateRetailCodesModel_IsValid_OnlyIf_AmountToGenerateIsBetween1to50Inclusive(int amountToGenerate, bool expectedIsValid)
    {
        // Arrange
        var model = CreateValidModel();
        model.AmountToGenerate = amountToGenerate;

        // Act
        var result = _validator.Validate(model);

        // Assert
        result.IsValid.ShouldBe(expectedIsValid);
    }

    private static GenerateRetailCodesModel CreateValidModel()
    {
        return new GenerateRetailCodesModel()
        {
            Country = new CountryModel()
            {
                Name = "LV",
                Value = BalticCountry.LV,
            },
            CodePrefix = CommonConstants.LvRetailPrefix,
            AmountToGenerate = 2,
        };
    }
}