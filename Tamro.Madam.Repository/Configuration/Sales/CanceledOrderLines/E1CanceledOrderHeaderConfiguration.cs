using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Tamro.Madam.Models.General;
using Tamro.Madam.Repository.Entities.Sales.CanceledOrderLines;

namespace Tamro.Madam.Repository.Configuration.Sales.CanceledOrderLines;

internal class E1CanceledOrderHeaderConfiguration : IEntityTypeConfiguration<E1CanceledOrderHeader>
{
    public void Configure(EntityTypeBuilder<E1CanceledOrderHeader> builder)
    {
        builder.Property(x => x.Country)
            .HasMaxLength(2)
            .HasConversion(new EnumToStringConverter<BalticCountry>());
        builder.HasMany(x => x.Lines)
            .WithOne()
            .HasForeignKey(x => x.E1CanceledOrderHeaderId);
        builder.HasKey(x => x.Id);
        builder.Property(x => x.OrderDate)
            .HasColumnType("date");
        builder.Property(x => x.CustomerOrderNo)
            .HasMaxLength(25);
        builder.Property(x => x.DocumentNo)
            .HasMaxLength(15);
    }
}
