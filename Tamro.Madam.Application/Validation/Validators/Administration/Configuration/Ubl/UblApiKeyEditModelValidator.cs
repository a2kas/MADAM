using FluentValidation;
using Tamro.Madam.Models.Administration.Configuration.Ubl;

namespace Tamro.Madam.Application.Validation.Validators.Administration.Configuration.Ubl;
public class UblApiKeyEditModelValidator : AbstractValidator<UblApiKeyEditModel>
{
    public UblApiKeyEditModelValidator()
    {
        RuleFor(x => x.Customer)
            .NotNull();

        RuleFor(x => x.ApiKey)
            .Must(apiKey => Guid.TryParse(apiKey, out _))
            .WithMessage("ApiKey must be a valid Guid.");
    }
}
