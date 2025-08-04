using Tamro.Madam.Models.General;
using Tamro.Madam.Repository.Context.Madam;
using TamroUtilities.EFCore.Models;

namespace Tamro.Madam.Repository.Entities.Customers;

public class CustomerLegalEntity : IMadamEntity<int>, IBaseEntity, IAuditable
{
    public int Id { get; set; }
    public BalticCountry Country { get; set; }
    public int E1SoldTo { get; set; }
    public bool IsActive { get; set; }
    public DateTime CreatedDate { get; set; }
    public DateTime RowVer { get; set; }

    public CustomerLegalEntityNotification NotificationSettings { get; set; }
}
