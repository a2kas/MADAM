using MediatR;
using Tamro.Madam.Application.Infrastructure.Behaviors;
using Tamro.Madam.Application.Models.Common;

namespace Tamro.Madam.Application.Commands.Sales.Sabis;

public class DeleteSksContractsCommand : IRequest<Result<int>>, IDefaultErrorMessage
{
    public DeleteSksContractsCommand(int[] id)
    {
        Id = id;
    }

    public int[] Id { get; }
    public string ErrorMessage { get; set; } = "Failed to delete contract mappings";
}
