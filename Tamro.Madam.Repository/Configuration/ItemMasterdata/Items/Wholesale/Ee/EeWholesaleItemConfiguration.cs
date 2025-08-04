using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Tamro.Madam.Repository.Entities.Wholesale.Ee;

namespace Tamro.Madam.Repository.Configuration.ItemMasterdata.Items.Wholesale.Ee;

internal class EeWholesaleItemConfiguration : IEntityTypeConfiguration<EeWholesaleItem>
{
    public void Configure(EntityTypeBuilder<EeWholesaleItem> builder)
    {
        builder.HasNoKey();
        builder.ToTable("ITEMS");
        builder.Property(p => p.ItemNo)
            .HasColumnName("THIRD_ITEM_NO");
        builder.Property(p => p.ItemDescription)
            .HasColumnName("ITEM_DESCRIPTION1");
    }
}
