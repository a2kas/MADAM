using Microsoft.Extensions.DependencyInjection;
using Tamro.Madam.Models.General;
using Tamro.Madam.Repository.Repositories.ItemMasterdata.Items.SafetyStocks;
using Tamro.Madam.Repository.Repositories.ItemMasterdata.Items.SafetyStocks.Lt;

namespace Tamro.Madam.Application.Services.Items.SafetyStocks.Factory;

public class SafetyStockWholesaleRepositoryFactory : ISafetyStockWholesaleRepositoryFactory
{
    private readonly IServiceProvider _serviceProvider;

    public SafetyStockWholesaleRepositoryFactory(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public ISafetyStockWholesaleRepository Get(BalticCountry country)
    {
        return country switch
        {
            BalticCountry.LT => _serviceProvider.GetService<LtSafetyStockWholesaleRepository>(),
            _ => throw new NotSupportedException($"Country '{country}' not supposed")
        };
    }
}
