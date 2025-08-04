using Microsoft.Extensions.Options;
using Tamro.Madam.Models.Configuration;
using Tamroutilities.Email.Models;

namespace Tamro.Madam.Ui.DependencyInjection;

public static class ConfigurationExtensions
{
    public static IServiceCollection AddSettings(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<AuthorizationSettings>(configuration.GetSection("Authorization"))
            .AddSingleton(s => s.GetRequiredService<IOptions<AuthorizationSettings>>().Value)
            .AddSingleton<IAuthorizationSettings>(s => s.GetRequiredService<IOptions<AuthorizationSettings>>().Value);

        services.Configure<EmailSettings>(configuration.GetSection("EmailSettings"));

        services.AddSingleton(s => configuration.GetSection("Sales:CanceledLines:Settings").Get<List<CanceledLineSetting>>() ?? []);

        return services;
    }
}
