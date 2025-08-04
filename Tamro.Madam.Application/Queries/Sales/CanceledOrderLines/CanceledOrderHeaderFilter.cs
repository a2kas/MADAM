using MudBlazor;
using Tamro.Madam.Application.Models.Common;
using Tamro.Madam.Models.General;
using Tamro.Madam.Models.Sales.CanceledOrderLines;

namespace Tamro.Madam.Application.Queries.Sales.CanceledOrderLines;

public class CanceledOrderHeaderFilter : PaginationFilter
{
    public ICollection<IFilterDefinition<CanceledOrderHeaderGridModel>>? Filters { get; set; }
    public BalticCountry? Country { get; set; }
    public DateTime? OrderDateFrom { get; set; }
    public DateTime? OrderDateTo { get; set; }
    public string SearchTerm { get; set; } = string.Empty;
    public HashSet<CanceledOrderHeaderEmailStatus>? EmailStatus { get; set; }
}
