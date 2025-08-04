using MudBlazor;
using Tamro.Madam.Application.Models.Common;
using Tamro.Madam.Models.Hr.Jira.Administration;

namespace Tamro.Madam.Application.Queries.Hr.Jira.Administration;
public class JiraAccountFilter : PaginationFilter
{
    public ICollection<IFilterDefinition<JiraAccountModel>>? Filters { get; set; }
    public HashSet<string>? Teams { get; set; }
    public HashSet<string>? IsActiveFilter { get; set; } = ["true"];
}
