using MediatR;
using Tamro.Madam.Repository.Common;
using Tamro.Madam.Models.ItemMasterdata.Items;

namespace Tamro.Madam.Application.Queries.Items;

public class ItemQuery : ItemFilter, IRequest<PaginatedData<ItemGridModel>>
{
    public ItemSpecification Specification => new(this);
}
