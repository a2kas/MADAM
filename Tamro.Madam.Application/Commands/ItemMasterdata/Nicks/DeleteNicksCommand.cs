using MediatR;
using Tamro.Madam.Application.Infrastructure.Behaviors;
using Tamro.Madam.Application.Models.Common;

namespace Tamro.Madam.Application.Commands.ItemMasterdata.Nicks;

public class DeleteNicksCommand : IRequest<Result<int>>, IDefaultErrorMessage
{
    public DeleteNicksCommand(int[] id)
    {
        Id = id;
    }

    public int[] Id { get; }
    public string ErrorMessage { get; set; } = "Failed to delete baltic nick(s)";
}
