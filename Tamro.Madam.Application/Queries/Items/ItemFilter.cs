using MudBlazor;
using Tamro.Madam.Application.Models.Common;
using Tamro.Madam.Models.ItemMasterdata.Items;

namespace Tamro.Madam.Application.Queries.Items;

public class ItemFilter : PaginationFilter
{
    public ICollection<IFilterDefinition<ItemGridModel>>? Filters { get; set; }
    public string? SearchTerm { get; set; }
}
