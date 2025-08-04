using Tamro.Madam.Application.Services.Items.SafetyStocks.Factory;
using Tamro.Madam.Application.Services.Items.Wholesale.Factories;
using Tamro.Madam.Models.General;
using Tamro.Madam.Models.ItemMasterdata.Items.SafetyStocks;

namespace Tamro.Madam.Application.Services.Items.SafetyStocks;

public class SafetyStockService : ISafetyStockService
{
    private readonly IWholesaleItemAvailabilityRepositoryFactory _wholesaleItemAvailabilityRepositoryFactory;
    private readonly ISafetyStockWholesaleRepositoryFactory _safetyStockWholesaleRepositoryFactory;

    public SafetyStockService(IWholesaleItemAvailabilityRepositoryFactory wholesaleItemAvailabilityRepositoryFactory, ISafetyStockWholesaleRepositoryFactory safetyStockWholesaleRepositoryFactory)
    {
        _wholesaleItemAvailabilityRepositoryFactory = wholesaleItemAvailabilityRepositoryFactory;
        _safetyStockWholesaleRepositoryFactory = safetyStockWholesaleRepositoryFactory;
    }

    public async Task<SafetyStockModel> GetSafetyStock(string itemNo2, int checkDays, BalticCountry country)
    {
        var whslStock = await _wholesaleItemAvailabilityRepositoryFactory.Get(country).Get(itemNo2s: [itemNo2,]);
        var rtlStock = await _safetyStockWholesaleRepositoryFactory.Get(country).GetRetailQtyByItemNo(itemNo2, checkDays);

        return new SafetyStockModel()
        {
            WholesaleQuantity = whslStock.FirstOrDefault()?.AvailableQuantity,
            RetailQuantity = rtlStock?.RtlTransQty,
        };
    }
}
