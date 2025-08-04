using MediatR;
using Tamro.Madam.Models.ItemMasterdata.Items.Bindings;
using Tamro.Madam.Repository.Common;

namespace Tamro.Madam.Application.Queries.Items.Bindings.Clsf;

public class ItemBindingClsfQuery : ItemBindingClsfFilter, IRequest<PaginatedData<ItemBindingClsfModel>>
{
    public ItemBindingClsfSpecification Specification => new(this);
}
