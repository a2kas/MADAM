using Tamro.Madam.Models.Customers.Wholesale.Clsf;
using Tamro.Madam.Repository.Common;

namespace Tamro.Madam.Repository.Repositories.Customers.E1;

public interface IE1CustomerRepository
{
    Task<PaginatedData<WholesaleCustomerClsfModel>> GetClsf(string searchTerm, int pageNumber, int pageSize);
}
