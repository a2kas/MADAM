using Shouldly;
using NUnit.Framework;
using Tamro.Madam.Application.Tests.TestExtensions.Validation;
using Tamro.Madam.Application.Validation.Validators.Items.SafetyStocks;
using Tamro.Madam.Models.ItemMasterdata.Items.SafetyStocks;

namespace Tamro.Madam.Application.Tests.Validation.Validators.Items.SafetyStocks;

[TestFixture]
public class SafetyStockConditionEditDialogModelValidatorTests : BaseValidatorTests<SafetyStockConditionEditDialogModelValidator>
{
    [Test]
    public void SafetyStockConditionEditDialogModel_IsValid_Validates()
    {
        // Arrange
        var model = CreateValidModel();

        // Act
        var result = _validator.Validate(model);

        // Assert
        result.IsValid.ShouldBeTrue();
    }

    [TestCase(-1)]
    [TestCase(0)]
    public void SafetyStockConditionEditDialogModel_CheckDaysNotValid_IsInvalid(int checkDays)
    {
        // Arrange
        var model = CreateValidModel();
        model.CheckDays = checkDays;

        // Act
        var result = _validator.Validate(model);

        // Assert
        result.IsValid.ShouldBeFalse();
    }

    private static SafetyStockConditionEditDialogModel CreateValidModel()
    {
        return new SafetyStockConditionEditDialogModel()
        {
            CheckDays = 8,
        };
    }
}
