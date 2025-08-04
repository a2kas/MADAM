using System.ComponentModel;

namespace Tamro.Madam.Models.Sales.CanceledOrderLines;

public class CanceledOrderHeaderGridModel : ISalesOrderHeader
{
    public int Id { get; set; }
    [DisplayName("Order date")]
    public DateTime OrderDate { get; set; }
    [DisplayName("Customer ship to")]
    public int E1ShipTo { get; set; }
    [DisplayName("Customer name")]
    public string CustomerName { get; set; }
    [DisplayName("Customer order No.")]
    public string CustomerOrderNo { get; set; }
    [DisplayName("E1 order No.")]
    public string DocumentNo { get; set; }
    [DisplayName("Notification status")]
    public CanceledOrderHeaderEmailStatus EmailStatus
    {
        get
        {
            if (Lines?.Any() != true)
            {
                return CanceledOrderHeaderEmailStatus.Sent;
            }

            if (Lines.All(line => line.EmailStatus == Lines[0].EmailStatus))
            {
                return (CanceledOrderHeaderEmailStatus)(int)Lines[0].EmailStatus;
            }
            else if (Lines.Any(line => line.EmailStatus == CanceledOrderLineEmailStatus.Sent))
            {
                return CanceledOrderHeaderEmailStatus.PartiallySent;
            }
            else if (Lines.Any(line => line.EmailStatus == CanceledOrderLineEmailStatus.NotSent) && Lines.Any(line => line.EmailStatus == CanceledOrderLineEmailStatus.FailureSending))
            {
                return CanceledOrderHeaderEmailStatus.NotSent;
            }

            return CanceledOrderHeaderEmailStatus.NotSent;
        }
    }

    public List<CanceledOrderLineGridModel> Lines { get; set; }
}
