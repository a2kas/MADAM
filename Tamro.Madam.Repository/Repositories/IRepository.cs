using Tamro.Madam.Repository.Entities;

namespace Tamro.Madam.Repository.Repositories;
public interface IRepository<TEntity> where TEntity : IEntity
{
    TEntity Create(TEntity entity);
    void CreateMany(IEnumerable<TEntity> entities);

    /// <summary>
    /// Inserts or Updates entities depending on the value of the Primary Key property
    /// If the Primary Key is empty, the Entity will be Inserted
    /// If the Primary Key is set -- Updated
    /// </summary>
    Task<TEntity> UpsertAsync(TEntity entity);

    /// <summary>
    /// The entity must be FOUND (can be untracked) in the DB first
    /// </summary>
    TEntity Delete(TEntity entity);

    /// <summary>
    /// The entities must be FOUND and TRACKED
    /// (not through AsReadOnly or WithNoTracking)
    /// </summary>
    void DeleteMany(ICollection<TEntity> entity);

    /// <summary>
    /// Query for Updatable (Tracked) entities
    /// </summary>
    IQueryable<TEntity> AsQueryable();

    /// <summary>
    /// Query for read-only (Untracked) entities
    /// </summary>
    IQueryable<TEntity> AsReadOnlyQueryable();
    void UpdateMany(ICollection<TEntity> entities);
}