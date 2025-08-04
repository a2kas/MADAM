using System.Reflection;
using Microsoft.EntityFrameworkCore;
using Tamro.Madam.Repository.Entities.Wholesale.Lv;
using TamroUtilities.EFCore;
using TamroUtilities.EFCore.Extensions;

namespace Tamro.Madam.Repository.Context.Wholesale;

public class WhRawLvDatabaseContext : BaseDbContext, IWhRawLvDatabaseContext
{
    public WhRawLvDatabaseContext(DbContextOptions<WhRawLvDatabaseContext> options) : base(options)
    {
    }

    public DbSet<LvItemAvailability> ItemAvailabilities { get; set; }
    public DbSet<LvWholesaleItem> Items { get; set; }
    public DbSet<LvWholesaleCustomer> Customers { get; set; }
    public DbSet<LvWholesaleEmployee> Employees { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<LvItemAvailability>()
            .HasNoKey();

        modelBuilder.ApplyConfigurationForEntitiesInContext(Assembly.GetExecutingAssembly());

        base.OnModelCreating(modelBuilder);
    }
}
