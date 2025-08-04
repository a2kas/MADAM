using Tamro.Madam.Models.Sales.CanceledOrderLines.Statistics;

namespace Tamro.Madam.Repository.Entities.Sales.CanceledOrderLines;
public class CanceledLineStatistic
{
    public string ItemNo2 { get; set; }
    public int E1ShipTo { get; set; }
    public int CanceledQuantity { get; set; }
    public DateTime CreatedDate { get; set; }
    public CancelationReason CancelationReason { get; set; }
}
