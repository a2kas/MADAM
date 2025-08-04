using Microsoft.Extensions.DependencyInjection;
using Tamro.Madam.Models.General;
using Tamro.Madam.Repository.Repositories.ItemMasterdata.Items.Wholesale;
using Tamro.Madam.Repository.Repositories.ItemMasterdata.Items.Wholesale.Ee;
using Tamro.Madam.Repository.Repositories.ItemMasterdata.Items.Wholesale.Lt;
using Tamro.Madam.Repository.Repositories.ItemMasterdata.Items.Wholesale.Lv;

namespace Tamro.Madam.Application.Services.Items.Wholesale.Factories;

public class WholesaleItemRepositoryFactory : IWholesaleItemRepositoryFactory
{
    private readonly IServiceProvider _serviceProvider;
    public WholesaleItemRepositoryFactory(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }
    public IWholesaleItemRepository Get(BalticCountry country)
    {
        return country switch
        {
            BalticCountry.LT => _serviceProvider.GetService<LtWholesaleItemRepository>(),
            BalticCountry.LV => _serviceProvider.GetService<LvWholesaleItemRepository>(),
            BalticCountry.EE => _serviceProvider.GetService<EeWholesaleItemRepository>(),
            _ => throw new NotSupportedException($"Country '{country}' not supposed")
        };
    }
}
