using MediatR;
using Tamro.Madam.Models.ItemMasterdata.Nicks;
using Tamro.Madam.Repository.Common;

namespace Tamro.Madam.Application.Queries.Nicks.Clsf;

public class NickClsfQuery : NickClsfFilter, IRequest<PaginatedData<NickClsfModel>>
{
    public NickClsfSpecification Specification => new(this);
}
