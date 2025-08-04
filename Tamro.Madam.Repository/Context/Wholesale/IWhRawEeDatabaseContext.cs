using Microsoft.EntityFrameworkCore;
using Tamro.Madam.Repository.Entities.Wholesale.Ee;

namespace Tamro.Madam.Repository.Context.Wholesale;

public interface IWhRawEeDatabaseContext
{
    DbSet<EeWholesaleItem> Items { get; set; }
    DbSet<EeItemAvailability> ItemAvailabilities { get; set; }
    DbSet<EeWholesaleCustomer> Customers { get; set; }
}
