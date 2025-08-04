using MediatR;
using Tamro.Madam.Models.Sales.CanceledOrderLines;
using Tamro.Madam.Repository.Common;

namespace Tamro.Madam.Application.Queries.Sales.CanceledOrderLines;

public class CanceledOrderHeaderQuery : CanceledOrderHeaderFilter, IRequest<PaginatedData<CanceledOrderHeaderGridModel>>
{
    public CanceledOrderHeaderSpecification Specification => new(this);
}
