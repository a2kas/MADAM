using Hangfire;
using Hangfire.Server;
using Microsoft.Extensions.Logging;
using Tamro.Madam.Application.Models.Common;
using Tamro.Madam.Application.Services.Items.SafetyStocks;

namespace Tamro.Madam.Application.Jobs.Hangfire;

public class SafetyStockItemsUpdate : SafetyStockItemsUpdateBase, IOneTimeJob
{
    public string Name => "Synchronize safety stock items";

    public string Description => "Synchronize safety stock items. The job is automatically run hourly from 6:00 am to 2:00 pm";

    public bool Processing { get; set; }

    private readonly IEnumerable<ISafetyStockItemsUpdateService> _safetyStockItemsUpdateServices;
    private readonly ILogger<SafetyStockItemsUpdate> _logger;

    public SafetyStockItemsUpdate()
    {
    }

    public SafetyStockItemsUpdate(IEnumerable<ISafetyStockItemsUpdateService> safetyStockItemsUpdateServices, ILogger<SafetyStockItemsUpdate> logger)
    {
        _safetyStockItemsUpdateServices = safetyStockItemsUpdateServices;
        _logger = logger;
    }

    public async Task<Result<int>> Execute()
    {
        try
        {
            await Update();
        }
        catch (Exception ex)
        {
            const string errorMessage = "Failed to update safety stock items";
            _logger.LogError(ex, errorMessage);
            return Result<int>.Failure(errorMessage);
        }

        return Result<int>.Success(1);
    }

    public override async Task JobToRun(IJobCancellationToken token, PerformContext context)
    {
        await Update();
    }

    private async Task Update()
    {
        foreach (var updateService in _safetyStockItemsUpdateServices)
        {
            await updateService.Update();
            await updateService.Cleanup();
        }
    }
}
