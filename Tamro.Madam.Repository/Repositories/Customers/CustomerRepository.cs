using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using Tamro.Madam.Repository.Common;
using Tamro.Madam.Repository.Entities.Customers;
using Tamro.Madam.Repository.UnitOfWork;

namespace Tamro.Madam.Repository.Repositories.Customers;
public class CustomerRepository : ICustomerRepository
{
    private readonly IMadamUnitOfWork _uow;

    public CustomerRepository(IMadamUnitOfWork uow)
    {
        _uow = uow;
    }

    public Task<Customer> Get(Expression<Func<Customer, bool>> filter, List<IncludeOperation<Customer>>? includes = null, bool track = true, CancellationToken cancellationToken = default)
    {
        var query = GetQuery(filter, includes, track);

        return query.SingleOrDefaultAsync(cancellationToken);
    }

    public async Task<List<Customer>> GetMany(Expression<Func<Customer, bool>> filter, List<IncludeOperation<Customer>>? includes = null, bool track = false, CancellationToken cancellationToken = default)
    {
        var query = GetQuery(filter, includes, track);
        return await query.ToListAsync(cancellationToken);
    }

    private IQueryable<Customer> GetQuery(Expression<Func<Customer, bool>> filter, List<IncludeOperation<Customer>>? includes = null, bool track = false)
    {
        IQueryable<Customer> query = _uow.GetRepository<Customer>().AsQueryable();

        if (includes != null)
        {
            foreach (var include in includes)
            {
                query = include.ApplyInclude(query);
            }
        }

        query = query.Where(filter);

        if (!track)
        {
            query = query.AsNoTracking();
        }

        return query;
    }
}
