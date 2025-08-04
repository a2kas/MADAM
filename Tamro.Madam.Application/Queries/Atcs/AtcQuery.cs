using MediatR;
using Tamro.Madam.Models.ItemMasterdata.Atcs;
using Tamro.Madam.Repository.Common;

namespace Tamro.Madam.Application.Queries.Atcs;

public class AtcQuery : AtcFilter, IRequest<PaginatedData<AtcModel>>
{
    public AtcSpecification Specification => new(this);
}
