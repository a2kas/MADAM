using Shouldly;
using NUnit.Framework;
using Tamro.Madam.Application.Tests.TestExtensions.Validation;
using Tamro.Madam.Application.Validation.Validators.Commerce.ItemAssortmentSalesChannels;
using Tamro.Madam.Models.Commerce.ItemAssortmentSalesChannels;

namespace Tamro.Madam.Application.Tests.Validation.Validators.Commerce.ItemAssortmentSalesChannels;

[TestFixture]
public class ItemAssortmentImportModelValidatorTests : BaseValidatorTests<ItemAssortmentImportModelValidator>
{
    [Test]
    public void ItemAssortmentImportModel_IsValid_Validtes()
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
    public void ItemAssortmentImportModel_ItemNosIsEmpty_IsNotValid(string itemNos)
    {
        // Arrange
        var model = CreateValidModel();
        model.ItemNos = itemNos;

        // Act
        var result = _validator.Validate(model);

        // Assert
        result.IsValid.ShouldBeFalse();
    }

    private ItemAssortmentImportModel CreateValidModel()
    {
        return new ItemAssortmentImportModel()
        {
            ItemNos = "WD WD WD",
        };
    }
}