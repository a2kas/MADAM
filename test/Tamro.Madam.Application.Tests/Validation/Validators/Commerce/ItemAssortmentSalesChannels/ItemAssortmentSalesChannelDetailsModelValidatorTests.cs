using AutoFixture;
using Shouldly;
using NUnit.Framework;
using Tamro.Madam.Application.Tests.TestExtensions.Validation;
using Tamro.Madam.Application.Validation.Validators.Commerce.ItemAssortmentSalesChannels;
using Tamro.Madam.Models.Commerce.ItemAssortmentSalesChannels;

namespace Tamro.Madam.Application.Tests.Validation.Validators.Commerce.ItemAssortmentSalesChannels;

[TestFixture]
public class ItemAssortmentSalesChannelDetailsModelValidatorTests : BaseValidatorTests<ItemAssortmentSalesChannelDetailsModelValidator>
{
    [Test]
    public void ItemAssortmentSalesChannelDetailsModel_IsValid_Validates()
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
    public void ItemAssortmentSalesChannelDetailsModel_NameIsEmpty_IsNotValid(string name)
    {
        // Arrange
        var model = CreateValidModel();
        model.Name = name;

        // Act
        var result = _validator.Validate(model);

        // Assert
        result.IsValid.ShouldBeFalse();
    }

    [Test]
    public void ItemAssortmentSalesChannelDetailsModel_NameLengthExceeds50Characters_IsNotValid()
    {
        var model = CreateValidModel();
        model.Name = new string(_fixture.CreateMany<char>(51).ToArray());

        // Act
        var result = _validator.Validate(model);

        // Assert
        result.IsValid.ShouldBeFalse();
    }

    private static ItemAssortmentSalesChannelDetailsModel CreateValidModel()
    {
        return new ItemAssortmentSalesChannelDetailsModel()
        {
            Name = "Wolts",
        };
    }
}