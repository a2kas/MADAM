using AutoMapper;
using EFCore.BulkExtensions;
using Microsoft.EntityFrameworkCore;
using Tamro.Madam.Application.Services.Items.SafetyStocks.Factory;
using Tamro.Madam.Application.Utilities.SafetyStocks;
using Tamro.Madam.Models.General;
using Tamro.Madam.Models.ItemMasterdata.Items.SafetyStocks;
using Tamro.Madam.Repository.Common;
using Tamro.Madam.Repository.Entities.ItemMasterdata.Items.SafetyStocks;
using Tamro.Madam.Repository.Repositories.ItemMasterdata.Items.SafetyStocks;

namespace Tamro.Madam.Application.Services.Items.SafetyStocks;

public class LtSafetyStockItemsUpdateService : ISafetyStockItemsUpdateService
{
    private readonly ISafetyStockWholesaleRepositoryFactory _safetyStockWholesaleRepositoryFactory;
    private readonly ISafetyStockItemRepository _safetyStockItemRepository;
    private readonly IMapper _mapper;

    public LtSafetyStockItemsUpdateService(ISafetyStockWholesaleRepositoryFactory safetyStockWholesaleRepositoryFactory, ISafetyStockItemRepository safetyStockItemRepository, IMapper mapper)
    {
        _safetyStockWholesaleRepositoryFactory = safetyStockWholesaleRepositoryFactory;
        _safetyStockItemRepository = safetyStockItemRepository;
        _mapper = mapper;
    }

    public async Task Update()
    {
        var safetyStockRepository = _safetyStockWholesaleRepositoryFactory.Get(BalticCountry.LT);
        var rxOtcItems = await safetyStockRepository.GetSafetyStockItems();
        var manuallyAddedItems = await safetyStockRepository.GetSafetyStockExistingItems();
        var whslItems = rxOtcItems.Union(manuallyAddedItems).DistinctBy(x => x.ItemNo);

        var safetyStockIncludes = new List<IncludeOperation<SafetyStockItem>>
        {
            new(q => q.Include(i => i.SafetyStock)),
            new(q => q.Include(i => i.SafetyStockConditions)),
        };
        var safetyStockItems = await _safetyStockItemRepository.GetMany(filter: x => x.Country == BalticCountry.LT,
            includes: safetyStockIncludes,
            track: false);

        var upsertableSafetyStockItems = new List<SafetyStockItemModel>();

        foreach (var whslItem in whslItems)
        {
            var safetyStockItem = safetyStockItems.SingleOrDefault(x => x.ItemNo == whslItem.ItemNo);
            if (safetyStockItem != null)
            {
                if (!string.Equals(whslItem.ItemGroup, safetyStockItem.ItemGroup, StringComparison.OrdinalIgnoreCase))
                {
                    safetyStockItem.CheckDays = SafetyStockUtility.GetCheckDays(whslItem.ItemGroup);
                }
                _mapper.Map(whslItem, safetyStockItem);
            }
            else
            {
                safetyStockItem = _mapper.Map<SafetyStockItemModel>(whslItem);
                safetyStockItem.CheckDays = SafetyStockUtility.GetCheckDays(safetyStockItem?.ItemGroup);
                safetyStockItem.SafetyStockConditions =
                [
                    new SafetyStockConditionModel()
                    {
                        CanBuy = true,
                        RestrictionLevel = SafetyStockRestrictionLevel.PharmacyChainGroup,
                        SafetyStockPharmacyChainGroup = PharmacyGroup.All,
                        CreatedDate = DateTime.UtcNow,
                        User = "System",                      
                    }
                ];
                safetyStockItem.SafetyStock = new SafetyStockModel();
            }
            upsertableSafetyStockItems.Add(safetyStockItem);
        }

        var existingSafetyStockItems = upsertableSafetyStockItems.Where(x => x.Id != default).ToList();
        var newSafetyStockItems = upsertableSafetyStockItems.Where(x => x.Id == default).ToList();

        var bulkConfig = new BulkConfig()
        {
            IncludeGraph = true,
        };

        if (existingSafetyStockItems.Any())
        {
            await _safetyStockItemRepository.UpsertBulkRange(existingSafetyStockItems, bulkConfig);
        }
        if (newSafetyStockItems.Any())
        {
            foreach (var newSafetyStockItem in newSafetyStockItems)
            {
                await _safetyStockItemRepository.UpsertGraph(newSafetyStockItem);
            }
        }

        safetyStockItems = await _safetyStockItemRepository.GetMany(filter: x => x.Country == BalticCountry.LT,
            includes: safetyStockIncludes,
            track: false);
        var retailQties = await safetyStockRepository.GetRetailQty();

        foreach  (var safetyStockItem in safetyStockItems)
        {
            var matchingRetailQty = retailQties.FirstOrDefault(qty => qty.SafetyStockItemId == safetyStockItem.Id);

            if (matchingRetailQty != null)
            {
                safetyStockItem.SafetyStock.RetailQuantity = matchingRetailQty.RtlTransQty;
            }
        }

        await _safetyStockItemRepository.UpsertBulkRange(safetyStockItems, bulkConfig);
    }

    public async Task Cleanup()
    {
        await _safetyStockItemRepository.DeleteMany(x => x.Country == BalticCountry.LT && !x.SafetyStockConditions.Any(), CancellationToken.None);
    }
}
