using FluentValidation;
using Tamro.Madam.Models.Suppliers;

namespace Tamro.Madam.Application.Validation.Validators.Suppliers;
public class SupplierContractModelValidator : AbstractValidator<SupplierContractModel>
{
    public SupplierContractModelValidator()
    {
        RuleFor(x => x.AgreementValidFrom)
            .Must((x, _) => !(x.AgreementValidFrom.HasValue && x.AgreementValidTo.HasValue) || x.AgreementValidTo > x.AgreementValidFrom)
            .WithMessage("'Valid from' should be lesser than 'Valid to'");
    }
}
