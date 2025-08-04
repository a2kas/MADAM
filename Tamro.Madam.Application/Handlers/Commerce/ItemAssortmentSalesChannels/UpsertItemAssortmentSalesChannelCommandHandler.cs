using MediatR;
using Tamro.Madam.Application.Access;
using Tamro.Madam.Application.Commands.Commerce.ItemAssortmentSalesChannels;
using Tamro.Madam.Application.Infrastructure.Attributes;
using Tamro.Madam.Application.Models.Common;
using Tamro.Madam.Models.Commerce.ItemAssortmentSalesChannels;
using Tamro.Madam.Repository.Repositories.Commerce.ItemAssortmentSalesChannels;

namespace Tamro.Madam.Application.Handlers.Commerce.ItemAssortmentSalesChannels;

[RequiresPermission(Permissions.CanManageItemAssortmentSalesChannels)]
public class UpsertItemAssortmentSalesChannelCommandHandler : IRequestHandler<UpsertItemAssortmentSalesChannelCommand, Result<ItemAssortmentSalesChannelDetailsModel>>
{
    private readonly IItemAssortmentSalesChannelRepository _repository;

    public UpsertItemAssortmentSalesChannelCommandHandler(IItemAssortmentSalesChannelRepository repository)
    {
        _repository = repository;
    }

    public async Task<Result<ItemAssortmentSalesChannelDetailsModel>> Handle(UpsertItemAssortmentSalesChannelCommand request, CancellationToken cancellationToken)
    {
        var upsertedItemAssortmentSalesChannel = await _repository.UpsertGraph(request.Model);

        return Result<ItemAssortmentSalesChannelDetailsModel>.Success(upsertedItemAssortmentSalesChannel);
    }
}
