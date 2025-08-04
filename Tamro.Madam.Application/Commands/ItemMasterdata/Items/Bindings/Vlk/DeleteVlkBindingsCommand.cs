using MediatR;
using Tamro.Madam.Application.Infrastructure.Behaviors;
using Tamro.Madam.Application.Models.Common;

namespace Tamro.Madam.Application.Commands.ItemMasterdata.Items.Bindings.Vlk;

public class DeleteVlkBindingsCommand : IRequest<Result<int>>, IDefaultErrorMessage
{
    public DeleteVlkBindingsCommand(int[] id)
    {
        Id = id;
    }

    public int[] Id { get; }
    public string ErrorMessage { get; set; } = "Failed to delete vlk bindings";
}
