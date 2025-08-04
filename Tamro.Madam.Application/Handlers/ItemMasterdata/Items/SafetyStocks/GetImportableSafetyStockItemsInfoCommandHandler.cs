using AutoMapper;
using MediatR;
using Tamro.Madam.Application.Access;
using Tamro.Madam.Application.Commands.ItemMasterdata.Items.SafetyStocks;
using Tamro.Madam.Application.Infrastructure.Attributes;
using Tamro.Madam.Application.Models.Common;
using Tamro.Madam.Application.Services.Items.SafetyStocks.Factory;
using Tamro.Madam.Models.ItemMasterdata.Items.SafetyStocks;

namespace Tamro.Madam.Application.Handlers.ItemMasterdata.Items.SafetyStocks;

[RequiresPermission(Permissions.CanManageSafetyStock)]
public class GetImportableSafetyStockItemsInfoCommandHandler : IRequestHandler<GetImportableSafetyStockItemsInfoCommand, Result<IEnumerable<SafetyStockGridDataModel>>>
{
    private readonly ISafetyStockWholesaleRepositoryFactory _safetyStockWholesaleRepositoryFactory;
    private readonly IMapper _mapper;

    public GetImportableSafetyStockItemsInfoCommandHandler(ISafetyStockWholesaleRepositoryFactory safetyStockWholesaleRepositoryFactory, IMapper mapper)
    {
        _safetyStockWholesaleRepositoryFactory = safetyStockWholesaleRepositoryFactory;
        _mapper = mapper;
    }

    public async Task<Result<IEnumerable<SafetyStockGridDataModel>>> Handle(GetImportableSafetyStockItemsInfoCommand request, CancellationToken cancellationToken)
    {
        var result = await _safetyStockWholesaleRepositoryFactory.Get(request.Country).GetSafetyStockImportedItems(request.ItemNumbers);

        return Result<IEnumerable<SafetyStockGridDataModel>>.Success(_mapper.Map<IEnumerable<SafetyStockGridDataModel>>(result));
    }
}
