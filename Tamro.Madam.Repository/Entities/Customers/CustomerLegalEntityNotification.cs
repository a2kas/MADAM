using Tamro.Madam.Repository.Context.Madam;
using TamroUtilities.EFCore.Models;

namespace Tamro.Madam.Repository.Entities.Customers;

public class CustomerLegalEntityNotification : IMadamEntity<int>, IBaseEntity, IAuditable
{
    public int Id { get; set; }
    public bool SendCanceledOrderNotification { get; set; }
    public DateTime CreatedDate { get; set; }
    public DateTime RowVer { get; set; }
    public int CustomerLegalEntityId { get; set; }
}
