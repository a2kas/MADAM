namespace Tamro.Madam.Application.Infrastructure.Behaviors;

public interface ICustomExceptionHandler<TResponse>
{
    TResponse HandleException(Exception exception);
}
