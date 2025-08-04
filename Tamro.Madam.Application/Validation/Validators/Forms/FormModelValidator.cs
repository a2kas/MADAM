using FluentValidation;
using Tamro.Madam.Models.ItemMasterdata.Forms;

namespace Tamro.Madam.Application.Validation.Validators.Forms;

public class FormModelValidator : AbstractValidator<FormModel>
{
    public FormModelValidator()
    {
        RuleFor(x => x.Name)
            .MaximumLength(50)
            .NotEmpty();
    }
}
