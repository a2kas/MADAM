using Tamro.Madam.Models.ItemMasterdata.Items;

namespace Tamro.Madam.Models.ItemMasterdata.Brands;

public class BrandDeletionOverviewModel
{
    public BrandModel Brand { get; set; }
    public List<ItemModel> AttachedItems { get; set; }
    public BrandClsfModel NewBrand { get; set; }
    public bool IsExpanded { get; set; }

    public bool IsDeletable
    {
        get { return AttachedItems?.Any() != true; }
    }
}
