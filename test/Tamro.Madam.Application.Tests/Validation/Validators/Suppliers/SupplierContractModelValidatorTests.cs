using Shouldly;
using NUnit.Framework;
using Tamro.Madam.Application.Tests.TestExtensions.Validation;
using Tamro.Madam.Application.Validation.Validators.Suppliers;
using Tamro.Madam.Models.Suppliers;

namespace Tamro.Madam.Application.Tests.Validation.Validators.Suppliers;

[TestFixture]
public class SupplierContractModelValidatorTests : BaseValidatorTests<SupplierContractModelValidator>
{
    [Test]
    public void SupplierContractModel_IsValid_Validates()
    {
        // Arrange
        var model = CreateValidModel();

        // Act
        var result = _validator.Validate(model);

        // Assert
        result.IsValid.ShouldBeTrue();
    }

    [Test]
    public void SupplierContract_ValidFromIsGreaterThanValidTo_IsInvalid()
    {
        // Arrange
        var model = CreateValidModel();
        model.AgreementValidFrom = DateTime.Now;
        model.AgreementValidTo = DateTime.Now.AddDays(-1);

        // Act
        var result = _validator.Validate(model);

        // Assert
        result.IsValid.ShouldBeFalse();
    }

    private static SupplierContractModel CreateValidModel()
    {
        return new SupplierContractModel();
    }
}