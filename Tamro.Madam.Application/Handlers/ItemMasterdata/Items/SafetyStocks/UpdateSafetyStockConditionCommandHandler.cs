using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Tamro.Madam.Application.Access;
using Tamro.Madam.Application.Commands.ItemMasterdata.Items.SafetyStocks;
using Tamro.Madam.Application.Infrastructure.Attributes;
using Tamro.Madam.Application.Models.Common;
using Tamro.Madam.Application.Services.Items.SafetyStocks.Factory;
using Tamro.Madam.Application.Validation;
using Tamro.Madam.Models.ItemMasterdata.Items.SafetyStocks;
using Tamro.Madam.Repository.Common;
using Tamro.Madam.Repository.Entities.ItemMasterdata.Items.SafetyStocks;
using Tamro.Madam.Repository.Repositories.ItemMasterdata.Items.SafetyStocks;

namespace Tamro.Madam.Application.Handlers.ItemMasterdata.Items.SafetyStocks;

[RequiresPermission(Permissions.CanManageSafetyStock)]
public class UpdateSafetyStockConditionCommandHandler : IRequestHandler<UpdateSafetyStockConditionCommand, Result<SafetyStockConditionModel>>
{
    private readonly ISafetyStockConditionRepository _safetyStockConditionRepository;
    private readonly ISafetyStockItemRepository _safetyStockItemRepository;
    private readonly ISafetyStockWholesaleRepositoryFactory _safetyStockWholesaleRepositoryFactory;
    private readonly IHandlerValidator _validator;
    private readonly IMapper _mapper;

    public UpdateSafetyStockConditionCommandHandler(ISafetyStockConditionRepository safetyStockConditionRepository,
        ISafetyStockItemRepository safetyStockItemRepository,
        ISafetyStockWholesaleRepositoryFactory safetyStockWholesaleRepositoryFactory,
        IHandlerValidator validator,
        IMapper mapper)
    {
        _safetyStockConditionRepository = safetyStockConditionRepository;
        _safetyStockItemRepository = safetyStockItemRepository;
        _safetyStockWholesaleRepositoryFactory = safetyStockWholesaleRepositoryFactory;
        _validator = validator;
        _mapper = mapper;
    }

    public async Task<Result<SafetyStockConditionModel>> Handle(UpdateSafetyStockConditionCommand request, CancellationToken cancellationToken)
    {
        await _validator.Validate(request.Model);

        var safetyStockCondition = await _safetyStockConditionRepository.Get(request.Model.Id);

        if (safetyStockCondition != null)
        {
            if (safetyStockCondition.CheckDays != request.Model.CheckDays)
            {
                await UpdateSafetyStockItem(safetyStockCondition, request.Model, cancellationToken);
            }
            _mapper.Map(request.Model, safetyStockCondition);
        }
        else
        {
            return Result<SafetyStockConditionModel>.Failure("Failed to upload safety stock condition as it does not exist anymore");
        }

        var result = await _safetyStockConditionRepository.Upsert(safetyStockCondition);
        return Result<SafetyStockConditionModel>.Success(result);
    }

    private async Task UpdateSafetyStockItem(SafetyStockConditionModel existingSafetyStockCondition, SafetyStockConditionUpsertModel updatedSafetyStockCondition, CancellationToken cancellationToken)
    {
        var includes = new List<IncludeOperation<SafetyStockItem>>
        {
            new(q => q.Include(ssi => ssi.SafetyStock)),
            new(q => q.Include(ssi => ssi.SafetyStockConditions)),
        };
        var safetyStockItem = await _safetyStockItemRepository.Get(x => x.Id == existingSafetyStockCondition.SafetyStockItemId, includes, track: true, cancellationToken);
        var retailStock = await _safetyStockWholesaleRepositoryFactory.Get(safetyStockItem.Country).GetRetailQtyByItemNo(safetyStockItem.ItemNo, updatedSafetyStockCondition?.CheckDays ?? 0);
        safetyStockItem.SafetyStock.RetailQuantity = retailStock?.RtlTransQty ?? 0;
        safetyStockItem.CheckDays = updatedSafetyStockCondition.CheckDays ?? 0;
        await _safetyStockItemRepository.UpsertGraph(safetyStockItem);
    }
}
