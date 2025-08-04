using Tamro.Madam.Models.General;
using Tamro.Madam.Models.ItemMasterdata.Draft.SkuForm;

namespace Tamro.Madam.RestApi.Contracts.Models.ItemMasterdata.Draft.SkuForms;
public class SkuFormViewModel
{
    public int Id { get; set; }

    public required BalticCountry Country { get; set; }
    public required SkuFormType Type { get; set; }

    public required int VersionMajor { get; set; }
    public required int VersionMinor { get; set; }
    public required string FullVersion { get; set; }
    public required string Name { get; set; }
}
