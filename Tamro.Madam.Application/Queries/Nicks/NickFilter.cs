using MudBlazor;
using Tamro.Madam.Application.Models.Common;
using Tamro.Madam.Models.ItemMasterdata.Nicks;

namespace Tamro.Madam.Application.Queries.Nicks;

public class NickFilter : PaginationFilter
{
    public ICollection<IFilterDefinition<NickModel>>? Filters { get; set; }
    public string SearchTerm { get; set; }
}
