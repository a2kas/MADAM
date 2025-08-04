using MediatR;
using Tamro.Madam.Application.Access;
using Tamro.Madam.Application.Commands.ItemMasterdata.Items.SafetyStocks;
using Tamro.Madam.Application.Infrastructure.Attributes;
using Tamro.Madam.Application.Models.Common;
using Tamro.Madam.Repository.Repositories.ItemMasterdata.Items.SafetyStocks;

namespace Tamro.Madam.Application.Handlers.ItemMasterdata.Items.SafetyStocks;

[RequiresPermission(Permissions.CanManageSafetyStock)]
public class DeleteSafetyStockConditionsCommandHandler : IRequestHandler<DeleteSafetyStockConditionsCommand, Result<int>>
{
    private readonly ISafetyStockConditionRepository _repository;

    public DeleteSafetyStockConditionsCommandHandler(ISafetyStockConditionRepository repository)
    {
        _repository = repository;
    }

    public async Task<Result<int>> Handle(DeleteSafetyStockConditionsCommand command, CancellationToken cancellationToken)
    {
        var result = await _repository.DeleteMany(command.Ids, cancellationToken);
        return Result<int>.Success(result);
    }
}