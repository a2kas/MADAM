using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Tamro.Madam.Repository.Entities.Atcs;

namespace Tamro.Madam.Repository.Configuration.ItemMasterdata.Items
{
    public class AtcConfiguration : IEntityTypeConfiguration<Atc>
    {
        public void Configure(EntityTypeBuilder<Atc> builder)
        {
            builder.ToTable("ATC");
        }
    }
}
