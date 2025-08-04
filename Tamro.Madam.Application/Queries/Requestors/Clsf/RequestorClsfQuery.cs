using MediatR;
using Tamro.Madam.Models.ItemMasterdata.Requestors;
using Tamro.Madam.Repository.Common;

namespace Tamro.Madam.Application.Queries.Requestors.Clsf;

public class RequestorClsfQuery : RequestorClsfFilter, IRequest<PaginatedData<RequestorClsfModel>>
{
    public RequestorClsfSpecification Specification => new(this);
}
