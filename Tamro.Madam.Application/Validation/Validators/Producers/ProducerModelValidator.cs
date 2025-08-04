using FluentValidation;
using Tamro.Madam.Models.ItemMasterdata.Producers;

namespace Tamro.Madam.Application.Validation.Validators.Producers;

public class ProducerModelValidator : AbstractValidator<ProducerModel>
{
    public ProducerModelValidator()
    {
        RuleFor(x => x.Name)
            .MaximumLength(50)
            .NotEmpty();
    }
}
