using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Tamro.Madam.Application.Access;
using Tamro.Madam.Application.Commands.ItemMasterdata.Items.SafetyStocks;
using Tamro.Madam.Application.Infrastructure.Attributes;
using Tamro.Madam.Application.Infrastructure.Session;
using Tamro.Madam.Application.Models.Common;
using Tamro.Madam.Application.Services.Items.SafetyStocks;
using Tamro.Madam.Application.Utilities.SafetyStocks;
using Tamro.Madam.Models.General;
using Tamro.Madam.Models.ItemMasterdata.Items.SafetyStocks;
using Tamro.Madam.Repository.Common;
using Tamro.Madam.Repository.Entities.ItemMasterdata.Items.SafetyStocks;
using Tamro.Madam.Repository.Repositories.ItemMasterdata.Items.SafetyStocks;

namespace Tamro.Madam.Application.Handlers.ItemMasterdata.Items.SafetyStocks;

[RequiresPermission(Permissions.CanManageSafetyStock)]
public class ImportSafetyStocksCommandHandler : IRequestHandler<ImportSafetyStocksCommand, Result<IEnumerable<SafetyStockImportResultModel>>>
{
    private readonly ISafetyStockItemRepository _safetyStockItemRepository;
    private readonly ISafetyStockService _safetyStockService;
    private readonly IUserContext _userContext;
    private readonly ILogger<ImportSafetyStocksCommandHandler> _logger;
    private readonly IMapper _mapper;

    public ImportSafetyStocksCommandHandler(ISafetyStockItemRepository safetyStockItemRepository,
        ISafetyStockService safetyStockService,
        IUserContext userContext,
        ILogger<ImportSafetyStocksCommandHandler> logger,
        IMapper mapper)
    {
        _userContext = userContext;
        _safetyStockItemRepository = safetyStockItemRepository;
        _safetyStockService = safetyStockService;
        _logger = logger;
        _mapper = mapper;
    }

    public async Task<Result<IEnumerable<SafetyStockImportResultModel>>> Handle(ImportSafetyStocksCommand request, CancellationToken cancellationToken)
    {
        var itemNumbers = request.SafetyStocks.Select(x => x.ItemNo).Distinct();
        var safetyStockIncludes = new List<IncludeOperation<SafetyStockItem>>
        {
            new(q => q.Include(s => s.SafetyStockConditions)),
            new(q => q.Include(s => s.SafetyStock)),
        };
        var safetyStockItems = await _safetyStockItemRepository.GetMany(filter: x => x.Country == request.Country && itemNumbers.Contains(x.ItemNo),
            includes: safetyStockIncludes,
            track: false,
            cancellationToken);

        var result = new List<SafetyStockImportResultModel>();
        var groupedSafetyStocks = request.SafetyStocks.Where(x => !string.IsNullOrEmpty(x.ItemName)).GroupBy(x => x.ItemNo);

        foreach (var groupedSafetyStock in groupedSafetyStocks)
        {
            bool shouldUpdate = false;
            var safetyStockItem = safetyStockItems.SingleOrDefault(x => x.ItemNo == groupedSafetyStock.Key);
            bool safetyStockItemAlreadyExists = safetyStockItem != null;
            if (!safetyStockItemAlreadyExists)
            {
                safetyStockItem = _mapper.Map<SafetyStockItemModel>(groupedSafetyStock.First());
                safetyStockItem.SafetyStock = await _safetyStockService.GetSafetyStock(groupedSafetyStock.Key, SafetyStockUtility.GetCheckDays(safetyStockItem.ItemGroup), request.Country);
            }
            var scopeResults = new List<SafetyStockImportResultModel>();
            foreach (var importableSafetyStock in groupedSafetyStock)
            {
                if (safetyStockItem.SafetyStockConditions.Any(x => x.SafetyStockPharmacyChainId == importableSafetyStock.PharmacyChainId && x.SafetyStockPharmacyChainGroup.ToString() == (importableSafetyStock.SafetyStockPharmacyChainGroup ?? "")))
                {
                    scopeResults.Add(new SafetyStockImportResultModel()
                    {
                        SafetyStock = importableSafetyStock,
                        IsImported = false,
                        Message = "Item already has such restriction set",
                    });
                }
                else
                {
                    shouldUpdate = true;
                    safetyStockItem.CheckDays = importableSafetyStock.CheckDays;
                    safetyStockItem.SafetyStockConditions = safetyStockItem.SafetyStockConditions.Append(new SafetyStockConditionModel()
                    {
                        RestrictionLevel = importableSafetyStock.PharmacyChainId != default ? SafetyStockRestrictionLevel.PharmacyChain : SafetyStockRestrictionLevel.PharmacyChainGroup,
                        CanBuy = false,
                        Comment = importableSafetyStock.Comment,
                        SafetyStockItemId = safetyStockItem.Id,
                        SafetyStockPharmacyChainId = importableSafetyStock.PharmacyChainId,
                        SafetyStockPharmacyChainGroup = !string.IsNullOrEmpty(importableSafetyStock.SafetyStockPharmacyChainGroup) ? Enum.Parse<PharmacyGroup>(importableSafetyStock.SafetyStockPharmacyChainGroup) : null,
                        User = _userContext.DisplayName,
                    });
                    scopeResults.Add(new SafetyStockImportResultModel()
                    {
                        SafetyStock = importableSafetyStock,
                        IsImported = true,
                        Message = safetyStockItemAlreadyExists ? "Item restrictions extended" : "Item added to safety stock and restrictions applied",
                    });
                }
            }
            try
            {
                if (shouldUpdate)
                {
                    safetyStockItem.SafetyStockConditions = safetyStockItem.SafetyStockConditions.Where(x => x.SafetyStockPharmacyChainGroup != PharmacyGroup.All);
                    await _safetyStockItemRepository.UpsertGraph(safetyStockItem);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed-to-upsert-safety-stock");
                scopeResults.ForEach(x => x.IsImported = false);
                scopeResults.ForEach(x => x.Message = "Failed to import due to unknown reason");
            }
            finally
            {
                result.AddRange(scopeResults);
            }
        }

        return Result<IEnumerable<SafetyStockImportResultModel>>.Success(result);
    }
}