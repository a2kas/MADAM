using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TamroUtilities.EFCore.Models;

namespace Tamro.Madam.Repository.Configuration.Audit;

internal class DbAuditEntryConfiguration : IEntityTypeConfiguration<DbAuditEntry>
{
    public void Configure(EntityTypeBuilder<DbAuditEntry> builder)
    {
        builder
            .Property(b => b.CreatedDate)
            .HasConversion(
                v => v.ToUniversalTime(),
                v => DateTime.SpecifyKind(v, DateTimeKind.Utc)
            );
    }
}
