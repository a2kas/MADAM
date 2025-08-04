using IdentityModel.Client;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Tamro.DocuQueryService.Client;
using Tamro.Madam.Application.Clients;
using Tamro.Madam.Application.Constants;

namespace Tamro.Madam.Ui.DependencyInjection;

public static class ClientExtensions
{
    public static IServiceCollection AddInternalHttpClients(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddAccessTokenManagement(options =>
        {
            options.Client.Clients.Add(IdentityConstants.IdentityServerClientName, new ClientCredentialsTokenRequest
            {
                Address = $"{configuration["BaseApiConfigurations:TamroGateway:Url"]}api/auth/token",
                ClientId = configuration["BaseApiConfigurations:TamroGateway:User"],
                ClientSecret = configuration["BaseApiConfigurations:TamroGateway:Password"],
                Scope = configuration["BaseApiConfigurations:TamroGateway:User"],
            });
        });

        services.AddHttpClient<IExtractionClient, ExtractionClient>(client => client.BaseAddress = new Uri(configuration["BaseApiConfigurations:TamroGateway:Url"]))
            .AddClientAccessTokenHandler(IdentityConstants.IdentityServerClientName);

        services.AddHttpClient(Application.Clients.Constants.MinoClientName);
        services.AddTransient<IMinioHttpClient, MinioHttpClient>();

        return services;
    }
}