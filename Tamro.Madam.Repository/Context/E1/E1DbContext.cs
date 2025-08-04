using Microsoft.EntityFrameworkCore;
using System.Reflection;
using Tamro.Madam.Repository.Entities.Customers.E1;
using Tamro.Madam.Repository.Entities.Sales.Sabis;
using TamroUtilities.EFCore;
using TamroUtilities.EFCore.Extensions;

namespace Tamro.Madam.Repository.Context.E1Gateway;

public class E1DbContext : BaseDbContext, IE1DbContext
{
    public E1DbContext(DbContextOptions<E1DbContext> options) : base(options)
    {
    }

    public DbSet<Customer> Customers { get; set; }
    public DbSet<SksContractMapping> SksContractMappings { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationForEntitiesInContext(Assembly.GetExecutingAssembly());
    }
}
