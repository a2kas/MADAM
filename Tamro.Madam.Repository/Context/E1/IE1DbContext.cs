using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Tamro.Madam.Repository.Entities.Customers.E1;
using Tamro.Madam.Repository.Entities.Sales.Sabis;

namespace Tamro.Madam.Repository.Context.E1Gateway;

public interface IE1DbContext
{
    DatabaseFacade Database { get; }
    DbSet<Customer> Customers { get; set; }
    DbSet<SksContractMapping> SksContractMappings { get; set; }
    Task<int> SaveChangesAsync(CancellationToken cancellationToken);
}
