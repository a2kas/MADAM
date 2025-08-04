using System.Collections.Immutable;
using Tamro.Madam.Models.Sales.HeldOrders;

namespace Tamro.Madam.Models.Data.Sales.HeldOrders;
public class HeldOrderData
{
    public static ImmutableDictionary<string, string> NotificationStatus { get; } = ImmutableDictionary<string, string>.Empty
   .Add(E1HeldNotificationStatusModel.NotSent.ToString(), "Not sent")
   .Add(E1HeldNotificationStatusModel.Sent.ToString(), "Sent")
   .Add(E1HeldNotificationStatusModel.FailureSending.ToString(), "Failure sending")
   .Add(E1HeldNotificationStatusModel.WillNotBeSent.ToString(), "Will not be sent");
}
