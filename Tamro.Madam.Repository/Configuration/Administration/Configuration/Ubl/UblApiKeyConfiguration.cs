using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Tamro.Madam.Repository.Entities.Administration.Configuration.Ubl;

namespace Tamro.Madam.Repository.Configuration.Administration.Configuration.Ubl;
public class UblApiKeyConfiguration : IEntityTypeConfiguration<UblApiKey>
{
    public void Configure(EntityTypeBuilder<UblApiKey> builder)
    {
        builder.ToTable(nameof(UblApiKey));
        builder.HasKey(x => x.E1SoldTo);
        builder.Property(x => x.CustomerName).HasMaxLength(40);
    }
}
