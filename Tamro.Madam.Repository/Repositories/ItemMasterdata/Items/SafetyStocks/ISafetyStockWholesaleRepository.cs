using Tamro.Madam.Repository.Entities.Wholesale;

namespace Tamro.Madam.Repository.Repositories.ItemMasterdata.Items.SafetyStocks;

public interface ISafetyStockWholesaleRepository
{
    Task<IEnumerable<WholesaleSafetyStockItem>> GetSafetyStockItems();
    Task<IEnumerable<WholesaleSafetyStockItem>> GetSafetyStockExistingItems();
    Task<IEnumerable<WholesaleSafetyStockItemRetailQty>> GetRetailQty();
    Task<IEnumerable<WholesaleSafetyStockItem>> GetSafetyStockImportedItems(string[] importedItemNumbers);
    Task<WholesaleSafetyStockItemRetailQty> GetRetailQtyByItemNo(string itemNo, int checkDays);
    Task<WholesaleSafetyStockItem> GetSafetyStockItemByItemNo(string itemNo);
}
