using Microsoft.EntityFrameworkCore;
using System.Reflection;
using Tamro.Madam.Repository.Entities.Customers.Sks;
using TamroUtilities.EFCore;
using TamroUtilities.EFCore.Extensions;

namespace Tamro.Madam.Repository.Context.Sks;

public class SksDbContext : BaseDbContext, ISksDbContext
{
    public SksDbContext(DbContextOptions<SksDbContext> options) : base(options)
    {
    }

    public DbSet<OrderNotificationEmail> OrderNotificationEmails { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationForEntitiesInContext(Assembly.GetExecutingAssembly());
    }
}
