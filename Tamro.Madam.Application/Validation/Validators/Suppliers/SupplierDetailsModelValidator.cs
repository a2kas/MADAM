using FluentValidation;
using Tamro.Madam.Models.Suppliers;

namespace Tamro.Madam.Application.Validation.Validators.Suppliers;

public class SupplierDetailsModelValidator : AbstractValidator<SupplierDetailsModel>
{
    public SupplierDetailsModelValidator()
    {
        RuleFor(x => x.RegistrationNumber)
            .MaximumLength(20)
            .NotEmpty();

        RuleFor(x => x.Name)
            .MaximumLength(200)
            .NotEmpty();
    }
}
