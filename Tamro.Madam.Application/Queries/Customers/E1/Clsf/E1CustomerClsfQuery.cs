using MediatR;
using Tamro.Madam.Models.Customers.Wholesale.Clsf;
using Tamro.Madam.Repository.Common;

namespace Tamro.Madam.Application.Queries.Customers.E1.Clsf;

public class E1CustomerClsfQuery : E1CustomerClsfFilter, IRequest<PaginatedData<WholesaleCustomerClsfModel>>
{
}
