using MediatR;
using Tamro.Madam.Application.Infrastructure.Behaviors;
using Tamro.Madam.Application.Models.Common;
using Tamro.Madam.Models.ItemMasterdata.Items.SafetyStocks;

namespace Tamro.Madam.Application.Commands.ItemMasterdata.Items.SafetyStocks;

public class UpsertSafetyStockItemCommand : IRequest<Result<SafetyStockItemModel>>, IDefaultErrorMessage
{
    public UpsertSafetyStockItemCommand(SafetyStockItemUpsertFormModel request)
    {
        Model = request;
    }
    public SafetyStockItemUpsertFormModel Model { get; set; }
    public string ErrorMessage { get; set; } = "Failed to save safety stock item";
}