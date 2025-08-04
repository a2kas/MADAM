using MediatR;
using Tamro.Madam.Application.Infrastructure.Behaviors;
using Tamro.Madam.Application.Models.Common;

namespace Tamro.Madam.Application.Commands.ItemMasterdata.Forms;

public class DeleteFormsCommand : IRequest<Result<int>>, IDefaultErrorMessage
{
    public DeleteFormsCommand(int[] id)
    {
        Id = id;
    }

    public int[] Id { get; }
    public string ErrorMessage { get; set; } = "Failed to delete forms";
}
