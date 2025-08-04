using MediatR;
using Tamro.Madam.Application.Access;
using Tamro.Madam.Application.Commands.ItemMasterdata.Items.SafetyStocks;
using Tamro.Madam.Application.Infrastructure.Attributes;
using Tamro.Madam.Application.Models.Common;
using Tamro.Madam.Repository.Repositories.ItemMasterdata.Items.SafetyStocks;

namespace Tamro.Madam.Application.Handlers.ItemMasterdata.Items.SafetyStocks;

[RequiresPermission(Permissions.CanManageSafetyStock)]
public class DeleteSafetyStocksCommandHandler : IRequestHandler<DeleteSafetyStocksCommand, Result<int>>
{
    private readonly ISafetyStockItemRepository _repository;

    public DeleteSafetyStocksCommandHandler(ISafetyStockItemRepository repository)
    {
        _repository = repository;
    }

    public async Task<Result<int>> Handle(DeleteSafetyStocksCommand command, CancellationToken cancellationToken)
    {
        var result = await _repository.DeleteMany(x => command.Id.Contains(x.Id), cancellationToken);
        return Result<int>.Success(result);
    }
}