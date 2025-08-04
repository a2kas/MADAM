using System.ComponentModel;

namespace Tamro.Madam.Models.Sales.CanceledOrderLines.Statistics;

public class CanceledLinesItemGridModel
{
    [DisplayName("Item No.")]
    public string ItemNo { get; set; }
    [DisplayName("Item Name")]
    public string ItemName { get; set; }
    [DisplayName("Total canceled quantity")]
    public int CanceledQuantity { get; set; }
    [DisplayName("Cancelation reason")]
    public CancelationReason CancelationReason { get; set; }
}
