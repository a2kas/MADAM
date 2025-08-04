using MudBlazor;
using Tamro.Madam.Application.Models.Common;
using Tamro.Madam.Models.General;
using Tamro.Madam.Models.Sales.CanceledOrderLines.ExcludedCustomers;

namespace Tamro.Madam.Application.Queries.Sales.CanceledOrderLines.ExcludedCustomers;

public class ExcludedCustomersFilter : PaginationFilter
{
    public ICollection<IFilterDefinition<ExcludedCustomerGridModel>>? Filters { get; set; }
    public BalticCountry? Country { get; set; }
}
