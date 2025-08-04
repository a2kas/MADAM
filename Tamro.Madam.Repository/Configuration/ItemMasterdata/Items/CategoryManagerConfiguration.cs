using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Tamro.Madam.Models.General;
using Tamro.Madam.Repository.Entities.ItemMasterdata.CategoryManagers;

namespace Tamro.Madam.Repository.Configuration.ItemMasterdata.Items;

public class CategoryManagerConfiguration : IEntityTypeConfiguration<CategoryManager>
{
    public void Configure(EntityTypeBuilder<CategoryManager> builder)
    {
        builder.ToTable("ItemCategoryManager");

        builder.HasKey(e => e.Id);

        builder.Property(e => e.EmailAddress)
            .IsRequired()
            .HasMaxLength(50);
        builder.HasIndex(e => e.EmailAddress)
        .IsUnique();

        builder.Property(e => e.FirstName)
            .IsRequired()
            .HasMaxLength(30);

        builder.Property(e => e.LastName)
            .IsRequired()
            .HasMaxLength(30);

        builder.Property(e => e.Country)
            .IsRequired()
            .HasConversion(new EnumToStringConverter<BalticCountry>())
            .HasMaxLength(2);

        builder.Property(i => i.CreatedDate)
            .HasDefaultValueSql("GETUTCDATE()");
        builder.Property(i => i.RowVer)
            .IsConcurrencyToken()
            .HasDefaultValueSql("GETUTCDATE()");
    }
}