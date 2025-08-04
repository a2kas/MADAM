namespace Tamro.Madam.Models.Commerce.ItemAssortmentSalesChannels.Import;

public class ItemAssortmentImportResultModel
{
    public IEnumerable<ItemAssortmentGridModel> Assortment { get; set; } = [];
    public IEnumerable<ItemAssortmentImportOverviewModel> Overview { get; set; } = [];
}
