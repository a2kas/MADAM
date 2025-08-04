using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Tamro.Madam.Models.Sales.CanceledOrderLines;
using Tamro.Madam.Models.Sales.CanceledOrderLines.Statistics;
using Tamro.Madam.Repository.Entities.Sales.CanceledOrderLines;

namespace Tamro.Madam.Repository.Configuration.Sales.CanceledOrderLines;

internal class E1CanceledOrderLineConfiguration : IEntityTypeConfiguration<E1CanceledOrderLine>
{
    public void Configure(EntityTypeBuilder<E1CanceledOrderLine> builder)
    {
        builder.Property(x => x.EmailStatus)
            .HasMaxLength(15)
            .HasConversion(new EnumToStringConverter<CanceledOrderLineEmailStatus>());
        builder.Property(x => x.ItemNo2)
            .HasMaxLength(25);
        builder.Property(x => x.EmailAddress)
            .HasMaxLength(200);
        builder.Property(x => x.CancelationReason)
            .HasMaxLength(35)
            .HasConversion(new EnumToStringConverter<CancelationReason>());
    }
}
