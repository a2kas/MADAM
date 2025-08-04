using MudBlazor;
using Tamro.Madam.Application.Models.Common;
using Tamro.Madam.Models.General;
using Tamro.Madam.Models.ItemMasterdata.Items.SafetyStocks.PharmacyChains;

namespace Tamro.Madam.Application.Queries.Items.SafetyStocks.PharmacyChains;

public class PharmacyChainFilter : PaginationFilter
{
    public ICollection<IFilterDefinition<PharmacyChainModel>>? Filters { get; set; }
    public string SearchTerm { get; set; } = string.Empty;
    public BalticCountry? Country { get; set; }
    public HashSet<string>? Group { get; set; }
}
