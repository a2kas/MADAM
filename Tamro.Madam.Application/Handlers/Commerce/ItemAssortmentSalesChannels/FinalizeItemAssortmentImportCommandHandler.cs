using MediatR;
using Tamro.Madam.Application.Access;
using Tamro.Madam.Application.Commands.Commerce.ItemAssortmentSalesChannels;
using Tamro.Madam.Application.Infrastructure.Attributes;
using Tamro.Madam.Application.Models.Common;
using Tamro.Madam.Models.Commerce.ItemAssortmentSalesChannels;
using Tamro.Madam.Models.Commerce.ItemAssortmentSalesChannels.Import;

namespace Tamro.Madam.Application.Handlers.Commerce.ItemAssortmentSalesChannels;

[RequiresPermission(Permissions.CanManageItemAssortmentSalesChannels)]
public class FinalizeItemAssortmentImportCommandHandler : IRequestHandler<FinalizeItemAssortmentImportCommand, Result<ItemAssortmentImportResultModel>>
{
    public FinalizeItemAssortmentImportCommandHandler()
    {
    }

    public async Task<Result<ItemAssortmentImportResultModel>> Handle(FinalizeItemAssortmentImportCommand request, CancellationToken cancellationToken)
    {
        var overview = new List<ItemAssortmentImportOverviewModel>();
        var newAssortment = InitializeAssortment(request);

        ProcessImportedAssortment(request, overview, newAssortment);

        if (request.Model.ImportAction == ItemAssortmentImportAction.Replace)
        {
            HandleRemovedItems(request, overview, newAssortment);
        }

        var result = new ItemAssortmentImportResultModel
        {
            Assortment = newAssortment,
            Overview = overview,
        };

        return Result<ItemAssortmentImportResultModel>.Success(result);
    }

    private static List<ItemAssortmentGridModel> InitializeAssortment(FinalizeItemAssortmentImportCommand request)
    {
        return request.Model.ImportAction == ItemAssortmentImportAction.Extend ? request.Model.ExistingAssortment.ToList() : new List<ItemAssortmentGridModel>();
    }

    private static void ProcessImportedAssortment(FinalizeItemAssortmentImportCommand request, List<ItemAssortmentImportOverviewModel> overview,List<ItemAssortmentGridModel> newAssortment)
    {
        foreach (var item in request.Model.ImportedAssortment)
        {
            if (string.IsNullOrEmpty(item.ItemName))
            {
                overview.Add(CreateOverviewModel(false, "Item was not found", item.ItemNo));
                continue;
            }

            if (!request.Model.ExistingAssortment.Any(x => x.ItemCode == item.ItemNo))
            {
                overview.Add(CreateOverviewModel(true, "Added to assortment", item.ItemNo, item.ItemName));
                newAssortment.Add(CreateAssortmentModel(request.Model.SalesChannelId, item));
            }
            else
            {
                overview.Add(CreateOverviewModel(true, "Remains in assortment", item.ItemNo, item.ItemName));

                if (request.Model.ImportAction == ItemAssortmentImportAction.Replace)
                {
                    var existingItem = request.Model.ExistingAssortment.First(x => x.ItemCode == item.ItemNo);
                    newAssortment.Add(existingItem);
                }
            }
        }
    }

    private static void HandleRemovedItems(FinalizeItemAssortmentImportCommand request, List<ItemAssortmentImportOverviewModel> overview, List<ItemAssortmentGridModel> newAssortment)
    {
        var removedItems = request.Model.ExistingAssortment.Where(x => !newAssortment.Any(y => y.ItemCode == x.ItemCode));

        foreach (var removedItem in removedItems)
        {
            overview.Add(CreateOverviewModel(true, "Removed from assortment", removedItem.ItemCode, removedItem.ItemName));
        }
    }

    private static ItemAssortmentImportOverviewModel CreateOverviewModel(bool isSuccess, string comment, string itemNo, string? itemName = null)
    {
        return new ItemAssortmentImportOverviewModel
        {
            IsSuccess = isSuccess,
            Comment = comment,
            ItemNo = itemNo,
            ItemName = itemName,
        };
    }

    private static ItemAssortmentGridModel CreateAssortmentModel(int salesChannelId, ItemAssortmentItemModel item)
    {
        return new ItemAssortmentGridModel
        {
            ItemAssortmentSalesChannelId = salesChannelId,
            ItemBindingId = item.ItemBindingId,
            ItemCode = item.ItemNo,
            ItemName = item.ItemName,
        };
    }
}