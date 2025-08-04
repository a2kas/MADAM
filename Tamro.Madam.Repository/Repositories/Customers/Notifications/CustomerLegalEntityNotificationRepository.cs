using Microsoft.EntityFrameworkCore;
using Tamro.Madam.Repository.Entities.Customers;
using Tamro.Madam.Repository.UnitOfWork;

namespace Tamro.Madam.Repository.Repositories.Customers.Notifications;

public class CustomerLegalEntityNotificationRepository : ICustomerLegalEntityNotificationRepository
{
    private readonly IMadamUnitOfWork _uow;

    public CustomerLegalEntityNotificationRepository(IMadamUnitOfWork uow)
    {
        _uow = uow;
    }

    public async Task MarkSendCanceledOrderNotification(List<int> customerLegalEntityIds, bool value, CancellationToken cancellationToken)
    {
        var notifications = await _uow.GetRepository<CustomerLegalEntityNotification>()
            .AsQueryable()
            .Where(x => customerLegalEntityIds.Contains(x.CustomerLegalEntityId))
            .ToListAsync(cancellationToken);

        if (notifications.Any())
        {
            notifications.ForEach(x => x.SendCanceledOrderNotification = value);
        }

        await _uow.SaveChangesAsync(cancellationToken);
    }
}
