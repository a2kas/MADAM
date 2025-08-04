namespace Tamro.Madam.Application.Services.Items.SafetyStocks;

public interface ISafetyStockItemsUpdateService
{
    Task Update();
    Task Cleanup();
}
