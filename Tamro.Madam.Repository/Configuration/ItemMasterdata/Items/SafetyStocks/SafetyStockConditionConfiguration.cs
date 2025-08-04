using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Tamro.Madam.Models.General;
using Tamro.Madam.Models.ItemMasterdata.Items.SafetyStocks;
using Tamro.Madam.Repository.Entities.ItemMasterdata.Items.SafetyStocks;

namespace Tamro.Madam.Repository.Configuration.ItemMasterdata.Items.SafetyStocks;

internal class SafetyStockConditionConfiguration : IEntityTypeConfiguration<SafetyStockCondition>
{
    public void Configure(EntityTypeBuilder<SafetyStockCondition> builder)
    {
        builder.Property(x => x.Comment)
            .HasMaxLength(300);

        builder.Property(x => x.User)
            .HasMaxLength(100);

        builder.Property(x => x.RowVer)
            .HasDefaultValueSql("GETUTCDATE()");

        builder.Property(x => x.CreatedDate)
             .HasDefaultValueSql("GETUTCDATE()");

        builder.HasOne(ssc => ssc.SafetyStockItem)
            .WithMany(i => i.SafetyStockConditions)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(ssc => ssc.SafetyStockPharmacyChain)
            .WithMany()
            .OnDelete(DeleteBehavior.Restrict);

        builder.Property(x => x.RestrictionLevel)
            .HasConversion(new EnumToStringConverter<SafetyStockRestrictionLevel>())
            .HasMaxLength(30);

        builder.Property(x => x.SafetyStockPharmacyChainGroup)
            .HasConversion(new EnumToStringConverter<PharmacyGroup>())
            .HasMaxLength(30);

        builder.HasIndex(x => new { x.SafetyStockItemId, x.SafetyStockPharmacyChainId, x.SafetyStockPharmacyChainGroup })
            .IsUnique()
            .HasFilter($"({nameof(SafetyStockCondition.SafetyStockPharmacyChainId)} IS NOT NULL)");

        builder.HasIndex(x => new { x.SafetyStockItemId, x.SafetyStockPharmacyChainGroup })
            .IsUnique()
            .HasFilter($"({nameof(SafetyStockCondition.SafetyStockPharmacyChainId)} IS NULL)");
    }
}
