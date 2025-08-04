using Microsoft.Extensions.DependencyInjection;
using Tamro.Madam.Application.Services.Customers.Factories;
using Tamro.Madam.Models.General;
using Tamro.Madam.Repository.Repositories.Customers.Wholesale;
using Tamro.Madam.Repository.Repositories.Customers.Wholesale.Lv;

namespace Tamro.Madam.Application.Services.Employees.Factories;
public class WholesaleEmployeeRepositoryFactory : IWholesaleEmployeeRepositoryFactory
{
    private readonly IServiceProvider _serviceProvider;

    public WholesaleEmployeeRepositoryFactory(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public IWholesaleEmployeeRepository Get(BalticCountry country)
    {
        return country switch
        {
            BalticCountry.LV => _serviceProvider.GetService<LvWholesaleEmployeeRepository>(),
            _ => throw new NotSupportedException($"Country '{country}' not supposed")
        };
    }
}
