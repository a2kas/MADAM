using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Tamro.Madam.Repository.Entities.Customers.E1;

namespace Tamro.Madam.Repository.Configuration.Customers.E1;

internal class CustomerConfiguration : IEntityTypeConfiguration<Customer>
{
    public void Configure(EntityTypeBuilder<Customer> builder)
    {
        builder.HasKey(x => x.AddressNumber);
        builder.Property(x => x.MailingName)
            .HasMaxLength(40);
    }
}