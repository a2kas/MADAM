using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Tamro.Madam.Repository.Entities.Wholesale.Lv;

namespace Tamro.Madam.Repository.Configuration.ItemMasterdata.Items.Wholesale.Lv;

internal class LvWholesaleItemConfiguration : IEntityTypeConfiguration<LvWholesaleItem>
{
    public void Configure(EntityTypeBuilder<LvWholesaleItem> builder)
    {
        builder.HasNoKey();
        builder.ToTable("Items");
        builder.Property(p => p.ItemNo)
            .HasColumnName("ItemNo2");
        builder.Property(p => p.ItemDescription)
            .HasColumnName("ItemDescription1");
    }
}
