using AutoFixture;
using Shouldly;
using NUnit.Framework;
using Tamro.Madam.Application.Tests.TestExtensions.Validation;
using Tamro.Madam.Application.Validation.Validators.SafetyStock.PharmacyChains;
using Tamro.Madam.Models.General;
using Tamro.Madam.Models.ItemMasterdata.Items.SafetyStocks.PharmacyChains;

namespace Tamro.Madam.Application.Tests.Validation.Validators.SafetyStock.PharmacyChains;

[TestFixture]
public class PharmacyChainModelValidatorTests : BaseValidatorTests<PharmacyChainModelValidator>
{
    [Test]
    public void PharmacyChainModel_IsValid_Validates()
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
    public void PharmacyChainModel_DisplayNameIsEmpty_IsNotValid(string name)
    {
        // Arrange
        var model = CreateValidModel();
        model.DisplayName = name;

        // Act
        var result = _validator.Validate(model);

        // Assert
        result.IsValid.ShouldBeFalse();
    }

    [Test]
    public void PharmacyChainModel_DisplayNameLengthExceeds51Characters_IsNotValid()
    {
        var model = CreateValidModel();
        model.DisplayName = new string(_fixture.CreateMany<char>(51).ToArray());

        // Act
        var result = _validator.Validate(model);

        // Assert
        result.IsValid.ShouldBeFalse();
    }

    private PharmacyChainModel CreateValidModel()
    {
        return new PharmacyChainModel()
        {
            Country = BalticCountry.LT,
            DisplayName = "Name",
            Group = PharmacyGroup.Benu,
        };
    }
}
