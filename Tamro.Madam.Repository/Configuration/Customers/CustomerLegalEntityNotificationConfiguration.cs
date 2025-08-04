using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Tamro.Madam.Repository.Entities.Customers;

namespace Tamro.Madam.Repository.Configuration.Customers;

internal class CustomerLegalEntityNotificationConfiguration : IEntityTypeConfiguration<CustomerLegalEntityNotification>
{
    public void Configure(EntityTypeBuilder<CustomerLegalEntityNotification> builder)
    {
        builder.ToTable(nameof(CustomerLegalEntityNotification));
        builder.HasKey(x => x.Id);
        builder.Property(x => x.SendCanceledOrderNotification)
            .HasDefaultValue(true);
        builder.Property(x => x.CreatedDate)
            .HasDefaultValueSql("GETUTCDATE()");
        builder.Property(x => x.RowVer)
            .IsConcurrencyToken()
            .HasDefaultValueSql("GETUTCDATE()");
    }
}
