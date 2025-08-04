using Microsoft.EntityFrameworkCore;
using Tamro.Madam.Repository.Entities.Wholesale;
using Tamro.Madam.Repository.Entities.Wholesale.Lt;

namespace Tamro.Madam.Repository.Context.Wholesale;

public interface IWhRawLtDatabaseContext
{
    DbSet<LtItemAvailability> ItemAvailabilities { get; set; }
    DbSet<WholesaleSafetyStockItem> SafetyStockItems { get; set; }
    DbSet<LtWholesaleCustomer> Customers { get; set; }
    DbSet<LtWholesaleItem> WholesaleItem { get; set; }
    DbSet<WholesaleSafetyStockItemRetailQty> SafetyStockItemRetailQty { get; set; }
}
