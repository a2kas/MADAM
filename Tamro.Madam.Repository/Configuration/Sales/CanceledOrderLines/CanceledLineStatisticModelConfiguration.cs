using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Tamro.Madam.Models.Sales.CanceledOrderLines.Statistics;
using Tamro.Madam.Repository.Entities.Sales.CanceledOrderLines;

namespace Tamro.Madam.Repository.Configuration.Sales.CanceledOrderLines;
public class CanceledLineStatisticModelConfiguration : IEntityTypeConfiguration<CanceledLineStatistic>
{
    public void Configure(EntityTypeBuilder<CanceledLineStatistic> builder)
    {
        builder.HasNoKey();
        builder.Property(x => x.CancelationReason)
            .HasMaxLength(35)
            .HasConversion(new EnumToStringConverter<CancelationReason>());
    }
}
