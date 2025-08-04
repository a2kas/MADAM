using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Tamro.Madam.Repository.Entities.Users.UserSettings;

namespace Tamro.Madam.Repository.Configuration.User.UserSettings;

internal class UserSettingConfiguration : IEntityTypeConfiguration<UserSetting>
{
    public void Configure(EntityTypeBuilder<UserSetting> builder)
    {
        builder.ToTable("UserSettings");
        builder.Property(us => us.Id).HasColumnName("UserName").HasMaxLength(100);
    }
}
