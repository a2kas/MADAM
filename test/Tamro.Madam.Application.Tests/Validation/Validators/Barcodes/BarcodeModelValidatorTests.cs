using AutoFixture;
using Shouldly;
using NUnit.Framework;
using Tamro.Madam.Application.Tests.TestExtensions.Validation;
using Tamro.Madam.Application.Validation.Validators.Barcodes;
using Tamro.Madam.Models.ItemMasterdata.Barcodes;
using Tamro.Madam.Models.ItemMasterdata.Items;

namespace Tamro.Madam.Application.Tests.Validation.Validators.Barcodes;

[TestFixture]
public class BarcodeModelValidatorTests : BaseValidatorTests<BarcodeModelValidator>
{
    [Test]
    public void BarcodeModel_IsValid_Validates()
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
    public void BarcodeModel_EanIsEmpty_IsNotValid(string ean)
    {
        // Arrange
        var model = CreateValidModel();
        model.Ean = ean;

        // Act
        var result = _validator.Validate(model);

        // Assert
        result.IsValid.ShouldBeFalse();
    }

    [Test]
    public void BarcodeModel_EanLengthExceeds50Characters_IsNotValid()
    {
        // Arrange
        var model = CreateValidModel();
        model.Ean = new string(_fixture.CreateMany<char>(51).ToArray());

        // Act
        var result = _validator.Validate(model);

        // Assert
        result.IsValid.ShouldBeFalse();
    }

    [Test]
    public void BarcodeModel_ItemIsNull_IsNotValid()
    {
        // Arrange
        var model = CreateValidModel();
        model.Item = null;

        // Act
        var result = _validator.Validate(model);

        // Assert
        result.IsValid.ShouldBeFalse();
    }

    private static BarcodeModel CreateValidModel()
    {
        return new BarcodeModel()
        {
            Ean = "Livsane",
            Item = new ItemClsfModel(),
        };
    }
}
