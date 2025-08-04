using Microsoft.AspNetCore.Mvc;

namespace Tamro.Madam.RestApi.Contracts.Models.ItemMasterdata.Draft.SkuForms;
public class GetSkuFormByIdViewModel
{
    [FromRoute]
    public required int Id { get; set; }
}
