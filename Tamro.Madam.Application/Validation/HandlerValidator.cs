using FluentValidation;

namespace Tamro.Madam.Application.Validation;

public class HandlerValidator : IHandlerValidator
{
    private readonly IValidationService _validationService;

    public HandlerValidator(IValidationService validationService)
    {
        _validationService = validationService;
    }

    public async Task Validate<TRequest>(TRequest model)
    {
        var result = await _validationService.ValidateAsync(model);

        if (result.Any())
        {
            throw new ValidationException(result.First().Value.First());
        }
    }
}
