using Tamro.Madam.Models.General;

namespace Tamro.Madam.Models.Sales.HeldOrders;
public class E1HeldOrderModel
{
    public int Id { get; set; }
    public BalticCountry Country { get; set; }
    public DateTime OrderDate { get; set; }
    public int DocumentNo { get; set; }
    public int E1ShipTo { get; set; }
    public E1HeldNotificationStatusModel NotificationStatus { get; set; }
    public string Email { get; set; }
    public DateTime? NotificationSendDate { get; set; }
    public DateTime CreatedDate { get; set; }
    public DateTime RowVer { get; set; }

    public E1HeldNotificationStatusModel OldNotificationStatus { get; set; }
    public DateTime? OldNotificationSendDate { get; set; }
    public string MailingName { get; set; }
    public int ResponsibleEmployeeNumber { get; set; }
    public string EmployeesEmail { get; set; }
    public bool HasValidCustomerEmails { get; set; }
    public bool HasValidEmployeeEmails { get; set; }
}
