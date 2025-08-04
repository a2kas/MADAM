namespace Tamro.Madam.Models.Sales.CanceledOrderLines.Statistics;
public class CanceledLineStatisticModel : IItemDetailsModel, ISalesOrderHeader
{
    public string ItemNo { get; set; }
    public string ItemName { get; set; }
    public int E1ShipTo { get; set; }
    public string CustomerName { get; set; }
    public int CanceledQuantity { get; set; }
    public DateTime CreatedDate { get; set; }
    public CancelationReason CancelationReason { get; set; }
}
