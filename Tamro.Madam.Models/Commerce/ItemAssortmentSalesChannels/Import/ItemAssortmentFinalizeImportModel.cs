namespace Tamro.Madam.Models.Commerce.ItemAssortmentSalesChannels.Import;

public class ItemAssortmentFinalizeImportModel
{
    public IEnumerable<ItemAssortmentGridModel> ExistingAssortment { get; set; } = [];
    public IEnumerable<ItemAssortmentItemModel> ImportedAssortment { get; set; } = [];
    public ItemAssortmentImportAction ImportAction { get; set; }
    public int SalesChannelId { get; set; }
}
