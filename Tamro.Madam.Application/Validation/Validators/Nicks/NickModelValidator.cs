using FluentValidation;
using Tamro.Madam.Models.ItemMasterdata.Nicks;

namespace Tamro.Madam.Application.Validation.Validators.Nicks;

public class NickModelValidator : AbstractValidator<NickModel>
{
    public NickModelValidator()
    {
        RuleFor(x => x.Name)
            .MaximumLength(50)
            .NotEmpty();
    }
}


