using Microsoft.EntityFrameworkCore;
using Tamro.Madam.Repository.Entities.Customers.Sks;

namespace Tamro.Madam.Repository.Context.Sks;

public interface ISksDbContext
{
    DbSet<OrderNotificationEmail> OrderNotificationEmails { get; set; }
}