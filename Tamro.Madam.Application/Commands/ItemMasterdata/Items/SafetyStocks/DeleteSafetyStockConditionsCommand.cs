using MediatR;
using Tamro.Madam.Application.Infrastructure.Behaviors;
using Tamro.Madam.Application.Models.Common;

namespace Tamro.Madam.Application.Commands.ItemMasterdata.Items.SafetyStocks;

public class DeleteSafetyStockConditionsCommand : IRequest<Result<int>>, IDefaultErrorMessage
{
    public DeleteSafetyStockConditionsCommand(int[] ids)
    {
        Ids = ids;
    }

    public int[] Ids { get; }
    public string ErrorMessage { get; set; } = "Failed to delete safety stock conditions";
}
