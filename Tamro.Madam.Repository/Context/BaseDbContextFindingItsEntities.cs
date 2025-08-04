using System.Reflection;
using Microsoft.EntityFrameworkCore;
using Tamro.Madam.Repository.Entities;
using TamroUtilities.Abstractions;
using TamroUtilities.EFCore;

namespace Tamro.Madam.Repository.Context;
public class BaseDbContextFindingItsEntities<TBaseEntity> : BaseDbContext where TBaseEntity : class, IEntity
{
    public BaseDbContextFindingItsEntities(DbContextOptions options) : base(options)
    {
    }
    public BaseDbContextFindingItsEntities(DbContextOptions options, IUserAccessor userAccessor) : base(options, userAccessor)
    {
    }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(
            Assembly.GetExecutingAssembly(),
            type => type.GetInterfaces().Any(i => // inherits at least one interface, which:
                i.IsGenericType &&
                i.GetGenericTypeDefinition() == typeof(IEntityTypeConfiguration<>) &&
                typeof(TBaseEntity).IsAssignableFrom(i.GenericTypeArguments[0])
            )
        );

        base.OnModelCreating(modelBuilder);
    }

}
