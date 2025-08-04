using Tamro.Madam.Models.General;
using Tamro.Madam.Repository.Repositories.Customers.Wholesale;

namespace Tamro.Madam.Application.Services.Customers.Factories;

public interface IWholesaleEmployeeRepositoryFactory
{
    IWholesaleEmployeeRepository Get(BalticCountry country);
}
