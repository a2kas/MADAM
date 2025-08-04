using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components.Authorization;
using Tamro.Madam.Ui.Handlers.Blazor;
using Tamro.Madam.Ui.Services.Auth;

namespace Tamro.Madam.Ui.DependencyInjection;

public static class SecurityExtensions
{
    public static IServiceCollection AddSecurity(this IServiceCollection services)
    {
        services.AddSingleton<IAuthorizationMiddlewareResultHandler, BlazorAuthorizationMiddlewareResultHandler>();

        services.AddAuthorization(options =>
        {
            options.FallbackPolicy = new AuthorizationPolicyBuilder()
                .RequireAuthenticatedUser()
                .Build();
        });

        services.AddScoped<AuthenticationStateProvider, TamroAuthenticationStateProvider>();

        services.AddAntiforgery(options => { options.Cookie.Expiration = TimeSpan.Zero; });

        return services;
    }
}
