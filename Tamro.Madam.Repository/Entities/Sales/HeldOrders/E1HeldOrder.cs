using Tamro.Madam.Models.General;
using Tamro.Madam.Models.Sales.HeldOrders;
using TamroUtilities.EFCore.Models;

namespace Tamro.Madam.Repository.Entities.Sales.HeldOrders;
public class E1HeldOrder : IBaseEntity
{
    public int Id { get; set; }
    public BalticCountry Country { get; set; }
    public DateTime OrderDate { get; set; }
    public int DocumentNo { get; set; }
    public int E1ShipTo { get; set; }
    public E1HeldNotificationStatusModel NotificationStatus { get; set; }
    public string? Email { get; set; }
    public DateTime? NotificationSendDate { get; set; }
    public DateTime CreatedDate { get; set; }
    public DateTime RowVer { get; set; }
}
