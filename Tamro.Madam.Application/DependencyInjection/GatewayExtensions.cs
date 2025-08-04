using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Tamro.Madam.Application.Gateways.GptService;

namespace Tamro.Madam.Ui.DependencyInjection;

public static class GatewayExtensions
{
    public static IServiceCollection AddGptService(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddHttpClient<IContractExtractionsClient, ContractExtractionsClient>(client => client.BaseAddress = new Uri(configuration["Gateways:GptService:Url"]));
        return services;
    }
}
