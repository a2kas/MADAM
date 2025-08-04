using Tamro.Madam.Models.Sales.HeldOrders;

namespace Tamro.Madam.Application.Services.Sales.HeldOrders.Resolvers;
public class E1HeldOrdersEmailStatusResolver : IE1HeldOrdersEmailStatusResolver
{
    private readonly TimeProvider _timeProvider;

    public E1HeldOrdersEmailStatusResolver(TimeProvider timeProvider)
    {
        _timeProvider = timeProvider ?? throw new ArgumentNullException(nameof(timeProvider));
    }

    public void ResolveNotifcationStatus(IEnumerable<E1HeldOrderModel> orders)
    {
        foreach (var order in orders)
        {
            if (!order.HasValidCustomerEmails)
            {
                if (order.OldNotificationStatus == E1HeldNotificationStatusModel.FailureSending && order.CreatedDate.AddDays(5) < _timeProvider.GetUtcNow().DateTime)
                {
                    order.NotificationStatus = E1HeldNotificationStatusModel.WillNotBeSent;
                    continue;
                }

                order.NotificationStatus = E1HeldNotificationStatusModel.FailureSending;
            }
        }
    }
}
