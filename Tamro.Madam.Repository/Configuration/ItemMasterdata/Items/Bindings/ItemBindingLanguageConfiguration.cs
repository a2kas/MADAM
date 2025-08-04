using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Tamro.Madam.Repository.Entities.ItemMasterdata.Items.Bindings;

namespace Tamro.Madam.Repository.Configuration.ItemMasterdata.Items.Bindings;

internal class ItemBindingLanguageConfiguration : IEntityTypeConfiguration<ItemBindingLanguage>
{
    public void Configure(EntityTypeBuilder<ItemBindingLanguage> builder)
    {
        builder
            .HasOne(i => i.ItemBinding)
            .WithMany(p => p.Languages)
            .HasForeignKey(i => i.ItemBindingId);
    }
}
