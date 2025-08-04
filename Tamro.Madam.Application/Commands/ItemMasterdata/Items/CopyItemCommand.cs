using MediatR;
using Tamro.Madam.Application.Infrastructure.Behaviors;
using Tamro.Madam.Application.Models.Common;
using Tamro.Madam.Models.ItemMasterdata.Items;

namespace Tamro.Madam.Application.Commands.ItemMasterdata.Items;

public class CopyItemCommand : IRequest<Result<ItemModel>>, IDefaultErrorMessage
{
    public CopyItemCommand(int id)
    {
        Id = id;
    }

    public int Id { get; set; }
    public string ErrorMessage { get; set; } = "Failed to copy item";
}