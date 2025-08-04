using MediatR;
using Tamro.Madam.Application.Infrastructure.Behaviors;
using Tamro.Madam.Application.Models.Common;
using Tamro.Madam.Models.Commerce.ItemAssortmentSalesChannels;
using Tamro.Madam.Models.Commerce.ItemAssortmentSalesChannels.Import;

namespace Tamro.Madam.Application.Commands.Commerce.ItemAssortmentSalesChannels;

public class GetImportedAssortmentCommand : IRequest<Result<IEnumerable<ItemAssortmentItemModel>>>, IDefaultErrorMessage
{
    public GetImportedAssortmentCommand(ItemAssortmentImportModel model)
    {
        Model = model;
    }

    public ItemAssortmentImportModel Model { get; set; }
    public string ErrorMessage { get; set; } = "Failed to retrieve import items";
}