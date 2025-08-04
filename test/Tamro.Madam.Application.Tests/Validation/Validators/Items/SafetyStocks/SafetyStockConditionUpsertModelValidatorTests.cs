using Shouldly;
using NUnit.Framework;
using Tamro.Madam.Application.Tests.TestExtensions.Validation;
using Tamro.Madam.Application.Validation.Validators.Items.SafetyStocks;
using Tamro.Madam.Models.ItemMasterdata.Items.SafetyStocks;

namespace Tamro.Madam.Application.Tests.Validation.Validators.Items.SafetyStocks;

[TestFixture]
public class SafetyStockConditionUpsertModelValidatorTests : BaseValidatorTests<SafetyStockConditionUpsertModelValidator>
{
    [Test]
    public void SafetyStockConditionUpsertModel_IsValid_Validates()
    {
        // Arrange
        var model = CreateValidModel();

        // Act
        var result = _validator.Validate(model);

        // Assert
        result.IsValid.ShouldBeTrue();
    }

    [TestCase(null)]
    [TestCase(-1)]
    [TestCase(0)]
    public void SafetyStockConditionUpsertModel_CheckDaysNotValid_IsInvalid(int? checkDays)
    {
        // Arrange
        var model = CreateValidModel();
        model.CheckDays = checkDays;

        // Act
        var result = _validator.Validate(model);

        // Assert
        result.IsValid.ShouldBeFalse();
    }

    private static SafetyStockConditionUpsertModel CreateValidModel()
    {
        return new SafetyStockConditionUpsertModel()
        {
            CheckDays = 8,
        };
    }
}
