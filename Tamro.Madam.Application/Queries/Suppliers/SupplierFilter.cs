using MudBlazor;
using Tamro.Madam.Application.Models.Common;
using Tamro.Madam.Models.General;
using Tamro.Madam.Models.Suppliers;

namespace Tamro.Madam.Application.Queries.Suppliers;

public class SupplierFilter : PaginationFilter
{
    public ICollection<IFilterDefinition<SupplierGridModel>>? Filters { get; set; }
    public BalticCountry? Country { get; set; }
    public string SearchTerm { get; set; }
}
