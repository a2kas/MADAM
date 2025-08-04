namespace Tamro.Madam.Application.Validation;

public interface IHandlerValidator
{
    Task Validate<TRequest>(TRequest model);
}
