using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Tamro.Madam.Repository.Entities.Wholesale.Lv;

namespace Tamro.Madam.Repository.Configuration.Customers.Wholesale.Lv;

internal class LvWholesaleCustomerConfiguration : IEntityTypeConfiguration<LvWholesaleCustomer>
{
    public void Configure(EntityTypeBuilder<LvWholesaleCustomer> builder)
    {
        builder.HasNoKey();
        builder.Property(x => x.ElectronicAddress)
            .HasColumnName("ElectronicAddress2");
        builder.Property(x => x.ResponsibleEmployeeNumber)
            .HasColumnName("AddressNumber5");
    }
}
