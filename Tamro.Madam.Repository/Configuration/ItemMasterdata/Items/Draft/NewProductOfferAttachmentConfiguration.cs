using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Tamro.Madam.Repository.Entities.ItemMasterdata.Draft.NewProductOffers;

namespace Tamro.Madam.Repository.Configuration.ItemMasterdata.Items.Draft;

public class NewProductOfferAttachmentConfiguration : IEntityTypeConfiguration<NewProductOfferAttachment>
{
    public void Configure(EntityTypeBuilder<NewProductOfferAttachment> builder)
    {
        builder.ToTable("NewProductOfferAttachment");

        builder.HasKey(p => p.Id);

        builder.HasOne(e => e.NewProductOffer)
            .WithMany(npo => npo.Attachments)
            .HasForeignKey(e => e.NewProductOfferId)
            .OnDelete(DeleteBehavior.Cascade)
            .IsRequired();

        builder.Property(e => e.FileReference)
            .IsRequired()
            .HasMaxLength(150);

        builder.Property(i => i.CreatedDate)
            .HasDefaultValueSql("GETUTCDATE()");
        builder.Property(i => i.RowVer)
            .IsConcurrencyToken()
            .HasDefaultValueSql("GETUTCDATE()");
    }
}