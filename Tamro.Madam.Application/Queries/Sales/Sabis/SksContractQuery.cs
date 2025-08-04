using MediatR;
using Tamro.Madam.Models.Sales.Sabis;
using Tamro.Madam.Repository.Common;

namespace Tamro.Madam.Application.Queries.Sales.Sabis;

public class SksContractQuery : SksContractFilter, IRequest<PaginatedData<SksContractGridModel>>
{
    public SksContractSpecification Specification => new(this);
}
