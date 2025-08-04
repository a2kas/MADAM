using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Tamro.Madam.Repository.Entities.ItemMasterdata.Items.SafetyStocks;

namespace Tamro.Madam.Repository.Configuration.ItemMasterdata.Items.SafetyStocks;

internal class SafetyStockConfiguration : IEntityTypeConfiguration<SafetyStock>
{
    public void Configure(EntityTypeBuilder<SafetyStock> builder)
    {
        builder.Property(x => x.RetailQuantity)
            .HasPrecision(18, 2);
    }
}
