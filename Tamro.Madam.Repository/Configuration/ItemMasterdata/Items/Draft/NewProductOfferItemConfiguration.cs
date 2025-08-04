using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Tamro.Madam.Models.ItemMasterdata.Draft.NewProductOfferItems;
using Tamro.Madam.Repository.Entities.ItemMasterdata.Draft.NewProductOffers;

namespace Tamro.Madam.Repository.Configuration.ItemMasterdata.Items.Draft;

public class NewProductOfferItemConfiguration : IEntityTypeConfiguration<NewProductOfferItem>
{
    public void Configure(EntityTypeBuilder<NewProductOfferItem> builder)
    {
        builder.ToTable("NewProductOfferItem");

        builder.HasKey(e => e.Id);

        builder.HasOne(e => e.NewProductOffer)
            .WithMany(npo => npo.Items)
            .HasForeignKey(e => e.NewProductOfferId)
            .OnDelete(DeleteBehavior.Cascade)
            .IsRequired();

        builder.Property(e => e.Name)
            .IsRequired()
            .HasMaxLength(150);

        builder.Property(e => e.NameEn)
            .IsRequired()
            .HasMaxLength(150);

        builder.Property(e => e.Group).HasMaxLength(100);
        builder.Property(e => e.Class).HasMaxLength(100);
        builder.Property(e => e.Atc).HasMaxLength(30);
        builder.Property(e => e.Barcode).HasMaxLength(30);
        builder.Property(e => e.RegistrationNo).HasMaxLength(30);
        builder.Property(e => e.Strength).HasMaxLength(30);
        builder.Property(e => e.Measure).HasMaxLength(30);
        builder.Property(e => e.UnitOfMeasure).HasMaxLength(30);
        builder.Property(e => e.Numero).HasMaxLength(10);
        builder.Property(e => e.Brand).HasMaxLength(50);
        builder.Property(e => e.ActiveSubstance).HasMaxLength(200);
        builder.Property(e => e.StoringTemperature).HasMaxLength(50);
        builder.Property(e => e.SpecialStorage).HasMaxLength(50);
        builder.Property(e => e.PackageOrLabelLanguage).HasMaxLength(50);
        builder.Property(e => e.PatientInformationLeafletLanguage).HasMaxLength(50);
        builder.Property(e => e.Height).HasMaxLength(30);
        builder.Property(e => e.Width).HasMaxLength(30);
        builder.Property(e => e.Depth).HasMaxLength(30);
        builder.Property(e => e.GrossWeight).HasMaxLength(30);
        builder.Property(e => e.PackageQty).HasMaxLength(30);
        builder.Property(e => e.PackageQtyInSecondaryUnit).HasMaxLength(30);
        builder.Property(e => e.PackageQtyInTransportUnit).HasMaxLength(30);
        builder.Property(e => e.MaximumShelfLifeInDays).HasMaxLength(30);
        builder.Property(e => e.CipPriceWithoutVat).HasMaxLength(30);
        builder.Property(e => e.DiscountWhsl).HasMaxLength(30);
        builder.Property(e => e.DiscountWhslAdditional).HasMaxLength(30);
        builder.Property(e => e.DiscountCountry).HasMaxLength(30);
        builder.Property(e => e.DiscountRet).HasMaxLength(30);
        builder.Property(e => e.Vat).HasMaxLength(30);
        builder.Property(e => e.Supplier).HasMaxLength(50);
        builder.Property(e => e.SupplierItemCode).HasMaxLength(30);
        builder.Property(e => e.CountryOfOrigin).HasMaxLength(30);
        builder.Property(e => e.CountryOfDelivery).HasMaxLength(30);
        builder.Property(e => e.Producer).HasMaxLength(50);
        builder.Property(e => e.ProducerAddress).HasMaxLength(100);
        builder.Property(e => e.ProducerEmail).HasMaxLength(50);
        builder.Property(e => e.ResponsibleContact).HasMaxLength(150);
        builder.Property(e => e.ProductInformationLeafletReference).HasMaxLength(200);
        builder.Property(e => e.Keywords).HasMaxLength(1000);
        builder.Property(e => e.SkinCondition).HasMaxLength(500);
        builder.Property(e => e.HairCondition).HasMaxLength(500);
        builder.Property(e => e.SpecialProperties).HasMaxLength(500);
        builder.Property(e => e.AgeLimitMin).HasMaxLength(50);
        builder.Property(e => e.AgeLimitMax).HasMaxLength(50);
        builder.Property(e => e.LifeStage).HasMaxLength(50);
        builder.Property(e => e.WarningsAndSafetyInstructions).HasMaxLength(4000);
        builder.Property(e => e.WarningsAndSafetySigns).HasMaxLength(4000);
        builder.Property(e => e.ReimbursedFrom).HasMaxLength(40);
        builder.Property(e => e.NarcoticsOrPsychotropics).HasMaxLength(30);
        builder.Property(e => e.MedicineType).HasMaxLength(30);
        builder.Property(e => e.NpakId7).HasMaxLength(30);
        builder.Property(e => e.Ingredients).HasMaxLength(100);
        builder.Property(e => e.AlcoholPercentage).HasMaxLength(20);
        builder.Property(e => e.NomenclatureNumber).HasMaxLength(30);
        builder.Property(e => e.RecommendedSalesPriceInPharmacy).HasMaxLength(30);
        builder.Property(e => e.CompensationBase).HasMaxLength(50);
        builder.Property(e => e.FmdVerificationInboundTamro).HasMaxLength(50);
        builder.Property(e => e.FmdSupplierType).HasMaxLength(50);
        builder.Property(e => e.MarketingAuthorizationHolder).HasMaxLength(50);
        builder.Property(e => e.MainSalesChannel).HasMaxLength(50);
        builder.Property(e => e.MarketingActivitiesInTv).HasMaxLength(300);
        builder.Property(e => e.MarketingActivitiesInBenu).HasMaxLength(300);
        builder.Property(e => e.MarketingInvestmentInBenu).HasMaxLength(300);
        builder.Property(e => e.MainProductCompetitors).HasMaxLength(200);
        builder.Property(e => e.ArgumentsForSale).HasMaxLength(300);


        builder.Property(e => e.Status)
            .IsRequired()
            .HasConversion(new EnumToStringConverter<NewProductOfferItemStatus>())
            .HasDefaultValue(NewProductOfferItemStatus.New)
            .HasMaxLength(20);

        builder.Property(i => i.CreatedDate)
            .HasDefaultValueSql("GETUTCDATE()");
        builder.Property(i => i.RowVer)
            .IsConcurrencyToken()
            .HasDefaultValueSql("GETUTCDATE()");
    }
}