using Hangfire;
using Hangfire.Server;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Tamro.Madam.Application.Models.Common;
using Tamro.Madam.Application.Services.Items.Wholesale.Factories;
using Tamro.Madam.Models.General;
using Tamro.Madam.Models.ItemMasterdata.Items.SafetyStocks;
using Tamro.Madam.Repository.Common;
using Tamro.Madam.Repository.Entities.ItemMasterdata.Items.SafetyStocks;
using Tamro.Madam.Repository.Repositories.ItemMasterdata.Items.SafetyStocks;

namespace Tamro.Madam.Application.Jobs.Hangfire;

public class SafetyStockWhslQtyUpdate : SafetyStockWhslQtyUpdateBase, IOneTimeJob
{
    public string Name => "Synchronize safety stock wholesale quantities";

    public string Description => "Execute job to synchronize safety stock wholesale quantities. Job is automatically run each 30 minutes every day, in the interval from 07:00 to 22:00.";

    public bool Processing { get; set; }

    private readonly IWholesaleItemAvailabilityRepositoryFactory _wholesaleItemAvailabilityRepositoryFactory;
    private readonly ISafetyStockItemRepository _safetyStockItemRepository;
    private readonly ISafetyStockRepository _safetyStockRepository;
    private readonly ILogger<SafetyStockWhslQtyUpdate> _logger;

    public SafetyStockWhslQtyUpdate()
    {
    }

    public SafetyStockWhslQtyUpdate(IWholesaleItemAvailabilityRepositoryFactory wholesaleItemAvailabilityRepositoryFactory,
        ISafetyStockItemRepository safetyStockItemRepository,
        ISafetyStockRepository safetyStockRepository,
        ILogger<SafetyStockWhslQtyUpdate> logger)
    {
        _wholesaleItemAvailabilityRepositoryFactory = wholesaleItemAvailabilityRepositoryFactory;
        _safetyStockItemRepository = safetyStockItemRepository;
        _safetyStockRepository = safetyStockRepository;
        _logger = logger;
    }

    public async Task<Result<int>> Execute()
    {
        try
        {
            await Synchronize();
        }
        catch (Exception ex)
        {
            const string errorMessage = "Failed to synchronize safety stock wholesale quantities";
            _logger.LogError(ex, errorMessage);
            return Result<int>.Failure(errorMessage);
        }

        return Result<int>.Success(1);
    }

    public override async Task JobToRun(IJobCancellationToken token, PerformContext context)
    {
        await Synchronize();
    }

    private async Task Synchronize()
    {
        var eeQtyTask = _wholesaleItemAvailabilityRepositoryFactory.Get(BalticCountry.EE).GetAll();
        var ltQtyTask = _wholesaleItemAvailabilityRepositoryFactory.Get(BalticCountry.LT).GetAll();
        var lvQtyTask = _wholesaleItemAvailabilityRepositoryFactory.Get(BalticCountry.LV).GetAll();

        var safetyStockIncludes = new List<IncludeOperation<SafetyStockItem>>
        {
            new(q => q.Include(s => s.SafetyStock)),
        };
        var safetyStockTask = _safetyStockItemRepository.GetMany(filter: null, includes: safetyStockIncludes, track: false);

        await Task.WhenAll(eeQtyTask, ltQtyTask, lvQtyTask, safetyStockTask);
        var quantitiesDictionary = eeQtyTask.Result
              .Concat(ltQtyTask.Result)
              .Concat(lvQtyTask.Result)
              .GroupBy(item => new { item.Country, item.ItemNo })
              .ToDictionary(group => group.Key, group => group.Sum(item => item.AvailableQuantity));

        var safetyStocks = new List<SafetyStockModel>();
        foreach (var safetyStockItem in safetyStockTask.Result)
        {
            quantitiesDictionary.TryGetValue(new { safetyStockItem.Country, safetyStockItem.ItemNo }, out var quantity);
            var safetyStock = safetyStockItem.SafetyStock ?? new SafetyStockModel();
            safetyStock.Id = safetyStockItem.Id;
            safetyStock.WholesaleQuantity = quantity;

            safetyStocks.Add(safetyStock);
        }

        await _safetyStockRepository.UpsertBulkRange(safetyStocks);
    }
}
