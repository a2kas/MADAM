using NUnit.Framework;
using Shouldly;
using Tamro.Madam.Application.Tests.TestExtensions.Validation;
using Tamro.Madam.Application.Validation.Validators.Administration.Configuration.Ubl;
using Tamro.Madam.Models.Administration.Configuration.Ubl;
using Tamro.Madam.Models.Customers.Wholesale.Clsf;

namespace Tamro.Madam.Application.Tests.Validation.Validators.Administration.Configuration.Ubl;

[TestFixture]
public class UblApiKeyEditModelValidatorTests : BaseValidatorTests<UblApiKeyEditModelValidator>
{
    [Test]
    public void UblApiKeyEditModel_IsValid_Validates()
    {
        // Arrange
        var model = CreateValidModel();

        // Act
        var result = _validator.Validate(model);

        // Assert
        result.IsValid.ShouldBeTrue();
    }

    [Test]
    public void UblApiKeyEditModel_HasNoCustomer_IsNotValid()
    {
        // Arrange
        var model = CreateValidModel();
        model.Customer = null;

        // Act
        var result = _validator.Validate(model);

        // Assert
        result.IsValid.ShouldBeFalse();
    }

    [Test]
    public void UblApiKeyEditModel_ApiKeyIsNotGuid_IsNotValid()
    {
        // Arrange
        var model = CreateValidModel();
        model.ApiKey = "123";

        // Act
        var result = _validator.Validate(model);

        // Assert
        result.IsValid.ShouldBeFalse();
    }

    private UblApiKeyEditModel CreateValidModel()
    {
        return new UblApiKeyEditModel
        {
            ApiKey = "2067DF54-1FAC-48ED-B37F-EC128865E23D",
            Customer = new WholesaleCustomerClsfModel
            {
                AddressNumber = 123,
                Name = "John Doe",
            },
        };
    }
}
