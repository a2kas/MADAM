using AutoFixture;
using Shouldly;
using NUnit.Framework;
using Tamro.Madam.Application.Tests.TestExtensions.Validation;
using Tamro.Madam.Application.Validation.Validators.Items.Bindings;
using Tamro.Madam.Models.General;
using Tamro.Madam.Models.ItemMasterdata.Items.Bindings;
using Tamro.Madam.Models.ItemMasterdata.Items;

namespace Tamro.Madam.Application.Tests.Validation.Validators.Items.Bindings;

[TestFixture]
public class ItemBindingModelValidatorTests : BaseValidatorTests<ItemBindingModelValidator>
{
    [Test]
    public void ItemBindingModel_IsValid_Validates()
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
    public void ItemBindingModel_LocalIdIsEmpty_IsNotValid(string localId)
    {
        // Arrange
        var model = CreateValidModel();
        model.LocalId = localId;

        // Act
        var result = _validator.Validate(model);

        // Assert
        result.IsValid.ShouldBeFalse();
    }

    [Test]
    public void ItemBindingModel_LocalIdLengthExceeds51Characters_IsNotValid()
    {
        // Arrange
        var model = CreateValidModel();
        model.LocalId = new string(_fixture.CreateMany<char>(51).ToArray());

        // Act
        var result = _validator.Validate(model);

        // Assert
        result.IsValid.ShouldBeFalse();
    }

    [Test]
    public void ItemBindingModel_CompanyIsNull_IsNotValid()
    {
        // Arrange
        var model = CreateValidModel();
        model.Company = null;

        // Act
        var result = _validator.Validate(model);

        // Assert
        result.IsValid.ShouldBeFalse();
    }

    [Test]
    public void ItemBindingModel_ItemIsNull_IsNotValid()
    {
        // Arrange
        var model = CreateValidModel();
        model.Item = null;

        // Act
        var result = _validator.Validate(model);

        // Assert
        result.IsValid.ShouldBeFalse();
    }

    private ItemBindingModel CreateValidModel()
    {
        return new ItemBindingModel()
        {
            Company = new Company(),
            LocalId = "123312",
            Item = new ItemClsfModel(),
        };
    }
}