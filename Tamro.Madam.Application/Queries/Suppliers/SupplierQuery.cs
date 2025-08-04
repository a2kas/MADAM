using MediatR;
using Tamro.Madam.Models.Suppliers;
using Tamro.Madam.Repository.Common;

namespace Tamro.Madam.Application.Queries.Suppliers;

public class SupplierQuery : SupplierFilter, IRequest<PaginatedData<SupplierGridModel>>
{
    public SupplierSpecification Specification => new(this);
}
