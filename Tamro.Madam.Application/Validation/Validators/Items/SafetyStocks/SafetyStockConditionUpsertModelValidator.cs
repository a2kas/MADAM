using FluentValidation;
using Tamro.Madam.Models.ItemMasterdata.Items.SafetyStocks;

namespace Tamro.Madam.Application.Validation.Validators.Items.SafetyStocks;

public class SafetyStockConditionUpsertModelValidator : AbstractValidator<SafetyStockConditionUpsertModel>
{
    public SafetyStockConditionUpsertModelValidator()
    {
        RuleFor(x => x.CheckDays)
            .NotEmpty()
            .GreaterThan(0);
    }
}
