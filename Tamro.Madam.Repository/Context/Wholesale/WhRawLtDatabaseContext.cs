using Microsoft.EntityFrameworkCore;
using System.Reflection;
using Tamro.Madam.Repository.Entities.Wholesale;
using Tamro.Madam.Repository.Entities.Wholesale.Lt;
using TamroUtilities.EFCore;
using TamroUtilities.EFCore.Extensions;

namespace Tamro.Madam.Repository.Context.Wholesale;

public class WhRawLtDatabaseContext : BaseDbContext, IWhRawLtDatabaseContext
{
    public WhRawLtDatabaseContext(DbContextOptions<WhRawLtDatabaseContext> options) : base(options)
    {
    }

    public DbSet<LtItemAvailability> ItemAvailabilities { get; set; }
    public DbSet<WholesaleSafetyStockItem> SafetyStockItems { get; set; }
    public DbSet<WholesaleSafetyStockItemRetailQty> SafetyStockItemRetailQty { get; set; }
    public DbSet<LtWholesaleCustomer> Customers { get; set; }
    public DbSet<LtWholesaleItem> WholesaleItem { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<LtItemAvailability>()
            .HasNoKey();
        modelBuilder.Entity<WholesaleSafetyStockItem>()
            .HasNoKey();
        modelBuilder.Entity<WholesaleSafetyStockItemRetailQty>()
            .HasNoKey();

        modelBuilder.ApplyConfigurationForEntitiesInContext(Assembly.GetExecutingAssembly());

        base.OnModelCreating(modelBuilder);
    }
}
