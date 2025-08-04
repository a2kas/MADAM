using MudBlazor;
using Tamro.Madam.Application.Models.Common;
using Tamro.Madam.Models.Commerce.ItemAssortmentSalesChannels;
using Tamro.Madam.Models.General;

namespace Tamro.Madam.Application.Queries.Commerce.ItemAssortmentSalesChannels;

public class ItemAssortmentSalesChannelFilter : PaginationFilter
{
    public ICollection<IFilterDefinition<ItemAssortmentSalesChannelGridModel>>? Filters { get; set; }
    public string SearchTerm { get; set; } = string.Empty;
    public BalticCountry? Country { get; set; }
}
