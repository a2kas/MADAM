using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Tamro.Madam.Repository.Entities.Wholesale.Lv;

namespace Tamro.Madam.Repository.Configuration.Customers.Wholesale.Lv;
internal class LvWholesaleEmployeeConfiguration : IEntityTypeConfiguration<LvWholesaleEmployee>
{
    public void Configure(EntityTypeBuilder<LvWholesaleEmployee> builder)
    {
        builder.HasNoKey();
    }
}