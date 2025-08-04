using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Tamro.Madam.Repository.Entities;
using Tamro.Madam.Repository.Repositories;

namespace Tamro.Madam.Repository.UnitOfWork;
public class BaseUnitOfWork<TBaseEntity>(DbContext _context) : IUnitOfWork<TBaseEntity> where TBaseEntity : class, IEntity
{
    public DbContext Context { get; } = _context;

    IRepository<TEntity> IUnitOfWork<TBaseEntity>.GetRepository<TEntity>()
    {
        return new BaseRepository<TEntity>(_context.Set<TEntity>());
    }

    public Task<IDbContextTransaction> BeginTransactionAsync(CancellationToken cancellationToken = default)
    {
        return _context.Database.BeginTransactionAsync(cancellationToken);
    }

    public Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        return _context.SaveChangesAsync(cancellationToken);
    }
}
