using MediatR;
using Tamro.Madam.Models.ItemMasterdata.Items.Bindings.Vlk;
using Tamro.Madam.Repository.Common;

namespace Tamro.Madam.Application.Queries.Items.Bindings.Vlk;

public class VlkBindingQuery : VlkBindingFilter, IRequest<PaginatedData<VlkBindingGridModel>>
{
    public VlkBindingSpecification Specification => new(this);
}
