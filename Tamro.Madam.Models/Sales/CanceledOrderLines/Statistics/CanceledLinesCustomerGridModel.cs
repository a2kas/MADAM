using System.ComponentModel;

namespace Tamro.Madam.Models.Sales.CanceledOrderLines.Statistics;

public class CanceledLinesCustomerGridModel
{
    [DisplayName("Customer name")]
    public string CustomerName { get; set; }
    [DisplayName("Customer ship to")]
    public string E1ShipTo { get; set; }
    [DisplayName("Customer total canceled quantity")]
    public int CanceledQuantity { get; set; }
    [DisplayName("Cancelation reason")]
    public CancelationReason CancelationReason { get; set; }
}
