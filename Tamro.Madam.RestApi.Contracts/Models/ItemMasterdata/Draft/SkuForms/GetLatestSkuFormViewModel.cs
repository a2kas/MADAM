using Microsoft.AspNetCore.Mvc;
using Tamro.Madam.Models.General;
using Tamro.Madam.Models.ItemMasterdata.Draft.SkuForm;

namespace Tamro.Madam.RestApi.Contracts.Models.ItemMasterdata.Draft.SkuForms;
public class GetLatestSkuFormViewModel
{
    [FromRoute]
    public required BalticCountry country { get; set; }

    [FromQuery]
    public required SkuFormType type { get; set; }
}
