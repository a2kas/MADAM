using System.Reflection;
using Microsoft.EntityFrameworkCore;
using Tamro.Madam.Repository.Entities.Customers;
using TamroUtilities.Abstractions;
using TamroUtilities.EFCore.Extensions;
using TamroUtilities.EFCore.Models;

namespace Tamro.Madam.Repository.Context.Madam;
public class MadamDbContext : BaseDbContextFindingItsEntities<IMadamEntity>, IMadamDbContext
{
    public MadamDbContext(DbContextOptions<MadamDbContext> options, IUserAccessor userAccessor) : base(options, userAccessor)
    {
    }

    public DbSet<Customer> Customers { get; set; }
    public DbSet<CustomerNotification> CustomerNotifications { get; set; }
    public DbSet<DbAuditEntry> AuditEntry { get; set; }
    public DbSet<DbAuditEntryProperty> AuditEntryProperty { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationForEntitiesInContext(Assembly.GetExecutingAssembly());

        base.OnModelCreating(modelBuilder);
    }
}
