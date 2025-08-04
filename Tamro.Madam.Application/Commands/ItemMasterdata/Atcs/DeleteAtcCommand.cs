using MediatR;
using Tamro.Madam.Application.Infrastructure.Behaviors;
using Tamro.Madam.Application.Models.Common;

namespace Tamro.Madam.Application.Commands.ItemMasterdata.Atcs;

public class DeleteAtcsCommand : IRequest<Result<int>>, IDefaultErrorMessage
{
    public DeleteAtcsCommand(int[] id)
    {
        Id = id;
    }

    public string ErrorMessage { get; set; } = "Failed to delete ATCs";

    public int[] Id { get; }
}
