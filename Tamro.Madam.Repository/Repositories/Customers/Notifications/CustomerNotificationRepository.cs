using Microsoft.EntityFrameworkCore;
using Tamro.Madam.Repository.Entities.Customers;
using Tamro.Madam.Repository.UnitOfWork;

namespace Tamro.Madam.Repository.Repositories.Customers.Notifications;
public class CustomerNotificationRepository : ICustomerNotificationRepository
{
    private readonly IMadamUnitOfWork _uow;

    public CustomerNotificationRepository(IMadamUnitOfWork uow)
    {
        _uow = uow;
    }
    public async Task MarkSendCanceledOrderNotification(List<int> customerLegalEntityIds, bool value, CancellationToken cancellationToken)
    {
        var customerIds = await _uow.GetRepository<Customer>()
            .AsQueryable()
            .Where(x => customerLegalEntityIds.Contains(x.CustomerLegalEntityId))
            .Select(e=>e.Id)
            .ToListAsync(cancellationToken);

        var notifications = await _uow.GetRepository<CustomerNotification>()
            .AsQueryable()
            .Where(x=> customerIds.Contains(x.CustomerId))
            .ToListAsync(cancellationToken);

        if (notifications.Any())
        {
            notifications.ForEach(x => x.SendCanceledOrderNotification = value);
        }

        await _uow.SaveChangesAsync(cancellationToken);
    }
}
