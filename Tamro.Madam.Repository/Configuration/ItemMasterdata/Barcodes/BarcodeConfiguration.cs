using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Tamro.Madam.Repository.Entities.ItemMasterdata.Barcodes;

namespace Tamro.Madam.Repository.Configuration.ItemMasterdata.Barcodes;

internal class BarcodeConfiguration : IEntityTypeConfiguration<Barcode>
{
    public void Configure(EntityTypeBuilder<Barcode> builder)
    {
        builder
            .HasOne(b => b.Item)
            .WithMany(i => i.Barcodes)
            .HasForeignKey(b => b.ItemId)
            .HasConstraintName("FK_Barcode_Item");

        builder
            .Property(b => b.RowVer)
            .HasDefaultValueSql("GETUTCDATE()")
            .HasConversion(
                v => v.HasValue ? v.Value.ToUniversalTime() : (DateTime?)null,
                v => v.HasValue ? DateTime.SpecifyKind(v.Value, DateTimeKind.Utc) : null
            );

        builder.ToTable("Barcode");
    }
}
