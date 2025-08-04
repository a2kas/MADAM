using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Tamro.Madam.Repository.Entities.Administration.Configuration.Ubl;
using Tamro.Madam.Repository.Entities.Finance.Peppol;
using Tamro.Madam.Repository.Entities.Sales.CanceledOrderLines;
using Tamro.Madam.Repository.Entities.Sales.HeldOrders;

namespace Tamro.Madam.Repository.Context.E1Gateway;

public interface IE1GatewayDbContext
{
    DatabaseFacade Database { get; }
    DbSet<PeppolInvoice> PeppolInvoices { get; set; }
    DbSet<PeppolInvoiceConsolidated> PeppolInvoiceDetails { get; set; }
    DbSet<E1CanceledOrderHeader> E1CanceledOrderHeaders { get; set; }
    DbSet<E1CanceledOrderLine> E1CanceledOrderLines { get; set; }
    DbSet<CanceledLineStatistic> CanceledLineStatistics { get; set; }
    DbSet<E1HeldOrder> E1HeldOrder { get; set; }
    DbSet<UblApiKey> UblApiKeys { get; set; }
}
