namespace Tamro.Madam.Models.Customers;

public class CustomerLegalEntityModel
{
    public int Id { get; set; }
    public int E1SoldTo { get; set; }
    public bool IsActive { get; set; }
    public CustomerLegalEntityNotificationModel NotificationSettings { get; set; }
}
