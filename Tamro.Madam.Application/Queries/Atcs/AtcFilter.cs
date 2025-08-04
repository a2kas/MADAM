using MudBlazor;
using Tamro.Madam.Application.Models.Common;
using Tamro.Madam.Models.ItemMasterdata.Atcs;

namespace Tamro.Madam.Application.Queries.Atcs;

public class AtcFilter : PaginationFilter
{
    public ICollection<IFilterDefinition<AtcModel>>? Filters { get; set; }
    public string SearchTerm { get; set; } = string.Empty;
}
