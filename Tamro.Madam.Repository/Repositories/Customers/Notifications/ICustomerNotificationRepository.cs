namespace Tamro.Madam.Repository.Repositories.Customers.Notifications;
public interface ICustomerNotificationRepository
{
    Task MarkSendCanceledOrderNotification(List<int> customerLegalEntityIds, bool value, CancellationToken cancellationToken);
}
