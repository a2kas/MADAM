using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using Tamro.Madam.Repository.Common;
using Tamro.Madam.Repository.Entities.Customers;
using Tamro.Madam.Repository.UnitOfWork;

namespace Tamro.Madam.Repository.Repositories.Customers;

public class CustomerLegalEntityRepository : ICustomerLegalEntityRepository
{
    private readonly IMadamUnitOfWork _uow;

    public CustomerLegalEntityRepository(IMadamUnitOfWork uow)
    {
        _uow = uow;
    }

    public Task<CustomerLegalEntity?> Get(Expression<Func<CustomerLegalEntity, bool>> filter, List<IncludeOperation<CustomerLegalEntity>>? includes = null, bool track = true, CancellationToken cancellationToken = default)
    {
        var query = GetQuery(filter, includes, track);

        return query.SingleOrDefaultAsync(cancellationToken);
    }

    public async Task<List<CustomerLegalEntity>> GetMany(Expression<Func<CustomerLegalEntity, bool>> filter, List<IncludeOperation<CustomerLegalEntity>>? includes = null, bool track = false, CancellationToken cancellationToken = default)
    {
        var query = GetQuery(filter, includes, track);

        return await query.ToListAsync(cancellationToken);
    }

    public async Task<CustomerLegalEntity> Upsert(CustomerLegalEntity entity, CancellationToken cancellationToken)
    {
        if (entity.Id == default)
        {
            var repository = _uow.GetRepository<CustomerLegalEntity>();
            repository.Create(entity);
        }

        await _uow.SaveChangesAsync(cancellationToken);

        return entity;
    }

    private IQueryable<CustomerLegalEntity> GetQuery(Expression<Func<CustomerLegalEntity, bool>> filter, List<IncludeOperation<CustomerLegalEntity>>? includes = null, bool track = false)
    {
        IQueryable<CustomerLegalEntity> query = _uow.GetRepository<CustomerLegalEntity>().AsQueryable();

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
