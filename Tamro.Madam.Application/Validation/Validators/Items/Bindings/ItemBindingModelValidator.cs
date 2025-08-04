using FluentValidation;
using Tamro.Madam.Models.ItemMasterdata.Items.Bindings;

namespace Tamro.Madam.Application.Validation.Validators.Items.Bindings;

public class ItemBindingModelValidator : AbstractValidator<ItemBindingModel>
{
    public ItemBindingModelValidator()
    {
        RuleFor(x => x.Company)
            .NotNull();

        RuleFor(x => x.LocalId)
            .NotEmpty()
            .MaximumLength(50);

        RuleFor(x => x.Item)
            .NotNull();
    }
}
