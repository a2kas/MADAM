using MediatR;
using Tamro.Madam.Repository.Common;
using Tamro.Madam.Models.Administration.Configuration.Ubl;

namespace Tamro.Madam.Application.Queries.Administration.Configuration.Ubl;
public class UblApiKeyQuery : UblApiKeyFilter, IRequest<PaginatedData<UblApiKeyModel>>
{
    public UblApiKeySpecification Specification => new(this);
}
