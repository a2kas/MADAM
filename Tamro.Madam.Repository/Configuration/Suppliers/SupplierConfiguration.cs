using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Tamro.Madam.Models.General;
using Tamro.Madam.Repository.Entities.Suppliers;

namespace Tamro.Madam.Repository.Configuration.Suppliers;

internal class SupplierConfiguration : IEntityTypeConfiguration<Supplier>
{
    public void Configure(EntityTypeBuilder<Supplier> builder)
    {
        builder.ToTable(nameof(Supplier));
        builder.Property(x => x.RegistrationNumber)
            .HasMaxLength(20);
        builder.Property(x => x.Name)
            .HasMaxLength(200);
        builder.Property(x => x.Country)
            .HasConversion(new EnumToStringConverter<BalticCountry>())
            .HasMaxLength(2);
        builder.Property(x => x.RowVer)
            .IsConcurrencyToken();
        builder.HasMany(x => x.Contracts)
            .WithOne()
            .HasForeignKey(x => x.SupplierId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
