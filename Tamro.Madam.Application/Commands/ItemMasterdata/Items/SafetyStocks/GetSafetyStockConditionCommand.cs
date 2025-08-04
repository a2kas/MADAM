using MediatR;
using Tamro.Madam.Application.Infrastructure.Behaviors;
using Tamro.Madam.Application.Models.Common;
using Tamro.Madam.Models.ItemMasterdata.Items.SafetyStocks;

namespace Tamro.Madam.Application.Commands.ItemMasterdata.Items.SafetyStocks;

public class GetSafetyStockConditionCommand : IRequest<Result<SafetyStockConditionModel>>, IDefaultErrorMessage
{
    public GetSafetyStockConditionCommand(int id)
    {
        Id = id;
    }

    public int Id { get; set; }

    public string ErrorMessage { get; set; } = "Failed to retrieve safety stock condition";
}