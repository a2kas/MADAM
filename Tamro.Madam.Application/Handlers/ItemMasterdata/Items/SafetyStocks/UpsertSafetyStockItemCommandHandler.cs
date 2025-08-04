using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Tamro.Madam.Application.Access;
using Tamro.Madam.Application.Commands.ItemMasterdata.Items.SafetyStocks;
using Tamro.Madam.Application.Infrastructure.Attributes;
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
public class UpsertSafetyStockItemCommandHandler : IRequestHandler<UpsertSafetyStockItemCommand, Result<SafetyStockItemModel>>
{
    private readonly ISafetyStockItemRepository _repository;
    private readonly ISafetyStockService _safetyStockService;
    private readonly IMapper _mapper;

    public UpsertSafetyStockItemCommandHandler(ISafetyStockItemRepository repository,
        ISafetyStockService safetyStockService,
        IMapper mapper)
    {
        _repository = repository;
        _safetyStockService = safetyStockService;
        _mapper = mapper;
    }

    public async Task<Result<SafetyStockItemModel>> Handle(UpsertSafetyStockItemCommand request, CancellationToken cancellationToken)
    {
        var safetyStockIncludes = new List<IncludeOperation<SafetyStockItem>>
        {
            new(q => q.Include(s => s.SafetyStockConditions)),
            new(q => q.Include(s => s.SafetyStock)),
        };
        var existingSafetyStockItem = (await _repository.GetMany(filter: x => x.Country == request.Model.Country && x.ItemNo == request.Model.ItemInfo.ItemNo,
            includes: safetyStockIncludes,
            track: false,
            cancellationToken)).SingleOrDefault();

        int checkDays = SafetyStockUtility.GetCheckDays(request.Model.ItemInfo.ItemGroup);
        var safetyStockItem = _mapper.Map<SafetyStockItemModel>(request.Model.ItemInfo);
        safetyStockItem.Country = request.Model.Country;

        if (existingSafetyStockItem != null)
        {
            safetyStockItem = existingSafetyStockItem;
            if (existingSafetyStockItem.SafetyStockConditions.Any(x => x.SafetyStockPharmacyChainGroup == PharmacyGroup.All))
            {
                return Result<SafetyStockItemModel>.Failure("Safety stock condition record for item already exists");
            }
            if (request.Model.RestrictionLevel == SafetyStockRestrictionLevel.PharmacyChain)
            {
                var overlappingChains = request.Model.PharmacyChains.Where(x => existingSafetyStockItem.SafetyStockConditions.Any(y => y.SafetyStockPharmacyChainId == x.Id));
                if (overlappingChains.Any())
                {
                    return Result<SafetyStockItemModel>.Failure($"Item '{request.Model.Item.DisplayName}' already has safety stock condition set for the following pharmacies: {string.Join(", ", overlappingChains.Select(x => x.DisplayName))}");
                }
            }
            if (request.Model.RestrictionLevel == SafetyStockRestrictionLevel.PharmacyChainGroup)
            {
                var overlappingPharmacyGroups = request.Model.PharmacyGroups.Where(x => existingSafetyStockItem.SafetyStockConditions.Any(y => y.SafetyStockPharmacyChainGroup == x));
                if (overlappingPharmacyGroups.Any())
                {
                    return  Result<SafetyStockItemModel>.Failure($"Item '{request.Model.Item.DisplayName}' already has safety stock condition set for the following pharmacy groups: {string.Join(", ", overlappingPharmacyGroups)}");
                }
            }
        }
        else
        {
            safetyStockItem.CheckDays = checkDays;
            safetyStockItem.SafetyStock = await _safetyStockService.GetSafetyStock(safetyStockItem.ItemNo, checkDays, safetyStockItem.Country);
        }

        var safetyStockConditions = new List<SafetyStockConditionModel>();
        
        if (request.Model.RestrictionLevel == SafetyStockRestrictionLevel.PharmacyChain)
        {
            foreach (var pharmacyChain in request.Model.PharmacyChains)
            {
                var condition = new SafetyStockConditionModel()
                {
                    User = request.Model.User.DisplayName,
                    Comment = request.Model.Comment,
                    CheckDays = checkDays,
                    RestrictionLevel = SafetyStockRestrictionLevel.PharmacyChain,
                    SafetyStockPharmacyChainId = pharmacyChain.Id,
                };
                safetyStockConditions.Add(condition);
            }
        }
        else if (request.Model.RestrictionLevel == SafetyStockRestrictionLevel.PharmacyChainGroup)
        {
            foreach (var pharmacyGroup in request.Model.PharmacyGroups)
            {
                var condition = new SafetyStockConditionModel()
                {
                    User = request.Model.User.DisplayName,
                    Comment = request.Model.Comment,
                    CheckDays = checkDays,
                    RestrictionLevel = SafetyStockRestrictionLevel.PharmacyChainGroup,
                    SafetyStockPharmacyChainGroup = pharmacyGroup,
                };
                safetyStockConditions.Add(condition);
            }
        }

        foreach (var ssc in safetyStockConditions)
        {
            safetyStockItem.SafetyStockConditions = safetyStockItem.SafetyStockConditions.Append(ssc);
        }

        var result = await _repository.UpsertGraph(safetyStockItem);

        return Result<SafetyStockItemModel>.Success(result);
    }
}
