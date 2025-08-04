using Microsoft.EntityFrameworkCore;
using Tamro.Madam.Repository.Entities.Wholesale.Lv;

namespace Tamro.Madam.Repository.Context.Wholesale;

public interface IWhRawLvDatabaseContext
{
    DbSet<LvItemAvailability> ItemAvailabilities { get; set; }
    DbSet<LvWholesaleCustomer> Customers { get; set; }
    DbSet<LvWholesaleItem> Items { get; set; }
    DbSet<LvWholesaleEmployee> Employees { get; set; }
}
