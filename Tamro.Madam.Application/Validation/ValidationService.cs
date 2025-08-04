using FluentValidation;
using FluentValidation.Internal;
using Microsoft.Extensions.DependencyInjection;
using Tamro.Madam.Application.Extensions;

namespace Tamro.Madam.Application.Validation;

public class ValidationService : IValidationService
{
    private readonly IServiceProvider _serviceProvider;

    public ValidationService(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public Func<object, string, Task<IEnumerable<string>>> ValidateValue<TRequest>()
        => async (model, propertyName)
        => await ValidatePropertyAsync((TRequest)model, propertyName);

    public Func<object, string, Task<IEnumerable<string>>> ValidateValue<TRequest>(TRequest _)
        => ValidateValue<TRequest>();

    public async Task<IDictionary<string, string[]>> ValidateAsync<TRequest>(TRequest model, CancellationToken cancellationToken = default)
    {
        var validators = _serviceProvider.GetServices<IValidator<TRequest>>();

        var context = new ValidationContext<TRequest>(model);

        return (await validators.ValidateAsync(context, cancellationToken)).ToDictionary();
    }

    public async Task<IDictionary<string, string[]>> ValidateAsync<TRequest>(TRequest model, Action<ValidationStrategy<TRequest>> options, CancellationToken cancellationToken = default)
    {
        var validators = _serviceProvider.GetServices<IValidator<TRequest>>();

        var context = ValidationContext<TRequest>
            .CreateWithOptions(model, options);

        return (await validators.ValidateAsync(context, cancellationToken)).ToDictionary();
    }

    public async Task<IEnumerable<string>> ValidatePropertyAsync<TRequest>(TRequest model, string propertyName, CancellationToken cancellationToken = default)
    {
        var segments = propertyName.Split('.');
        if (segments.Length > 2)
        {
            propertyName = string.Join('.', segments.Skip(2));
        }

        var validationResult = await ValidateAsync(model,
            options =>
            {
                options.IncludeProperties(propertyName);
            }, cancellationToken);

        return validationResult.Where(x => x.Key == propertyName).SelectMany(x => x.Value);
    }
}
