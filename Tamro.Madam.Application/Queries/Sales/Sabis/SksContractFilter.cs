using MudBlazor;
using Tamro.Madam.Application.Models.Common;
using Tamro.Madam.Models.Sales.Sabis;

namespace Tamro.Madam.Application.Queries.Sales.Sabis;

public class SksContractFilter : PaginationFilter
{
    public ICollection<IFilterDefinition<SksContractGridModel>>? Filters { get; set; }
    public string SearchTerm { get; set; } = string.Empty;
}