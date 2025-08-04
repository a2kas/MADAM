using Tamro.Madam.Models.Customers.Wholesale;
using Tamro.Madam.Models.Customers.Wholesale.Clsf;
using Tamro.Madam.Repository.Common;

namespace Tamro.Madam.Repository.Repositories.Customers.Wholesale;

public interface IWholesaleCustomerRepository
{
    Task<PaginatedData<WholesaleCustomerClsfModel>> GetClsf(IEnumerable<int> addressNumbers, WholesaleCustomerType customerType, int pageNumber, int pageSize);
    Task<PaginatedData<WholesaleCustomerClsfModel>> GetClsf(string searchTerm, WholesaleCustomerType customerType, int pageNumber, int pageSize);
    Task<List<WholesaleCustomerModel>> GetMany(WholesaleCustomerSearchModel search);
}
