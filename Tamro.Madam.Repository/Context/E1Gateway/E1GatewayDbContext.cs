using System.Reflection;
using Microsoft.EntityFrameworkCore;
using Tamro.Madam.Repository.Entities.Administration.Configuration.Ubl;
using Tamro.Madam.Repository.Entities.Finance.Peppol;
using Tamro.Madam.Repository.Entities.Sales.CanceledOrderLines;
using Tamro.Madam.Repository.Entities.Sales.HeldOrders;
using TamroUtilities.EFCore;
using TamroUtilities.EFCore.Extensions;

namespace Tamro.Madam.Repository.Context.E1Gateway;

public class E1GatewayDbContext : BaseDbContext, IE1GatewayDbContext
{
    public E1GatewayDbContext(DbContextOptions<E1GatewayDbContext> options) : base(options)
    {
    }

    public DbSet<PeppolInvoice> PeppolInvoices { get; set; }
    public DbSet<PeppolInvoiceConsolidated> PeppolInvoiceDetails { get; set; }
    public DbSet<E1CanceledOrderHeader> E1CanceledOrderHeaders { get; set; }
    public DbSet<E1CanceledOrderLine> E1CanceledOrderLines { get; set; }
    public DbSet<CanceledLineStatistic> CanceledLineStatistics { get; set; }
    public DbSet<E1HeldOrder> E1HeldOrder { get; set; }
    public DbSet<UblApiKey> UblApiKeys { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationForEntitiesInContext(Assembly.GetExecutingAssembly());
    }
}
