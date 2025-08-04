using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Tamro.Madam.Models.General;
using Tamro.Madam.Repository.Entities.Customers;

namespace Tamro.Madam.Repository.Configuration.Customers;

internal class CustomerLegalEntityConfiguration : IEntityTypeConfiguration<CustomerLegalEntity>
{
    public void Configure(EntityTypeBuilder<CustomerLegalEntity> builder)
    {
        builder.ToTable(nameof(CustomerLegalEntity));
        builder.HasKey(x => x.Id);
        builder.Property(x => x.IsActive)
            .HasDefaultValue(true);
        builder.Property(x => x.Country)
            .HasConversion(new EnumToStringConverter<BalticCountry>())
            .HasMaxLength(2);
        builder.HasOne(x => x.NotificationSettings)
            .WithOne();
        builder.Property(x => x.CreatedDate)
            .HasDefaultValueSql("GETUTCDATE()");
        builder.Property(x => x.RowVer)
            .IsConcurrencyToken()
            .HasDefaultValueSql("GETUTCDATE()");
    }
}
