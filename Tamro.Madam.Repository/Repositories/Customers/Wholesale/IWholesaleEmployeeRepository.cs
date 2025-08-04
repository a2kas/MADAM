using Tamro.Madam.Models.Employees.Wholesale;

namespace Tamro.Madam.Repository.Repositories.Customers.Wholesale;
public interface IWholesaleEmployeeRepository
{
    Task<List<WholesaleEmployeeModel>> GetMany(WholesaleEmployeeSearchModel search);
}
