using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Tamro.Madam.Repository.Entities.ItemMasterdata.Items.Bindings;

namespace Tamro.Madam.Repository.Configuration.ItemMasterdata.Items.Bindings;

internal class ItemBindingInfoConfiguration : IEntityTypeConfiguration<ItemBindingInfo>
{
    public void Configure(EntityTypeBuilder<ItemBindingInfo> builder)
    {
        builder.ToTable("ItemBindingInfo");
        builder.HasKey(i => i.ItemBindingId);
        builder.Property(x => x.ShortDescription).HasMaxLength(4000);
        builder.Property(x => x.DescriptionReference).HasMaxLength(100);
        builder.Property(x => x.Usage).HasMaxLength(4000);
    }
}
