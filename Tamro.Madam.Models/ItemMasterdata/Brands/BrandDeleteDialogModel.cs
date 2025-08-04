namespace Tamro.Madam.Models.ItemMasterdata.Brands;

public class BrandDeleteDialogModel
{
    private IEnumerable<BrandDeletionOverviewModel> _overviews;

    public IEnumerable<BrandModel> Brands { get; set; }
    public IEnumerable<BrandDeletionOverviewModel> Overviews
    {
        get => _overviews;
        set => _overviews = value?.OrderBy(o => o.Brand?.Name);
    }
    public bool IsDeleteButtonEnabled
    {
        get
        {
            return Overviews?.Any(x => x.IsDeletable) ?? false;
        }
    }
}
