using MediatR;
using Tamro.Madam.Repository.Common;
using Tamro.Madam.Models.ItemMasterdata.Atcs;

namespace Tamro.Madam.Application.Queries.Atcs.Clsf;

public class AtcClsfQuery : AtcClsfFilter, IRequest<PaginatedData<AtcClsfModel>>
{
    public AtcClsfSpecification Specification => new(this);
}
