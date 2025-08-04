using Shouldly;
using NUnit.Framework;
using Tamro.Madam.Application.Tests.TestExtensions.Validation;
using Tamro.Madam.Application.Validation.Validators.Sales.Sabis;
using Tamro.Madam.Models.Sales.Sabis;

namespace Tamro.Madam.Application.Tests.Validation.Validators.Sales.Sabis;

[TestFixture]
public class SksContractModelValidatorTests : BaseValidatorTests<SksContractModelValidator>
{
    [Test]
    public void SksContractModel_IsValid_Validates()
    {
        // Arrange
        var model = CreateValidModel();

        // Act
        var result = _validator.Validate(model);

        // Assert
        result.IsValid.ShouldBeTrue();
    }

    [Test]
    public void SksContractModel_CustomerIsEmpty_IsNotValid()
    {
        // Arrange
        var model = CreateValidModel();
        model.Customer = null;

        // Act
        var result = _validator.Validate(model);

        // Assert
        result.IsValid.ShouldBeFalse();
    }

    [TestCase(19, true)]
    [TestCase(21, false)]
    public void SksContractModel_CompanyIdLength_IsValidated(int length, bool expectedIsValid)
    {
        // Arrange
        var model = CreateValidModel();
        model.CompanyId = new string(Enumerable.Repeat('X', length).ToArray());

        // Act
        var result = _validator.Validate(model);

        // Assert
        result.IsValid.ShouldBe(expectedIsValid);
    }

    [TestCase("")]
    [TestCase(" ")]
    public void SksContractModel_ContractTamroIsEmpty_IsNotValid(string contractTamro)
    {
        // Arrange
        var model = CreateValidModel();
        model.ContractTamro = contractTamro;

        // Act
        var result = _validator.Validate(model);

        // Assert
        result.IsValid.ShouldBeFalse();
    }

    [TestCase(149, true)]
    [TestCase(151, false)]
    public void SksContractModel_ContractTamroLength_IsValidated(int length, bool expectedIsValid)
    {
        // Arrange
        var model = CreateValidModel();
        model.ContractTamro = new string(Enumerable.Repeat('X', length).ToArray());

        // Act
        var result = _validator.Validate(model);

        // Assert
        result.IsValid.ShouldBe(expectedIsValid);
    }

    [TestCase("")]
    [TestCase(" ")]
    public void SksContractModel_ContractSabisIsEmpty_IsNotValid(string contractSabis)
    {
        // Arrange
        var model = CreateValidModel();
        model.ContractSabis = contractSabis;

        // Act
        var result = _validator.Validate(model);

        // Assert
        result.IsValid.ShouldBeFalse();
    }

    [TestCase(149, true)]
    [TestCase(151, false)]
    public void SksContractModel_ContractSabisLength_IsValidated(int length, bool expectedIsValid)
    {
        // Arrange
        var model = CreateValidModel();
        model.ContractSabis = new string(Enumerable.Repeat('X', length).ToArray());

        // Act
        var result = _validator.Validate(model);

        // Assert
        result.IsValid.ShouldBe(expectedIsValid);
    }

    private SksContractModel CreateValidModel()
    {
        return new SksContractModel()
        {
            Customer = new()
            {
                AddressNumber = 366,
                Name = "Test",
            },
            ContractTamro = "123",
            ContractSabis = "123",
        };
    }
}