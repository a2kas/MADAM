using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Tamro.Madam.Models.General;
using Tamro.Madam.Models.Sales.HeldOrders;
using Tamro.Madam.Repository.Entities.Sales.HeldOrders;

namespace Tamro.Madam.Repository.Configuration.Sales.HeldOrders;
internal class E1HeldOrderConfiguration : IEntityTypeConfiguration<E1HeldOrder>
{
    public void Configure(EntityTypeBuilder<E1HeldOrder> builder)
    {
        builder.Property(x => x.Country)
            .HasMaxLength(2)
            .HasConversion(new EnumToStringConverter<BalticCountry>());
        builder.Property(x => x.NotificationStatus)
            .HasMaxLength(15)
            .HasConversion(new EnumToStringConverter<E1HeldNotificationStatusModel>());
    }
}
