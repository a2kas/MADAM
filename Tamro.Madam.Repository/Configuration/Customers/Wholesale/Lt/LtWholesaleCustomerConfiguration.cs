using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Tamro.Madam.Repository.Entities.Wholesale.Lt;

namespace Tamro.Madam.Repository.Configuration.Customers.Wholesale.Lv;

internal class LtWholesaleCustomerConfiguration : IEntityTypeConfiguration<LtWholesaleCustomer>
{
    public void Configure(EntityTypeBuilder<LtWholesaleCustomer> builder)
    {
        builder.ToTable("Customers");
        builder.HasNoKey();
    }
}
