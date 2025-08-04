using Shouldly;
using NUnit.Framework;
using Tamro.Madam.Application.Tests.TestExtensions.Validation;
using Tamro.Madam.Application.Validation.Validators.Items.SafetyStocks;
using Tamro.Madam.Models.General;
using Tamro.Madam.Models.ItemMasterdata.Items.SafetyStocks;

namespace Tamro.Madam.Application.Tests.Validation.Validators.Items.SafetyStocks;

[TestFixture]
public class SafetyStockItemUpsertFormModelValidatorTests : BaseValidatorTests<SafetyStockItemUpsertFormModelValidator>
{
    [Test]
    public void SafetyStockItemUpsertFormModel_IsValid_Validates()
    {
        // Arrange
        var model = CreateValidModel();

        // Act
        var result = _validator.Validate(model);

        // Assert
        result.IsValid.ShouldBeTrue();
    }

    [Test]
    public void SafetyStockItemUpsertFormModel_ItemNoMissing_IsInvalid()
    {
        // Arrange
        var model = CreateValidModel();
        model.Item.ItemNo = string.Empty;

        // Act
        var result = _validator.Validate(model);

        // Assert
        result.IsValid.ShouldBeFalse();
    }

    [Test]
    public void SafetyStockItemUpsertFormModel_RestrictionLevelIsPharmacyChainGroupButNoPharmacyChainGroupsSpecified_IsInvalid()
    {
        // Arrange
        var model = CreateValidModel();
        model.RestrictionLevel = SafetyStockRestrictionLevel.PharmacyChainGroup;
        model.PharmacyGroups = [];

        // Act
        var result = _validator.Validate(model);

        // Assert
        result.IsValid.ShouldBeFalse();
    }

    [Test]
    public void SafetyStockItemUpsertFormModel_RestrictionLevelIsPharmacyChainButNoPharmacyChainsSpecified_IsInvalid()
    {
        // Arrange
        var model = CreateValidModel();
        model.RestrictionLevel = SafetyStockRestrictionLevel.PharmacyChain;
        model.PharmacyChains = [];

        // Act
        var result = _validator.Validate(model);

        // Assert
        result.IsValid.ShouldBeFalse();
    }

    private static SafetyStockItemUpsertFormModel CreateValidModel()
    {
        return new SafetyStockItemUpsertFormModel()
        {
            IsSaveAttempted = true,
            RestrictionLevel = SafetyStockRestrictionLevel.PharmacyChainGroup,
            PharmacyGroups = [PharmacyGroup.Benu, PharmacyGroup.NonBenu,],
            Item = new()
            {
                ItemNo = "W2332",
            },
        };
    }
}