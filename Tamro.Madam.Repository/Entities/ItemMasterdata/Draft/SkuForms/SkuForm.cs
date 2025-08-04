using System.ComponentModel;
using Tamro.Madam.Models.General;
using Tamro.Madam.Models.ItemMasterdata.Draft.SkuForm;
using Tamro.Madam.Repository.Context.Madam;
using TamroUtilities.EFCore.Models;

namespace Tamro.Madam.Repository.Entities.ItemMasterdata.Draft.SkuForms;

[DisplayName("SKU form")]
public class SkuForm : IMadamEntity<int>, IAuditable, IBaseEntity
{
    public int Id { get; set; }
    public DateTime CreatedDate { get; set; }
    public DateTime RowVer { get; set; }

    public required string FileReference { get; set; }
    public required BalticCountry Country { get; set; }
    public required SkuFormType Type { get; set; }
    public required int VersionMajor { get; set; }
    public required int VersionMinor { get; set; }
}
