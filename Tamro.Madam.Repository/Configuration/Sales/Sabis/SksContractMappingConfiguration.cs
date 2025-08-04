using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Tamro.Madam.Repository.Entities.Customers.E1;
using Tamro.Madam.Repository.Entities.Sales.Sabis;

namespace Tamro.Madam.Repository.Configuration.Sales.CanceledOrderLines;

internal class SksContractMappingConfiguration : IEntityTypeConfiguration<SksContractMapping>
{
    public void Configure(EntityTypeBuilder<SksContractMapping> builder)
    {
        builder.ToTable("SKS_Contract_mapping");
        builder.HasKey(x => x.Id);
        builder.Property(x => x.AdditionalTaxId)
            .HasMaxLength(20);
        builder.Property(x => x.ContractTamro)
            .HasMaxLength(150);
        builder.Property(x => x.ContractSabis)
            .HasMaxLength(150)
            .HasColumnName("ContractSabis");
        builder.Property(x => x.EditedBy)
            .HasMaxLength(50);

        builder.HasOne(x => x.Customer)
            .WithOne()
            .HasForeignKey<SksContractMapping>(s => s.AddressNumber)
            .HasPrincipalKey<Customer>(c => c.AddressNumber)
            .IsRequired(false);
    }
}
