using Tamro.Madam.Models.General;

namespace Tamro.Madam.Models.ItemMasterdata.Draft.SkuForm;
public class SkuFormModel
{
    public static string ConstructFullSkuFormFileName(BalticCountry country, SkuFormType type, int versionMajor, int versionMinor, string fileRef)
        => $"NEW_SKU_Tamro_{country}_{type}_{SkuFormModel.ConstructFullVersion(versionMajor, versionMinor)}{Path.GetExtension(fileRef)}";

    private static string ConstructFullVersion(int versionMajor, int versionMinor)
        => $"v{versionMajor}.{versionMinor}";

    public int Id { get; set; }
    public DateTime CreatedDate { get; set; }
    public DateTime RowVer { get; set; }

    public required string FileReference { get; set; }
    public required BalticCountry Country { get; set; }
    public required SkuFormType Type { get; set; }
    public required int VersionMajor { get; set; }
    public required int VersionMinor { get; set; }

    public string FullVersion => ConstructFullVersion(VersionMajor, VersionMinor);

    public string Name => ConstructFullSkuFormFileName(Country, Type, VersionMajor, VersionMinor, FileReference);
}
