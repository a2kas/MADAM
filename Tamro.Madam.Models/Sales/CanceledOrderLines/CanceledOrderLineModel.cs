using Tamro.Madam.Models.Sales.CanceledOrderLines.Statistics;

namespace Tamro.Madam.Models.Sales.CanceledOrderLines;

public class CanceledOrderLineModel : IItemDetailsModel
{
    public int Id { get; set; }
    public int OrderLineNo { get; set; }
    public string ItemNo { get; set; }
    public string ItemName { get; set; }
    public int OrderedQuantity { get; set; }
    public int CanceledQuantity { get; set; }
    public int LastStatus { get; set; }
    public CanceledOrderLineEmailStatus? EmailStatus { get; set; }
    public DateTime? NotificationSendDate { get; set; }
    public string EmailAddress { get; set; }
    public CancelationReason CancelationReason { get; set; }
    public DateTime CreatedDate { get; set; }
    public DateTime RowVer { get; set; }

    public int E1CanceledOrderHeaderId { get; set; }
}
