using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Tamro.Madam.Models.General;
using Tamro.Madam.Repository.Entities.ItemMasterdata.Items.SafetyStocks;

namespace Tamro.Madam.Repository.Configuration.ItemMasterdata.Items.SafetyStocks;

internal class SafetyStockItemConfiguration : IEntityTypeConfiguration<SafetyStockItem>
{
    public void Configure(EntityTypeBuilder<SafetyStockItem> builder)
    {
        builder.Property(x => x.Country)
            .HasConversion(new EnumToStringConverter<BalticCountry>())
            .HasMaxLength(2);

        builder.Property(x => x.ItemNo)
            .HasMaxLength(25);

        builder.Property(x => x.ItemName)
            .HasMaxLength(100);

        builder.Property(x => x.ItemGroup)
            .HasMaxLength(6);

        builder.Property(x => x.ProductClass)
            .HasMaxLength(3);
        
        builder.Property(x => x.Brand)
            .HasMaxLength(70);

        builder.Property(x => x.SupplierNick)
            .HasMaxLength(30);

        builder.Property(x => x.Cn3)
            .HasMaxLength(100);

        builder.Property(x => x.Cn1)
            .HasMaxLength(250);

        builder.Property(x => x.Substance)
            .HasMaxLength(200);

        builder.HasIndex(x => new { x.ItemNo, x.Country }).IsUnique();

        builder.HasOne(ssi => ssi.SafetyStock)
            .WithOne()
            .HasForeignKey<SafetyStock>(ssi => ssi.Id)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
