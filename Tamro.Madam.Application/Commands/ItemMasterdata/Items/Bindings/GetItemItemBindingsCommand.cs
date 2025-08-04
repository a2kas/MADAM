using MediatR;
using Tamro.Madam.Application.Infrastructure.Behaviors;
using Tamro.Madam.Application.Models.Common;
using Tamro.Madam.Models.ItemMasterdata.Items.Bindings;

namespace Tamro.Madam.Application.Commands.ItemMasterdata.Items.Bindings;

public class GetItemItemBindingsCommand : IRequest<Result<IEnumerable<ItemBindingModel>>>, IDefaultErrorMessage
{
    public GetItemItemBindingsCommand(int itemId)
    {
        ItemId = itemId;
    }

    public int ItemId { get; set; }
    public string ErrorMessage { get; set; } = "Failed to retrieve bindings";
}