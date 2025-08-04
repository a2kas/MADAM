using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Tamro.Madam.Models.General;
using Tamro.Madam.Repository.Entities.ItemMasterdata.Items.Bindings.Retail;

namespace Tamro.Madam.Repository.Configuration.ItemMasterdata.Items.Bindings.Retail;

internal class GeneratedRetailCodeConfiguration : IEntityTypeConfiguration<GeneratedRetailCode>
{
    public void Configure(EntityTypeBuilder<GeneratedRetailCode> builder)
    {
        builder.ToTable("GeneratedRetailCodes");
        builder.Property(x => x.Country)
            .HasConversion(new EnumToStringConverter<BalticCountry>())
            .HasMaxLength(2);

        builder.Property(x => x.CodePrefix)
            .HasMaxLength(10);

        builder.Property(x => x.GeneratedBy)
            .HasMaxLength(50);

        builder.HasIndex(x => new { x.Country, x.CodePrefix, x.Code }).IsUnique();
    }
}
