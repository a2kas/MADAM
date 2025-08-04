using Microsoft.Extensions.DependencyInjection;
using Tamro.Madam.Models.General;
using Tamro.Madam.Repository.Repositories.ItemMasterdata.Items.Wholesale;
using Tamro.Madam.Repository.Repositories.ItemMasterdata.Items.Wholesale.Ee;
using Tamro.Madam.Repository.Repositories.ItemMasterdata.Items.Wholesale.Lt;
using Tamro.Madam.Repository.Repositories.ItemMasterdata.Items.Wholesale.Lv;

namespace Tamro.Madam.Application.Services.Items.Wholesale.Factories;

public class WholesaleItemAvailabilityRepositoryFactory : IWholesaleItemAvailabilityRepositoryFactory
{
    private readonly IServiceProvider _serviceProvider;
    public WholesaleItemAvailabilityRepositoryFactory(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public IWholesaleItemAvailabilityRepository Get(BalticCountry country)
    {
        return country switch
        {
            BalticCountry.LT => _serviceProvider.GetService<LtWholesaleItemAvailabilityRepository>(),
            BalticCountry.EE => _serviceProvider.GetService<EeWholesaleItemAvailabilityRepository>(),
            BalticCountry.LV => _serviceProvider.GetService<LvWholesaleItemAvailabilityRepository>(),
            _ => throw new NotSupportedException($"Country '{country}' not supposed")
        };
    }
}
