using Tamro.Madam.Application.Models.Common;
using Tamro.Madam.Models.General;
using Tamro.Madam.Models.ItemMasterdata.Draft.SkuForm;

namespace Tamro.Madam.Application.Queries.Items.Draft.SkuForms;
public class SkuFormFilter : PaginationFilter
{
    public HashSet<BalticCountry>? Countries { get; set; }
    public HashSet<SkuFormType>? Types { get; set; }
}

