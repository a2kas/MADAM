using MudBlazor;
using Tamro.Madam.Application.Models.Common;
using Tamro.Madam.Models.ItemMasterdata.Items.Bindings.Vlk;

namespace Tamro.Madam.Application.Queries.Items.Bindings.Vlk;

public class VlkBindingFilter : PaginationFilter
{
    public ICollection<IFilterDefinition<VlkBindingGridModel>>? Filters { get; set; }
    public string? SearchTerm { get; set; }
}
