using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Tamro.Madam.Models.Finance.Peppol;
using Tamro.Madam.Repository.Entities.Finance.Peppol;

namespace Tamro.Madam.Repository.Configuration.Finance.Peppol;

public class PeppolInvoiceConfiguration : IEntityTypeConfiguration<PeppolInvoice>
{
    public void Configure(EntityTypeBuilder<PeppolInvoice> builder)
    {
        builder.ToTable("v_PeppolInvoice");
        builder.HasKey(x => x.Id);
        builder.Property(x => x.CustomerName)
            .IsRequired()
            .HasMaxLength(40);
        builder.Property(x => x.Status)
            .HasMaxLength(10)
            .HasConversion(new EnumToStringConverter<PeppolInvoiceStatus>())
            .HasDefaultValue(PeppolInvoiceStatus.NotSent);
        builder.Property(x => x.TotalInvoiceTaxAmount)
            .HasPrecision(15, 2);
        builder.Property(x => x.TotalInvoiceAmountWithTax)
            .HasPrecision(15, 2);
        builder.Property(x => x.TotalInvoiceAmountWithoutTax)
            .HasPrecision(15, 2);
        builder.Property(x => x.Type)
            .HasMaxLength(15)
            .HasConversion(new EnumToStringConverter<PeppolInvoiceType>())
            .HasDefaultValue(PeppolInvoiceType.Regular);
        builder.HasMany(x => x.Invoices)
            .WithOne()
            .HasForeignKey(x => x.ConsolidationNumber)
            .HasPrincipalKey(x => x.ConsolidationNumber);
    }
}
