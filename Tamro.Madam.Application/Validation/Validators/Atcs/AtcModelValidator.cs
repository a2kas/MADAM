using FluentValidation;
using Tamro.Madam.Models.ItemMasterdata.Atcs;

namespace Tamro.Madam.Application.Validation.Validators.Atcs;

public class AtcModelValidator : AbstractValidator<AtcModel>
{
    public AtcModelValidator()
    {
        RuleFor(x => x.Name)
            .MaximumLength(250)
            .NotEmpty();

        RuleFor(x => x.Value)
            .MaximumLength(50)
            .NotEmpty();
    }
}
