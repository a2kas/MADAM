using MudBlazor;
using Tamro.Madam.Application.Models.Common;
using Tamro.Madam.Models.ItemMasterdata.Items.QualityCheck;

namespace Tamro.Madam.Application.Queries.Items.QualityCheck;

public class ItemQualityCheckFilter : PaginationFilter
{
    public ICollection<IFilterDefinition<ItemQualityCheckGridModel>>? Filters { get; set; }
    public HashSet<ItemQualityIssueSeverity> UnresolvedSeverities { get; set; }
    public HashSet<ItemQualityIssueStatus> Statuses { get; set; }
    public string? SearchTerm { get; set; }
}
