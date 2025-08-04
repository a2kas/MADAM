using MediatR;
using Tamro.Madam.Application.Infrastructure.Behaviors;
using Tamro.Madam.Application.Models.Common;
using Tamro.Madam.Models.Commerce.ItemAssortmentSalesChannels;

namespace Tamro.Madam.Application.Commands.Commerce.ItemAssortmentSalesChannels;

public class GetItemAssortmentSalesChannelCommand : IRequest<Result<ItemAssortmentSalesChannelDetailsModel>>, IDefaultErrorMessage
{
    public GetItemAssortmentSalesChannelCommand(int id)
    {
        Id = id;
    }

    public int Id { get; set; }
    public string ErrorMessage { get; set; } = "Failed to retrieve item assortment sales channel";
}
