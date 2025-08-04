using MediatR;
using Tamro.Madam.Application.Infrastructure.Behaviors;
using Tamro.Madam.Application.Models.Common;
using Tamro.Madam.Models.Commerce.ItemAssortmentSalesChannels.Import;

namespace Tamro.Madam.Application.Commands.Commerce.ItemAssortmentSalesChannels;

public class FinalizeItemAssortmentImportCommand : IRequest<Result<ItemAssortmentImportResultModel>>, IDefaultErrorMessage
{
    public FinalizeItemAssortmentImportCommand(ItemAssortmentFinalizeImportModel model)
    {
        Model = model;
    }

    public string ErrorMessage { get; set; } = "Failed to finalize assortment import";

    public ItemAssortmentFinalizeImportModel Model { get; }
}
