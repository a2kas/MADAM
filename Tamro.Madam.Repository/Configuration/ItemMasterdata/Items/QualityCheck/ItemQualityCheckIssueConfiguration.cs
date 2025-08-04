using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Tamro.Madam.Models.General;
using Tamro.Madam.Models.ItemMasterdata.Items.QualityCheck;
using Tamro.Madam.Repository.Entities.ItemMasterdata.Items.QualityCheck;

internal class ItemQualityCheckIssueConfiguration : IEntityTypeConfiguration<ItemQualityCheckIssue>
{
    public void Configure(EntityTypeBuilder<ItemQualityCheckIssue> builder)
    {
        builder.ToTable(nameof(ItemQualityCheckIssue));
        builder.HasKey(x => x.Id);
        builder.Property(x => x.IssueEntity)
            .HasMaxLength(30);
        builder.Property(x => x.IssueField)
            .HasMaxLength(30);
        builder.Property(x => x.IssueSeverity)
            .HasConversion(new EnumToStringConverter<ItemQualityIssueSeverity>())
            .HasMaxLength(20)
            .HasDefaultValue(ItemQualityIssueSeverity.Low);
        builder.Property(x => x.IssueStatus)
            .HasConversion(new EnumToStringConverter<ItemQualityIssueStatus>())
            .HasMaxLength(20)
            .HasDefaultValue(ItemQualityIssueStatus.New);
        builder.Property(x => x.Description)
            .HasMaxLength(4000);
        builder.Property(x => x.ActualValue)
            .HasMaxLength(4000);
        builder.Property(x => x.ExpectedValue)
            .HasMaxLength(4000);
        builder.Property(x => x.Country)
            .HasConversion(new EnumToStringConverter<BalticCountry>())
            .HasMaxLength(2);
        builder.Property(i => i.CreatedDate)
            .HasDefaultValueSql("GETUTCDATE()");
        builder.Property(i => i.RowVer)
            .IsConcurrencyToken()
            .HasDefaultValueSql("GETUTCDATE()");

        builder
            .HasOne(i => i.Item)
            .WithMany()
            .OnDelete(DeleteBehavior.NoAction);
        builder
            .HasOne(i => i.ItemBinding)
            .WithMany()
            .IsRequired(false)
            .OnDelete(DeleteBehavior.NoAction);
        builder
            .HasOne(i => i.ItemQualityCheck)
            .WithMany(i => i.ItemQualityCheckIssues)
            .OnDelete(DeleteBehavior.Cascade);
    }
}