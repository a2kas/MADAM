using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Tamro.Madam.Repository.Entities.ItemMasterdata.Items.Bindings.Assortment;

namespace Tamro.Madam.Repository.Configuration.ItemMasterdata.Items.Bindings.Assortment;

internal class ItemAssortmentBindingMapConfiguration : IEntityTypeConfiguration<ItemAssortmentBindingMap>
{
    public void Configure(EntityTypeBuilder<ItemAssortmentBindingMap> builder)
    {
        builder.ToTable(nameof(ItemAssortmentBindingMap));
        builder.HasKey(i => i.Id);

        builder
            .HasOne(i => i.ItemBinding)
            .WithMany(ib => ib.ItemAssortmentBindingMaps)
            .HasForeignKey(i => i.ItemBindingId)
            .OnDelete(DeleteBehavior.Cascade);
        builder
            .HasOne(i => i.ItemBinding)
            .WithMany(ib => ib.ItemAssortmentBindingMaps)
            .HasForeignKey(i => i.ItemBindingId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.Property(i => i.CreatedDate)
            .HasDefaultValueSql("GETUTCDATE()");
        builder.Property(i => i.RowVer)
            .IsConcurrencyToken()
            .HasDefaultValueSql("GETUTCDATE()");

        builder.HasIndex(i => new { i.ItemBindingId, i.ItemAssortmentSalesChannelId, })
            .IsUnique();
    }
}
