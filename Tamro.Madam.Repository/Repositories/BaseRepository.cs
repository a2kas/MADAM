using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Tamro.Madam.Repository.Entities;
using TamroUtilities.EFCore;

namespace Tamro.Madam.Repository.Repositories;
public class BaseRepository<TEntity>
        (DbSet<TEntity> _dbSet)
        : IRepository<TEntity> where TEntity : class, IEntity
{
    public TEntity Create(TEntity entity)
    {
        return _dbSet.Add(entity).Entity;
    }

    public void CreateMany(IEnumerable<TEntity> entities)
    {
        _dbSet.AddRange(entities);
    }

    public Task<TEntity> UpsertAsync(TEntity entity)
    {
        var context = _dbSet.GetContext();
        var primaryKeyValue = GetPrimaryKeyValue(context, entity);
        var defaultValue = GetDefaultValue(context);

        if (!Equals(primaryKeyValue, defaultValue))
        {
            return Update(entity, context, primaryKeyValue);
        }

        return Task.FromResult(Create(entity));

    }

    public TEntity Delete(TEntity entity)
    {
        return _dbSet.Remove(entity).Entity;
    }

    public void DeleteMany(ICollection<TEntity> entity)
    {
        _dbSet.RemoveRange(entity);
    }

    public IQueryable<TEntity> AsQueryable()
    {
        return _dbSet;
    }

    public IQueryable<TEntity> AsReadOnlyQueryable()
    {
        return _dbSet.AsNoTracking();
    }

    public void UpdateMany(ICollection<TEntity> entities)
    {
        foreach (var entity in entities)
        {
            _dbSet.Update(entity);
        }
    }

    private static object GetDefaultValue(DbContext context)
    {
        var entityType = context.Model.FindEntityType(typeof(TEntity));
        var primaryKeyProperty = entityType.FindPrimaryKey()?.Properties.FirstOrDefault();
        var primaryKeyType = primaryKeyProperty.ClrType;
        object defaultValue = primaryKeyType.IsValueType ? Activator.CreateInstance(primaryKeyType) : null;

        return defaultValue;
    }

    private async Task<TEntity> Update(TEntity entity, DbContext context, object primaryKeyValue)
    {
        var foundEntity = await _dbSet.FindAsync(primaryKeyValue);

        if (foundEntity != null)
        {
            var entry = context.Entry(foundEntity);

            UpdateEntityProperties(entity, foundEntity, entry);

            return entry.Entity;
        }
        else
        {
            throw new KeyNotFoundException();
        }
    }

    private static void UpdateEntityProperties(TEntity sourceEntity, TEntity targetEntity, EntityEntry<TEntity> entry)
    {
        var properties = typeof(TEntity).GetProperties();
        foreach (var property in properties)
        {
            var newValue = property.GetValue(sourceEntity);
            var currentValue = property.GetValue(targetEntity);

            if (!Equals(newValue, currentValue))
            {
                entry.Property(property.Name).CurrentValue = newValue;
                entry.Property(property.Name).IsModified = true;
            }
        }
    }

    private static object GetPrimaryKeyValue(DbContext context, object entity)
    {
        var entityType = context.Model.FindEntityType(entity.GetType());
        var primaryKey = entityType.FindPrimaryKey();
        var primaryKeyProperty = primaryKey.Properties.FirstOrDefault();

        if (primaryKeyProperty != null)
        {
            var propertyInfo = entity.GetType().GetProperty(primaryKeyProperty.Name);
            return propertyInfo.GetValue(entity);
        }

        return null;
    }
}
