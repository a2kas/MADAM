using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authorization.Policy;

namespace Tamro.Madam.Ui.Handlers.Blazor;

//Hack: https://github.com/dotnet/aspnetcore/issues/52063#issuecomment-1817420640, check if it can be removed once that is resolved
public class BlazorAuthorizationMiddlewareResultHandler : IAuthorizationMiddlewareResultHandler
{
    public Task HandleAsync(RequestDelegate next, HttpContext context, AuthorizationPolicy policy, PolicyAuthorizationResult authorizeResult)
    {
        return next(context);
    }
}
