using MediatR;
using Tamro.Madam.Models.Sales.HeldOrders;
using Tamro.Madam.Repository.Common;

namespace Tamro.Madam.Application.Queries.Sales.HeldOrders;
public class HeldOrderQuery : HeldOrderFilter, IRequest<PaginatedData<HeldOrderGridModel>>
{
    public HeldOrderSpecification Specification => new(this);
}
