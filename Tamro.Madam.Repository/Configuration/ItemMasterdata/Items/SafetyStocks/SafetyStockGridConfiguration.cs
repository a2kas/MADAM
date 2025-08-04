using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Tamro.Madam.Models.General;
using Tamro.Madam.Repository.Entities.ItemMasterdata.Items.SafetyStocks;

namespace Tamro.Madam.Repository.Configuration.ItemMasterdata.Items.SafetyStocks;

internal class SafetyStockGridConfiguration : IEntityTypeConfiguration<SafetyStockGridData>
{
    public void Configure(EntityTypeBuilder<SafetyStockGridData> builder)
    {
        builder.Property(x => x.Country)
            .HasConversion(new EnumToStringConverter<BalticCountry>())
            .HasMaxLength(2);
    }
}
