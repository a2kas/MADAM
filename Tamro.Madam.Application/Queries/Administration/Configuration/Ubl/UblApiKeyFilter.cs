using MudBlazor;
using Tamro.Madam.Application.Models.Common;
using Tamro.Madam.Models.Administration.Configuration.Ubl;

namespace Tamro.Madam.Application.Queries.Administration.Configuration.Ubl;
public class UblApiKeyFilter : PaginationFilter
{
    public ICollection<IFilterDefinition<UblApiKeyModel>>? Filters { get; set; }
    public string SearchTerm { get; set; } = string.Empty;
}
