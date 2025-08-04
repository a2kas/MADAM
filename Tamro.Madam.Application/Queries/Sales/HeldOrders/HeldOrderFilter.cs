using MudBlazor;
using Tamro.Madam.Application.Models.Common;
using Tamro.Madam.Models.General;
using Tamro.Madam.Models.Sales.HeldOrders;

namespace Tamro.Madam.Application.Queries.Sales.HeldOrders;
public class HeldOrderFilter : PaginationFilter
{
    public ICollection<IFilterDefinition<HeldOrderGridModel>>? Filters { get; set; }
    public BalticCountry? Country { get; set; }
    public DateTime? OrderDateFrom { get; set; }
    public DateTime? OrderDateTo { get; set; }
    public string SearchTerm { get; set; } = string.Empty;
    public HashSet<E1HeldNotificationStatusModel> NotificationStatus { get; set; }
}
