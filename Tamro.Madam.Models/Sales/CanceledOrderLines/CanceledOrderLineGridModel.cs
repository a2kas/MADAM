using System.ComponentModel;
using Tamro.Madam.Models.Sales.CanceledOrderLines.Statistics;

namespace Tamro.Madam.Models.Sales.CanceledOrderLines;

public class CanceledOrderLineGridModel : IItemDetailsModel
{
    [DisplayName("Order line")]
    public int OrderLineNo { get; set; }
    [DisplayName("Item No.")]
    public string ItemNo { get; set; }
    [DisplayName("Item name")]
    public string ItemName { get; set; }
    [DisplayName("Ordered quantity")]
    public int OrderedQuantity { get; set; }
    [DisplayName("Canceled quantity")]
    public int CanceledQuantity { get; set; }
    [DisplayName("Cancelation reason")]
    public CancelationReason CancelationReason { get; set; }
    [DisplayName("Notification status")]
    public CanceledOrderLineEmailStatus EmailStatus { get; set; }
    [DisplayName("Notification send date")]
    public DateTime? NotificationSendDate { get; set; }
    [DisplayName("E-mail address(es)")]
    public string EmailAddress { get; set; }
}
