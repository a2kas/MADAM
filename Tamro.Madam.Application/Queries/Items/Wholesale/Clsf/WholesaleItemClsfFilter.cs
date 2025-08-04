using Tamro.Madam.Models.General;

namespace Tamro.Madam.Application.Queries.Items.Wholesale.Clsf;

public class WholesaleItemClsfFilter
{
    public List<string> ItemNo2 { get; set; } = [];
    public string SearchTerm { get; set; }
    public BalticCountry Country { get; set; }
}
