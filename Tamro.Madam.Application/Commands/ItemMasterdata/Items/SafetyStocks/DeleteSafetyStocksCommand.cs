using MediatR;
using Tamro.Madam.Application.Infrastructure.Behaviors;
using Tamro.Madam.Application.Models.Common;

namespace Tamro.Madam.Application.Commands.ItemMasterdata.Items.SafetyStocks;

public class DeleteSafetyStocksCommand : IRequest<Result<int>>, IDefaultErrorMessage
{
    public DeleteSafetyStocksCommand(List<int> id)
    {
        Id = id;
    }

    public List<int> Id { get; }
    public string ErrorMessage { get; set; } = "Failed to delete safety stocks";
}
