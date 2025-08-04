using MediatR;
using Tamro.Madam.Models.ItemMasterdata.Nicks;
using Tamro.Madam.Repository.Common;

namespace Tamro.Madam.Application.Queries.Nicks;

public class NickQuery : NickFilter, IRequest<PaginatedData<NickModel>>
{
    public NickSpecification Specification => new(this);
}
