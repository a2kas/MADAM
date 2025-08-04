using MudBlazor;
using Tamro.Madam.Application.Models.Common;
using Tamro.Madam.Models.ItemMasterdata.Requestors;

namespace Tamro.Madam.Application.Queries.Requestors;

public class RequestorFilter: PaginationFilter
{
    public ICollection<IFilterDefinition<RequestorModel>>? Filters { get; set; }
    public string SearchTerm { get; set; }
}
