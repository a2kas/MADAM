using MediatR;
using Tamro.Madam.Application.Infrastructure.Behaviors;
using Tamro.Madam.Application.Models.Common;

namespace Tamro.Madam.Application.Commands.Commerce.ItemAssortmentSalesChannels;

public class DeleteItemAssortmentSalesChannelCommand : IRequest<Result<int>>, IDefaultErrorMessage
{
    public DeleteItemAssortmentSalesChannelCommand(int[] id)
    {
        Id = id;
    }

    public string ErrorMessage { get; set; } = "Failed to delete item assortment sales channels";

    public int[] Id { get; }
}
