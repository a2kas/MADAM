using MediatR;
using Tamro.Madam.Models.Commerce.ItemAssortmentSalesChannels;
using Tamro.Madam.Repository.Common;

namespace Tamro.Madam.Application.Queries.Commerce.ItemAssortmentSalesChannels;

public class ItemAssortmentSalesChannelQuery : ItemAssortmentSalesChannelFilter, IRequest<PaginatedData<ItemAssortmentSalesChannelGridModel>>
{
    public ItemAssortmentSalesChannelSpecification Specification => new(this);
}
