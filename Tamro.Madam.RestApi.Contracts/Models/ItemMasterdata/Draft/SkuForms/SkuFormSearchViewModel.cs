using System.Collections.Generic;
using Tamro.Madam.Application.Models.Common;
using Tamro.Madam.Models.General;
using Tamro.Madam.Models.ItemMasterdata.Draft.SkuForm;

namespace Tamro.Madam.RestApi.Contracts.Models.ItemMasterdata.Draft.SkuForms;
public class SkuFormSearchViewModel : PaginationFilter
{
    public HashSet<BalticCountry>? Countries { get; set; }
    public HashSet<SkuFormType>? Types { get; set; }
}
