using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Tamro.Madam.Models.General;
using Tamro.Madam.Models.ItemMasterdata.Draft.NewProductOffers;
using Tamro.Madam.Repository.Entities.ItemMasterdata.Draft.NewProductOffers;

namespace Tamro.Madam.Repository.Configuration.ItemMasterdata.Items.Draft;

public class NewProductOfferConfiguration : IEntityTypeConfiguration<NewProductOffer>
{
    public void Configure(EntityTypeBuilder<NewProductOffer> builder)
    {
        builder.ToTable("NewProductOffer");

        builder.HasKey(p => p.Id);

        builder.Property(p => p.SupplierId)
            .IsRequired();

        builder.HasOne(p => p.ItemCategoryManager)
            .WithMany(icm => icm.NewProductOffers)
            .HasForeignKey(p => p.ItemCategoryManagerId)
            .OnDelete(DeleteBehavior.SetNull)
            .IsRequired(false);

        builder.Property(e => e.FileReference)
            .IsRequired()
            .HasMaxLength(150);

        builder.Property(e => e.Country)
            .IsRequired()
            .HasConversion(new EnumToStringConverter<BalticCountry>())
            .HasMaxLength(2);

        builder.Property(e => e.Status)
            .IsRequired()
            .HasConversion(new EnumToStringConverter<NewProductOfferStatus>())
            .HasDefaultValue(NewProductOfferStatus.New)
            .HasMaxLength(20);

        builder.Property(i => i.CreatedDate)
            .HasDefaultValueSql("GETUTCDATE()");
        builder.Property(i => i.RowVer)
            .IsConcurrencyToken()
            .HasDefaultValueSql("GETUTCDATE()");
    }
}