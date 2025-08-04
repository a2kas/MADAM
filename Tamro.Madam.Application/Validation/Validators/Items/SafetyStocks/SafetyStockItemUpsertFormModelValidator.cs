using FluentValidation;
using Tamro.Madam.Models.ItemMasterdata.Items.SafetyStocks;

namespace Tamro.Madam.Application.Validation.Validators.Items.SafetyStocks;

public class SafetyStockItemUpsertFormModelValidator : AbstractValidator<SafetyStockItemUpsertFormModel>
{
    public SafetyStockItemUpsertFormModelValidator()
    {
        RuleFor(x => x.Item)
            .NotNull()
            .Must(x => !string.IsNullOrEmpty(x.ItemNo))
            .When(x => x.IsSaveAttempted)
            .WithMessage("Required");

        RuleFor(x => x.RestrictionLevel)
            .Must((model, restrictionLevel) =>
            {
                if (restrictionLevel == SafetyStockRestrictionLevel.PharmacyChainGroup)
                {
                    return model.PharmacyGroups != null && model.PharmacyGroups.Any();
                }
                else if (restrictionLevel == SafetyStockRestrictionLevel.PharmacyChain)
                {
                    return model.PharmacyChains != null && model.PharmacyChains.Any();
                }
                return true;
            })
            .When(x => x.IsSaveAttempted)
            .WithMessage("At least one value should be selected below");
    }
}
