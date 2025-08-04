using AutoMapper;
using MediatR;
using Tamro.Madam.Application.Access;
using Tamro.Madam.Application.Commands.ItemMasterdata.Items.SafetyStocks;
using Tamro.Madam.Application.Infrastructure.Attributes;
using Tamro.Madam.Application.Models.Common;
using Tamro.Madam.Application.Services.Items.SafetyStocks.Factory;
using Tamro.Madam.Application.Utilities.SafetyStocks;
using Tamro.Madam.Models.ItemMasterdata.Items.Wholesale;

namespace Tamro.Madam.Application.Handlers.ItemMasterdata.Items.SafetyStocks;

[RequiresPermission(Permissions.CanViewSafetyStock)]
public class GetSafetyStockItemInfoCommandHandler : IRequestHandler<GetSafetyStockItemInfoCommand, Result<WholesaleSafetyStockItemModel>>
{
    private readonly ISafetyStockWholesaleRepositoryFactory _safetyStockWholesaleRepositoryFactory;
    private readonly IMapper _mapper;

    public GetSafetyStockItemInfoCommandHandler(ISafetyStockWholesaleRepositoryFactory safetyStockWholesaleRepositoryFactory, IMapper mapper)
    {
        _safetyStockWholesaleRepositoryFactory = safetyStockWholesaleRepositoryFactory;
        _mapper = mapper;
    }

    public async Task<Result<WholesaleSafetyStockItemModel>> Handle(GetSafetyStockItemInfoCommand request, CancellationToken cancellationToken)
    {
        var safetyStockWholesaleRepository = _safetyStockWholesaleRepositoryFactory.Get(request.Country);
        var whslItem = await safetyStockWholesaleRepository.GetSafetyStockItemByItemNo(request.ItemNo);
        var rtlQty = await safetyStockWholesaleRepository.GetRetailQtyByItemNo(request.ItemNo, SafetyStockUtility.GetCheckDays(whslItem.ItemGroup));

        var result = _mapper.Map<WholesaleSafetyStockItemModel>(whslItem);
        result.RtlTransQty = rtlQty?.RtlTransQty ?? 0;

        return Result<WholesaleSafetyStockItemModel>.Success(result);
    }
}
