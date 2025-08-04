using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Tamro.Madam.Repository.Entities.Suppliers;

namespace Tamro.Madam.Repository.Configuration.Suppliers;

internal class SupplierContractConfiguration : IEntityTypeConfiguration<SupplierContract>
{
    public void Configure(EntityTypeBuilder<SupplierContract> builder)
    {
        builder.ToTable(nameof(SupplierContract));
        builder.Property(x => x.RowVer)
            .IsConcurrencyToken();
        builder.Property(x => x.DocumentReference)
            .HasMaxLength(100);
    }
}