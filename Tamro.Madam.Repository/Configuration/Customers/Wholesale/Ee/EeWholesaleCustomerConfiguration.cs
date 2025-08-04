using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Tamro.Madam.Repository.Entities.Wholesale.Ee;

namespace Tamro.Madam.Repository.Configuration.Customers.Wholesale.Ee;

internal class EeWholesaleCustomerConfiguration : IEntityTypeConfiguration<EeWholesaleCustomer>
{
    public void Configure(EntityTypeBuilder<EeWholesaleCustomer> builder)
    {
        builder.ToTable("CUSTOMER_DATA");
        builder.HasNoKey();
        builder.Property(x => x.AddressNumber)
            .HasColumnType("numeric(8,0)")
            .HasColumnName("ADDRESS_NUMBER");
        builder.Property(x => x.AddressNumber2)
            .HasColumnType("numeric(8,0)")
            .HasColumnName("SECOND_ADDRESS_NUMBER");
        builder.Property(x => x.MailingName)
            .HasColumnName("MAILING_NAME");
        builder.Property(x => x.ElectronicAddress)
            .HasColumnName("ELECTRONIC_ADDRESS");
    }
}
