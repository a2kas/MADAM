using MediatR;
using Tamro.Madam.Application.Infrastructure.Behaviors;
using Tamro.Madam.Application.Models.Common;

namespace Tamro.Madam.Application.Commands.ItemMasterdata.Barcodes;

public class DeleteBarcodesCommand : IRequest<Result<int>>, IDefaultErrorMessage
{
    public DeleteBarcodesCommand(int[] id)
    {
        Id = id;
    }

    public int[] Id { get; }
    public string ErrorMessage { get; set; } = "Failed to delete barcodes";
}
