using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Tamro.Madam.Models.General;
using Tamro.Madam.Models.ItemMasterdata.Draft.SkuForm;
using Tamro.Madam.Repository.Entities.ItemMasterdata.Draft.SkuForms;

namespace Tamro.Madam.Repository.Configuration.ItemMasterdata.Items.Draft;

public class SkuFormConfiguration : IEntityTypeConfiguration<SkuForm>
{
    public void Configure(EntityTypeBuilder<SkuForm> builder)
    {
        builder.ToTable("SkuForm");

        builder.HasKey(p => p.Id);

        builder.Property(e => e.Country)
            .HasConversion(new EnumToStringConverter<BalticCountry>())
            .HasMaxLength(2)
            .IsRequired();

        builder.Property(e => e.Type)
           .HasConversion(new EnumToStringConverter<SkuFormType>())
           .HasMaxLength(20)
           .IsRequired();

        builder.Property(e => e.VersionMajor)
            .IsRequired();
        builder.Property(e => e.VersionMinor)
            .IsRequired();

        builder.Property(i => i.CreatedDate)
            .HasDefaultValueSql("GETUTCDATE()");
        builder.Property(i => i.RowVer)
            .IsConcurrencyToken()
            .HasDefaultValueSql("GETUTCDATE()");
    }
}