using MediatR;
using Tamro.Madam.Models.ItemMasterdata.Requestors;
using Tamro.Madam.Repository.Common;

namespace Tamro.Madam.Application.Queries.Requestors;

public class RequestorQuery : RequestorFilter, IRequest<PaginatedData<RequestorModel>>
{
    public RequestorSpecification Specification => new(this);
}
