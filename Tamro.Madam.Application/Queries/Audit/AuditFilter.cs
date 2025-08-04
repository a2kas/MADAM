using MudBlazor;
using Tamro.Madam.Application.Models.Common;
using Tamro.Madam.Models.Audit;

namespace Tamro.Madam.Application.Queries.Audit;

public class AuditFilter : PaginationFilter
{
    public ICollection<IFilterDefinition<AuditGridModel>>? Filters { get; set; }
    public string? SearchTerm { get; set; }
    public string? EntityId { get; set; }
    public string? EntityTypeName { get; set; }
}
