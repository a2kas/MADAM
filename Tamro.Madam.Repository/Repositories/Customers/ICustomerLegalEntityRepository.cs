using System.Linq.Expressions;
using Tamro.Madam.Repository.Common;
using Tamro.Madam.Repository.Entities.Customers;

namespace Tamro.Madam.Repository.Repositories.Customers;

public interface ICustomerLegalEntityRepository
{
    Task<CustomerLegalEntity?> Get(Expression<Func<CustomerLegalEntity, bool>> filter, List<IncludeOperation<CustomerLegalEntity>>? includes = null, bool track = true, CancellationToken cancellationToken = default);
    Task<List<CustomerLegalEntity>> GetMany(Expression<Func<CustomerLegalEntity, bool>> filter, List<IncludeOperation<CustomerLegalEntity>>? includes = null, bool track = false, CancellationToken cancellationToken = default);
    Task<CustomerLegalEntity> Upsert(CustomerLegalEntity entity, CancellationToken cancellationToken);
}
