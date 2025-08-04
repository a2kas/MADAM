using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Tamro.Madam.Models.Configuration;

namespace Tamro.Madam.Repository.DependencyInjection.Setup;
internal static class ConfigurationSetup
{
    public static IServiceCollection AddAllDatabasesConfiguration(IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<DatabaseSettings>(configuration.GetSection("Databases"));

        services.AddSingleton(s => s.GetRequiredService<IOptions<DatabaseSettings>>().Value);

        services.AddSingleton<IDatabaseSettings>(s => s.GetRequiredService<IOptions<DatabaseSettings>>().Value);

        return services;
    }
}
