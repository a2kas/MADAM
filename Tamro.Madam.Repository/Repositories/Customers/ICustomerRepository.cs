using System.Linq.Expressions;
using Tamro.Madam.Repository.Common;
using Tamro.Madam.Repository.Entities.Customers;

namespace Tamro.Madam.Repository.Repositories.Customers;
public interface ICustomerRepository
{
    Task<Customer> Get(Expression<Func<Customer, bool>> filter, List<IncludeOperation<Customer>>? includes = null, bool track = true, CancellationToken cancellationToken = default);
    Task<List<Customer>> GetMany(Expression<Func<Customer, bool>> filter, List<IncludeOperation<Customer>>? includes = null, bool track = false, CancellationToken cancellationToken = default);
    Task<Customer> Upsert(Customer entity, CancellationToken cancellationToken);
}
