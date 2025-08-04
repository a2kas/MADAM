using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Tamro.Madam.Repository.Entities;
using Tamro.Madam.Repository.Repositories;

namespace Tamro.Madam.Repository.UnitOfWork;
public interface IUnitOfWork<TBaseEntity> where TBaseEntity : class, IEntity
{
    DbContext Context { get; }
    Task<IDbContextTransaction> BeginTransactionAsync(CancellationToken cancellationToken = default);

    IRepository<TEntity> GetRepository<TEntity>() where TEntity : class, TBaseEntity;

    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}