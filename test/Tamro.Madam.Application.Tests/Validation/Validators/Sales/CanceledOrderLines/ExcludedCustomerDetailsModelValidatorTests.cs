using Shouldly;
using NUnit.Framework;
using Tamro.Madam.Application.Tests.TestExtensions.Validation;
using Tamro.Madam.Application.Validation.Validators.Sales.CanceledOrderLines;
using Tamro.Madam.Models.Sales.CanceledOrderLines.ExcludedCustomers;

namespace Tamro.Madam.Application.Tests.Validation.Validators.Sales.CanceledOrderLines;

[TestFixture]
public class ExcludedCustomerDetailsModelValidatorTests : BaseValidatorTests<ExcludedCustomerDetailsModelValidator>
{
    [Test]
    public void ExcludedCustomerDetailsModel_IsValid_Validates()
    {
        // Arrange
        var model = CreateValidModel();

        // Act
        var result = _validator.Validate(model);

        // Assert
        result.IsValid.ShouldBeTrue();
    }

    [Test]
    public void ExcludedCustomerDetailsModel_CustomerIsEmpty_IsNotValid()
    {
        // Arrange
        var model = CreateValidModel();
        model.Customer = null;

        // Act
        var result = _validator.Validate(model);

        // Assert
        result.IsValid.ShouldBeFalse();
    }

    private static ExcludedCustomerDetailsModel CreateValidModel()
    {
        return new ExcludedCustomerDetailsModel()
        {
            Customer = new(),
        };
    }
}