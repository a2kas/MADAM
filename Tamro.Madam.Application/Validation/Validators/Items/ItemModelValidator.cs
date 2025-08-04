using FluentValidation;
using Tamro.Madam.Models.ItemMasterdata.Items;

namespace Tamro.Madam.Application.Validation.Validators.Items;

public class ItemModelValidator : AbstractValidator<ItemModel>
{
    public ItemModelValidator()
    {
        RuleFor(x => x.Description)
            .MaximumLength(200);

        RuleFor(x => x.Brand)
            .NotEmpty();

        RuleFor(x => x.Producer)
            .NotEmpty();

        RuleFor(x => x.Requestor)
            .NotEmpty();

        RuleFor(x => x.ActiveSubstance)
            .MaximumLength(800);

        RuleFor(x => x.Measure)
            .GreaterThanOrEqualTo(0);

        RuleFor(x => x.Strength)
            .MaximumLength(200);
    }
}
