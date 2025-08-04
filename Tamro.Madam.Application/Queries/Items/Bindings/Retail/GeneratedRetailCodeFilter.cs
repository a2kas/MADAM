using MudBlazor;
using Tamro.Madam.Application.Models.Common;
using Tamro.Madam.Models.ItemMasterdata.Items.Bindings.Retail;

namespace Tamro.Madam.Application.Queries.Items.Bindings.Retail;

public class GeneratedRetailCodeFilter : PaginationFilter
{
    public ICollection<IFilterDefinition<GeneratedRetailCodeModel>>? Filters { get; set; }
    public string? SearchTerm { get; set; }
}
