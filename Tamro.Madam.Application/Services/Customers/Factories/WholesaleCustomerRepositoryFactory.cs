using Microsoft.Extensions.DependencyInjection;
using Tamro.Madam.Models.General;
using Tamro.Madam.Repository.Repositories.Customers.Wholesale;
using Tamro.Madam.Repository.Repositories.Customers.Wholesale.Ee;
using Tamro.Madam.Repository.Repositories.Customers.Wholesale.Lt;
using Tamro.Madam.Repository.Repositories.Customers.Wholesale.Lv;

namespace Tamro.Madam.Application.Services.Customers.Factories;

public class WholesaleCustomerRepositoryFactory : IWholesaleCustomerRepositoryFactory
{
    private readonly IServiceProvider _serviceProvider;

    public WholesaleCustomerRepositoryFactory(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }
    public IWholesaleCustomerRepository Get(BalticCountry country)
    {
        return country switch
        {
            BalticCountry.LV => _serviceProvider.GetService<LvWholesaleCustomerRepository>(),
            BalticCountry.LT => _serviceProvider.GetService<LtWholesaleCustomerRepository>(),
            BalticCountry.EE => _serviceProvider.GetService<EeWholesaleCustomerRepository>(),
            _ => throw new NotSupportedException($"Country '{country}' not supposed")
        };
    }
}
