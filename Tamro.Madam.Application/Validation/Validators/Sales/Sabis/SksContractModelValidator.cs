using FluentValidation;
using Tamro.Madam.Models.Sales.Sabis;

namespace Tamro.Madam.Application.Validation.Validators.Sales.Sabis;

public class SksContractModelValidator : AbstractValidator<SksContractModel>
{
    public SksContractModelValidator()
    {
        RuleFor(x => x.Customer)
            .NotEmpty()
            .WithMessage("Required");
        RuleFor(x => x.CompanyId)
            .MaximumLength(20);
        RuleFor(x => x.ContractTamro)
            .NotEmpty()
            .WithMessage("Required")
            .MaximumLength(150);
        RuleFor(x => x.ContractSabis)
            .NotEmpty()
            .WithMessage("Required")
            .MaximumLength(150);
    }
}
