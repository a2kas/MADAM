using MediatR;
using Tamro.Madam.Application.Access;
using Tamro.Madam.Application.Commands.Commerce.ItemAssortmentSalesChannels;
using Tamro.Madam.Application.Infrastructure.Attributes;
using Tamro.Madam.Application.Models.Common;
using Tamro.Madam.Models.Commerce.ItemAssortmentSalesChannels;
using Tamro.Madam.Repository.Repositories.Commerce.ItemAssortmentSalesChannels;

namespace Tamro.Madam.Application.Handlers.Commerce.ItemAssortmentSalesChannels;

[RequiresPermission(Permissions.CanViewItemAssortmentSalesChannels)]
public class GetItemAssortmentSalesChannelCommandHandler : IRequestHandler<GetItemAssortmentSalesChannelCommand, Result<ItemAssortmentSalesChannelDetailsModel>>
{
    private readonly IItemAssortmentSalesChannelRepository _repository;

    public GetItemAssortmentSalesChannelCommandHandler(IItemAssortmentSalesChannelRepository repository)
    {
        _repository = repository;
    }

    public async Task<Result<ItemAssortmentSalesChannelDetailsModel>> Handle(GetItemAssortmentSalesChannelCommand request, CancellationToken cancellationToken)
    {
        var itemAssortmentSalesChannel = await _repository.Get(request.Id);

        return Result<ItemAssortmentSalesChannelDetailsModel>.Success(itemAssortmentSalesChannel);
    }
}
