using System.ComponentModel;

namespace Tamro.Madam.Models.Sales.HeldOrders;
public class HeldOrderGridModel : ISalesOrderHeader
{
    public int Id { get; set; }
    [DisplayName("Order date")]
    public DateTime OrderDate { get; set; }
    [DisplayName("Customer ship to")]
    public int E1ShipTo { get; set; }
    [DisplayName("Customer name")]
    public string CustomerName { get; set; }
    [DisplayName("E1 order No.")]
    public string DocumentNo { get; set; }
    [DisplayName("Notification status")]
    public E1HeldNotificationStatusModel NotificationStatus { get; set; }
    [DisplayName("Notification send date")]
    public DateTime? NotificationSendDate { get; set; }
    [DisplayName("Notification e-mail")]
    public string Email { get; set; }
}
