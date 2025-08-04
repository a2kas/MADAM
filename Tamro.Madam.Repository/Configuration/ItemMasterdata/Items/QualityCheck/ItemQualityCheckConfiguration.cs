using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Tamro.Madam.Repository.Entities.ItemMasterdata.Items.QualityCheck;

namespace Tamro.Madam.Repository.Configuration.ItemMasterdata.Items.QualityCheck;
internal class ItemQualityCheckConfiguration : IEntityTypeConfiguration<ItemQualityCheck>
{
    public void Configure(EntityTypeBuilder<ItemQualityCheck> builder)
    {
        builder.ToTable(nameof(ItemQualityCheck));
        builder.HasKey(x => x.Id);
        builder.Property(i => i.CreatedDate)
            .HasDefaultValueSql("GETUTCDATE()");
        builder.Property(i => i.RowVer)
            .IsConcurrencyToken()
            .HasDefaultValueSql("GETUTCDATE()");
        builder
            .HasOne(i => i.Item)
            .WithOne()
            .OnDelete(DeleteBehavior.NoAction);
    }
}

