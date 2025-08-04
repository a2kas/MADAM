using MediatR;
using System.Reflection;
using Tamro.Madam.Application.Infrastructure.Attributes;
using Tamro.Madam.Application.Infrastructure.Cache;
using Tamro.Madam.Application.Infrastructure.Session;

namespace Tamro.Madam.Application.Infrastructure.Behaviors;

public class AuthorizationBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
{
    private readonly IUserContext _userContext;

    public AuthorizationBehavior(IUserContext userContext)
    {
        _userContext = userContext;
    }

    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        var requestType = typeof(TRequest);
        if (MediatRHandlerCache.HandlerCache.TryGetValue(requestType, out var handlerType))
        {
            var permissionAttributes = handlerType.GetCustomAttributes<RequiresPermissionAttribute>();
            if (permissionAttributes.Any())
            {
                var permissions = permissionAttributes.Select(x => x.Permission);
                if (!_userContext.HasPermission(permissions))
                {
                    throw new UnauthorizedAccessException("You do not have permission to perform this action.");
                }
            }
        }

        return await next();
    }
}
