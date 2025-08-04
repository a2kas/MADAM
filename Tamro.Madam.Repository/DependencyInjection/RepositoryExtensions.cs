using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Tamro.Madam.Repository.Audit;
using Tamro.Madam.Repository.DependencyInjection.Setup;
using TamroUtilities.Abstractions;

namespace Tamro.Madam.Repository.DependencyInjection;
public static class RepositoryExtensions
{
    public static IServiceCollection AddRepository(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddScoped<UserAccessor>();
        services.AddScoped<IUserAccessor>(services => services.GetRequiredService<UserAccessor>());

        ConfigurationSetup.AddAllDatabasesConfiguration(services, configuration);
        DatabasesSetup.AddAllDatabases(services, configuration);

        return services;
    }
}
