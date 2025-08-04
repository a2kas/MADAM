using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Tamro.Madam.Models.General;
using Tamro.Madam.Repository.Entities.ItemMasterdata.Items.SafetyStocks;

namespace Tamro.Madam.Repository.Configuration.ItemMasterdata.Items.SafetyStocks;

internal class SafetyStockPharmacyChainConfiguration : IEntityTypeConfiguration<SafetyStockPharmacyChain>
{
    public void Configure(EntityTypeBuilder<SafetyStockPharmacyChain> builder)
    {
        builder.Property(x => x.Country)
            .HasConversion(new EnumToStringConverter<BalticCountry>())
            .HasMaxLength(2);

        builder.Property(x => x.DisplayName)
            .HasMaxLength(50);

        builder.Property(x => x.Group)
            .HasConversion(new EnumToStringConverter<PharmacyGroup>())
            .HasMaxLength(20);

        builder.HasIndex(x => new { x.Country, x.E1SoldTo }).IsUnique();
    }
}
