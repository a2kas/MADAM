using MudBlazor;
using Tamro.Madam.Application.Models.Common;
using Tamro.Madam.Models.General;
using Tamro.Madam.Models.ItemMasterdata.Items.SafetyStocks;

namespace Tamro.Madam.Application.Queries.Items.SafetyStocks;

public class SafetyStockFilter : PaginationFilter
{
    public ICollection<IFilterDefinition<SafetyStockGridDataModel>>? Filters { get; set; }
    public string SearchTerm { get; set; } = string.Empty;
    public BalticCountry? Country { get; set; }
}
