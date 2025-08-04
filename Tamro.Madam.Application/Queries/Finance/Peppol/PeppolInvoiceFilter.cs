using MudBlazor;
using Tamro.Madam.Application.Models.Common;
using Tamro.Madam.Models.Finance.Peppol;

namespace Tamro.Madam.Application.Queries.Finance.Peppol;

public class PeppolInvoiceFilter : PaginationFilter
{
    public ICollection<IFilterDefinition<PeppolInvoiceGridModel>>? Filters { get; set; }
    public string SearchTerm { get; set; } = string.Empty;
    public HashSet<PeppolInvoiceStatus>? Status { get; set; }
    public HashSet<string>? Types {  get; set; }
}
