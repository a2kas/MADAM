using AutoFixture;
using Shouldly;
using NUnit.Framework;
using Tamro.Madam.Application.Tests.TestExtensions.Validation;
using Tamro.Madam.Application.Validation.Validators.Items;
using Tamro.Madam.Models.ItemMasterdata.Brands;
using Tamro.Madam.Models.ItemMasterdata.Items;
using Tamro.Madam.Models.ItemMasterdata.Producers;
using Tamro.Madam.Models.ItemMasterdata.Requestors;

namespace Tamro.Madam.Application.Tests.Validation.Validators.Items;

[TestFixture]
public class ItemModelValidatorTests : BaseValidatorTests<ItemModelValidator>
{
    [Test]
    public void ItemModel_IsValid_Validates()
    {
        // Arrange
        var model = CreateValidModel();

        // Act
        var result = _validator.Validate(model);

        // Assert
        result.IsValid.ShouldBeTrue();
    }

    [Test]
    public void ItemModel_DescriptionExceeds200Characters_IsNotValid()
    {
        var model = CreateValidModel();
        model.Description = new string(_fixture.CreateMany<char>(201).ToArray());

        // Act
        var result = _validator.Validate(model);

        // Assert
        result.IsValid.ShouldBeFalse();
    }

    [Test]
    public void ItemModel_BrandIsNull_IsNotValid()
    {
        // Arrange
        var model = CreateValidModel();
        model.Brand = null;

        // Act
        var result = _validator.Validate(model);

        // Assert
        result.IsValid.ShouldBeFalse();
    }

    [Test]
    public void ItemModel_ProducerIsNull_IsNotValid()
    {
        // Arrange
        var model = CreateValidModel();
        model.Producer = null;

        // Act
        var result = _validator.Validate(model);

        // Assert
        result.IsValid.ShouldBeFalse();
    }

    [Test]
    public void ItemModel_ActiveSubstanceExceeeds800Characters_IsNotValid()
    {
        // Arrange
        var model = CreateValidModel();
        model.ActiveSubstance = new string(_fixture.CreateMany<char>(801).ToArray());

        // Act
        var result = _validator.Validate(model);

        // Assert
        result.IsValid.ShouldBeFalse();
    }

    [TestCase(-1, false)]
    [TestCase(0, true)]
    [TestCase(1, true)]
    public void ItemModel_MeasureIsValid_OnlyIfGreaterOrEqualTo0(decimal measure, bool expectedIsValid)
    {
        // Arrange
        var model = CreateValidModel();
        model.Measure = measure;

        // Act
        var result = _validator.Validate(model);

        // Assert
        result.IsValid.ShouldBe(expectedIsValid);
    }

    [Test]
    public void ItemModel_StrengthExceeds200Characters_IsNotValid()
    {
        var model = CreateValidModel();
        model.Strength = new string(_fixture.CreateMany<char>(201).ToArray());

        // Act
        var result = _validator.Validate(model);

        // Assert
        result.IsValid.ShouldBeFalse();
    }

    private static ItemModel CreateValidModel()
    {
        return new ItemModel()
        {
            Brand = new BrandClsfModel(),
            Producer = new ProducerClsfModel(),
            Requestor = new RequestorClsfModel(),
        };
    }
}
