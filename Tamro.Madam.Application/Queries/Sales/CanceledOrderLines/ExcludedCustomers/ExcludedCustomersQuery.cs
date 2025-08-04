using MediatR;
using Tamro.Madam.Application.Queries.Sales.CanceledOrderLines.ExcludedCustomers;
using Tamro.Madam.Models.Sales.CanceledOrderLines.ExcludedCustomers;
using Tamro.Madam.Repository.Common;

namespace Tamro.Madam.Application.Queries.Sales.CanceledOrderLines;

public class ExcludedCustomersQuery : ExcludedCustomersFilter, IRequest<PaginatedData<ExcludedCustomerGridModel>>
{
    public ExcludedCustomersSpecification Specification => new(this);
}
