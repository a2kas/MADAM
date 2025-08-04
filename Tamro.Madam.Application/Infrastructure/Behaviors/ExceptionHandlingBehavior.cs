using MediatR;
using Microsoft.Extensions.Logging;
using Tamro.Madam.Application.Models.Common;

namespace Tamro.Madam.Application.Infrastructure.Behaviors;

public class ExceptionHandlingBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
{
    private readonly ILogger<ExceptionHandlingBehavior<TRequest, TResponse>> _logger;

    public ExceptionHandlingBehavior(ILogger<ExceptionHandlingBehavior<TRequest, TResponse>> logger)
    {
        _logger = logger;
    }

    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        try
        {
            return await next();
        }
        catch (Exception ex)
        {
            var responseType = typeof(TResponse);
            if (responseType.GetGenericTypeDefinition() == typeof(Result<>))
            {
                var customExceptionHandler = Array.Find(request.GetType().GetInterfaces(), i =>
                    i.IsGenericType &&
                    i.GetGenericTypeDefinition() == typeof(ICustomExceptionHandler<>) &&
                    i.GenericTypeArguments[0] == responseType);

                if (customExceptionHandler != null)
                {
                    var handleExceptionMethod = customExceptionHandler.GetMethod("HandleException");
                    if (handleExceptionMethod != null)
                    {
                        var customResult = handleExceptionMethod.Invoke(request, [ex]);
                        if (customResult != null)
                        {
                            return (TResponse)customResult;
                        }
                    }
                }
                
                var resultType = responseType.GetGenericArguments()[0];
                var resultGenericType = typeof(Result<>);
                var resultClosedGenericType = resultGenericType.MakeGenericType(resultType);

                var failureMethod = resultClosedGenericType.GetMethod("FailureAsync", [typeof(string[])]);

                var message = ex.Message;
                if (request is IDefaultErrorMessage customExceptionMessage)
                {
                    message = customExceptionMessage.ErrorMessage;
                }

                _logger.LogError(ex, "An error occured: {Message}", ex.Message);

                object[] methodArgs = [new string[] { message }];
                var task = (Task?)failureMethod?.Invoke(null, methodArgs);
                await task.ConfigureAwait(false);
                return (TResponse)((dynamic)task).Result;
            }
            throw;
        }
    }
}
