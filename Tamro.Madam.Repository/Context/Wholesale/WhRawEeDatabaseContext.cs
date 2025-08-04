using Microsoft.EntityFrameworkCore;
using System.Reflection;
using Tamro.Madam.Repository.Entities.Wholesale.Ee;
using TamroUtilities.EFCore;
using TamroUtilities.EFCore.Extensions;

namespace Tamro.Madam.Repository.Context.Wholesale;

public class WhRawEeDatabaseContext : BaseDbContext, IWhRawEeDatabaseContext
{
    public WhRawEeDatabaseContext(DbContextOptions<WhRawEeDatabaseContext> options) : base(options)
    {
    }

    public DbSet<EeWholesaleItem> Items { get; set; }
    public DbSet<EeItemAvailability> ItemAvailabilities { get; set; }
    public DbSet<EeWholesaleCustomer> Customers { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<EeItemAvailability>()
            .HasNoKey();

        modelBuilder.ApplyConfigurationForEntitiesInContext(Assembly.GetExecutingAssembly());

        base.OnModelCreating(modelBuilder);
    }
}
