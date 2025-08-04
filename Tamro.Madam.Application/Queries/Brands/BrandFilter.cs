using MudBlazor;
using Tamro.Madam.Application.Models.Common;
using Tamro.Madam.Models.ItemMasterdata.Brands;

namespace Tamro.Madam.Application.Queries.Brands;

public class BrandFilter : PaginationFilter
{
    public ICollection<IFilterDefinition<BrandModel>>? Filters { get; set; }
    public string SearchTerm { get; set; } = string.Empty;
}
