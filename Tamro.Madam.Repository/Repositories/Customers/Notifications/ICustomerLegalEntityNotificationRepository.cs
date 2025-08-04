namespace Tamro.Madam.Repository.Repositories.Customers.Notifications;

public interface ICustomerLegalEntityNotificationRepository
{
    Task MarkSendCanceledOrderNotification(List<int> customerLegalEntityIds, bool value, CancellationToken cancellationToken);
}
