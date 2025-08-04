namespace Tamro.Madam.Ui.Store.Actions.ItemMasterdata.Brands;

public class UpdateBrandAttachedItemsAction
{
    public IEnumerable<int> UpdatedItemIds { get; set; }
    public int NewBrandId { get; set; }
    public int OldBrandId { get; set; }
}
