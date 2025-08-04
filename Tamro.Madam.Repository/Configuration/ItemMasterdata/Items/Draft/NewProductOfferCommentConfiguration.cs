using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Tamro.Madam.Models.ItemMasterdata.Draft.NewProductOfferComment;
using Tamro.Madam.Repository.Entities.ItemMasterdata.Draft.NewProductOffers;

namespace Tamro.Madam.Repository.Configuration.ItemMasterdata.Items.Draft;

public class NewProductOfferCommentConfiguration : IEntityTypeConfiguration<NewProductOfferComment>
{
    public void Configure(EntityTypeBuilder<NewProductOfferComment> builder)
    {
        builder.ToTable("NewProductOfferComment");

        builder.HasKey(p => p.Id);

        builder.HasOne(e => e.NewProductOffer)
            .WithMany(npo => npo.Comments)
            .HasForeignKey(e => e.NewProductOfferId)
            .OnDelete(DeleteBehavior.Cascade)
            .IsRequired();

        builder.Property(e => e.Comment)
            .HasMaxLength(int.MaxValue)
            .IsRequired();

        builder.Property(e => e.SubmittedBy)
            .HasMaxLength(20)
            .HasConversion(new EnumToStringConverter<NewProductOfferComentSubmittedBy>())
            .IsRequired();

        builder.Property(e => e.Author)
            .HasMaxLength(50)
            .IsRequired();

        builder.Property(i => i.CreatedDate)
            .HasDefaultValueSql("GETUTCDATE()");
        builder.Property(i => i.RowVer)
            .IsConcurrencyToken()
            .HasDefaultValueSql("GETUTCDATE()");
    }
}