using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Tamro.Madam.Repository.Entities.Customers.Sks;

namespace Tamro.Madam.Repository.Configuration.Customers.Sks;

internal class CustomerSubscriptionConfiguration : IEntityTypeConfiguration<OrderNotificationEmail>
{
    public void Configure(EntityTypeBuilder<OrderNotificationEmail> builder)
    {
        builder.HasKey(x => x.AddressNumber);
    }
}
