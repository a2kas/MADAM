using FluentValidation;
using Tamro.Madam.Models.ItemMasterdata.Items.SafetyStocks;

namespace Tamro.Madam.Application.Validation.Validators.Items.SafetyStocks;

public class SafetyStockConditionEditDialogModelValidator : AbstractValidator<SafetyStockConditionEditDialogModel>
{
    public SafetyStockConditionEditDialogModelValidator()
    {
        RuleFor(x => x.CheckDays)
            .GreaterThan(0);          
    }
}
