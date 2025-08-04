using Tamro.Madam.Application.Models.Common;

namespace Tamro.Madam.Application.Queries.Items;

public class CategoryManagerFilter : PaginationFilter
{
    public ICollection<FilterDefinition> Filters { get; set; } = new List<FilterDefinition>();
    public string? SearchTerm { get; set; }
    public HashSet<string>? Countries { get; set; }
}
