using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Tamro.Madam.Repository.Entities.ItemMasterdata.Items.Bindings.Vlk;

namespace Tamro.Madam.Repository.Configuration.ItemMasterdata.Items.Bindings.Vlk;

internal class VlkBindingConfiguration : IEntityTypeConfiguration<VlkBinding>
{
    public void Configure(EntityTypeBuilder<VlkBinding> builder)
    {
        builder.HasKey(v => v.Id);

        builder
            .HasOne(v => v.ItemBinding)
            .WithMany()
            .HasForeignKey(v => v.ItemBindingId)
            .OnDelete(DeleteBehavior.Restrict);

        builder
            .HasIndex(v => v.NpakId7)
            .IsUnique();
    }
}
