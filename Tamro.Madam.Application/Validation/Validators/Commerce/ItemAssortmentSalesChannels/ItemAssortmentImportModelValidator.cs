using FluentValidation;
using Tamro.Madam.Models.Commerce.ItemAssortmentSalesChannels;

namespace Tamro.Madam.Application.Validation.Validators.Commerce.ItemAssortmentSalesChannels;

public class ItemAssortmentImportModelValidator : AbstractValidator<ItemAssortmentImportModel>
{
    public ItemAssortmentImportModelValidator()
    {
        RuleFor(x => x.ItemNos)
            .NotEmpty();
    }
}
