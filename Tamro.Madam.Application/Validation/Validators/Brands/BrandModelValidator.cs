using FluentValidation;
using Tamro.Madam.Models.ItemMasterdata.Brands;

namespace Tamro.Madam.Application.Validation.Validators.Brands;

public class BrandModelValidator : AbstractValidator<BrandModel>
{
    public BrandModelValidator()
    {
        RuleFor(x => x.Name)
            .MaximumLength(100)
            .NotEmpty();
    }
}
