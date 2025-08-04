using FluentValidation;
using Tamro.Madam.Models.Commerce.ItemAssortmentSalesChannels;

namespace Tamro.Madam.Application.Validation.Validators.Commerce.ItemAssortmentSalesChannels;

public class ItemAssortmentSalesChannelDetailsModelValidator : AbstractValidator<ItemAssortmentSalesChannelDetailsModel>
{
    public ItemAssortmentSalesChannelDetailsModelValidator()
    {
        RuleFor(x => x.Name)
            .MaximumLength(50)
            .NotEmpty();
    }
}
