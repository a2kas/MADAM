using FluentValidation;
using Tamro.Madam.Models.ItemMasterdata.Items.SafetyStocks.PharmacyChains;

namespace Tamro.Madam.Application.Validation.Validators.SafetyStock.PharmacyChains;

public class PharmacyChainModelValidator : AbstractValidator<PharmacyChainModel>
{
    public PharmacyChainModelValidator()
    {
        RuleFor(x => x.DisplayName)
            .MaximumLength(50)
            .NotEmpty();
    }
}
