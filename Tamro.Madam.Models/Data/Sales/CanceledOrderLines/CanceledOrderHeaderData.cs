using System.Collections.Immutable;
using Tamro.Madam.Models.Sales.CanceledOrderLines;
using Tamro.Madam.Models.Sales.CanceledOrderLines.Statistics;

namespace Tamro.Madam.Models.Data.Sales.CanceledOrderLines;

public static class CanceledOrderHeaderData
{
    public static ImmutableDictionary<string, string> EmailNotificationStatus { get; } = ImmutableDictionary<string, string>.Empty
       .Add(CanceledOrderHeaderEmailStatus.NotSent.ToString(), "Not sent")
       .Add(CanceledOrderHeaderEmailStatus.Sent.ToString(), "Sent")
       .Add(CanceledOrderHeaderEmailStatus.FailureSending.ToString(), "Failure sending")
       .Add(CanceledOrderHeaderEmailStatus.WillNotBeSent.ToString(), "Will not be sent")
       .Add(CanceledOrderHeaderEmailStatus.PartiallySent.ToString(), "Partially sent");

    public static ImmutableDictionary<string, string> CancellationReasons { get; } = ImmutableDictionary<string, string>.Empty
        .Add(CancelationReason.E1Canceled.ToString(), "(E1) Canceled")
        .Add(CancelationReason.SafetyStockOutOfStock.ToString(), "(Safety stock) Out of stock")
        .Add(CancelationReason.SafetyStockCanceledPartially.ToString(), "(Safety stock) Canceled partially")
        .Add(CancelationReason.SafetyStockRestricted.ToString(), "(Safety stock) Restricted for purchase");
}
