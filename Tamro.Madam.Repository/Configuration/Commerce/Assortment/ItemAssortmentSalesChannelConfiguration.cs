using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Tamro.Madam.Models.General;
using Tamro.Madam.Repository.Entities.Commerce.Assortment;

namespace Tamro.Madam.Repository.Configuration.Commerce.Assortment;

internal class ItemAssortmentSalesChannelConfiguration : IEntityTypeConfiguration<ItemAssortmentSalesChannel>
{
    public void Configure(EntityTypeBuilder<ItemAssortmentSalesChannel> builder)
    {
        builder.ToTable(nameof(ItemAssortmentSalesChannel));
        builder.HasKey(x => x.Id);

        builder.Property(x => x.Name)
            .HasMaxLength(50);
        builder.Property(x => x.Country)
            .HasConversion(new EnumToStringConverter<BalticCountry>())
            .HasMaxLength(2);
        builder.Property(x => x.CreatedDate)
            .HasDefaultValueSql("GETUTCDATE()");
        builder.Property(x => x.RowVer)
            .IsConcurrencyToken()
            .HasDefaultValueSql("GETUTCDATE()");

        builder.HasIndex(x => new { x.Country, x.Name })
            .IsUnique();
    }
}
