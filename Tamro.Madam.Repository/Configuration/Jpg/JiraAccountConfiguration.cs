using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Tamro.Madam.Models.Hr.Jira.Administration;
using Tamro.Madam.Repository.Entities.Jpg;

namespace Tamro.Madam.Repository.Configuration.Jpg;
public class JiraAccountConfiguration : IEntityTypeConfiguration<JiraAccount>
{
    public void Configure(EntityTypeBuilder<JiraAccount> builder)
    {
        builder.ToTable("Jira2Account");

        builder.HasKey(e => e.Id);

        builder.Property(e => e.DisplayName)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(e => e.Team)
            .HasConversion(new EnumToStringConverter<JiraAccountTeam>())
            .HasMaxLength(20)
            .IsRequired(false);

        builder.Property(e => e.IsActive)
            .IsRequired();
    }
}
