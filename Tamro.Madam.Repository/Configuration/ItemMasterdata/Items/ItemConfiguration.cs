using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Tamro.Madam.Models.ItemMasterdata.Items;
using Tamro.Madam.Repository.Entities.ItemMasterdata.Items;

namespace Tamro.Madam.Repository.Configuration.ItemMasterdata.Items;

internal class ItemConfiguration : IEntityTypeConfiguration<Item>
{
    public void Configure(EntityTypeBuilder<Item> builder)
    {
        builder
            .Property(i => i.ItemType)
            .HasConversion(v => v.ToString(),
            v => (ItemType)Enum.Parse(typeof(ItemType), v))
            .HasMaxLength(50)
            .HasDefaultValue(ItemType.Regular);

        builder
            .HasOne(i => i.Producer)
            .WithMany(p => p.Items)
            .HasForeignKey(i => i.ProducerId)
            .HasConstraintName("FK_Item_Producer");

        builder
            .HasOne(i => i.Brand)
            .WithMany(p => p.Items)
            .HasForeignKey(i => i.BrandId)
            .HasConstraintName("FK_Item_Brand");

        builder
            .HasOne(i => i.MeasurementUnit)
            .WithMany(p => p.Items);

        builder
            .HasOne(i => i.Form)
            .WithMany(p => p.Items)
            .HasForeignKey(i => i.FormId)
            .HasConstraintName("FK_Item_Form");

        builder
            .HasOne(i => i.Atc)
            .WithMany(p => p.Items)
            .HasForeignKey(i => i.AtcId)
            .HasConstraintName("FK_Item_BATC");

        builder
            .HasOne(i => i.Requestor)
            .WithMany(p => p.Items)
            .HasForeignKey(i => i.RequestorId)
            .HasConstraintName("Item_RequestorIdr_FK");

        builder
            .HasOne(i => i.SupplierNick)
            .WithMany(p => p.Items);

        builder.ToTable("Item", tbl => tbl.HasTrigger("TR_Item_InsertUpdate"));
        builder.ToTable("Item", tbl => tbl.HasTrigger("TR_Item_Delete"));
    }
}
