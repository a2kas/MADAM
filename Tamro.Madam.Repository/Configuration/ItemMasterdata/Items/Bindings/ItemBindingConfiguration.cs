using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Tamro.Madam.Repository.Entities.ItemMasterdata.Items.Bindings;

namespace Tamro.Madam.Repository.Configuration.ItemMasterdata.Items.Bindings;

internal class ItemBindingConfiguration : IEntityTypeConfiguration<ItemBinding>
{
    public void Configure(EntityTypeBuilder<ItemBinding> builder)
    {
        builder
            .HasOne(i => i.Item)
            .WithMany(p => p.Bindings)
            .HasForeignKey(i => i.ItemId);

        builder
            .HasOne(b => b.ItemBindingInfo)
            .WithOne(i => i.ItemBinding)
            .HasForeignKey<ItemBindingInfo>(i => i.ItemBindingId)
            .IsRequired(false)
            .OnDelete(DeleteBehavior.Cascade);

        builder.Property(b => b.RowVer)
            .IsConcurrencyToken()
            .HasDefaultValueSql("GETUTCDATE()")
            .HasColumnName("RowVerNew");

        builder.Property(b => b.RowVerDeprecated)
            .HasColumnName("RowVer");

        builder.ToTable("ItemBinding", tbl => tbl.HasTrigger("TR_ItemBinding_InsertUpdate"));
        builder.ToTable("ItemBinding", tbl => tbl.HasTrigger("TR_ItemBinding_Delete"));
    }
}
