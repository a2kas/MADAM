using MediatR;
using Tamro.Madam.Models.ItemMasterdata.Items;
using Tamro.Madam.Repository.Common;

namespace Tamro.Madam.Application.Queries.Items.Clsf;

public class ItemsClsfQuery : ItemsClsfFilter, IRequest<PaginatedData<ItemClsfModel>>
{
    public ItemsClsfSpecification Specification => new(this);
}
