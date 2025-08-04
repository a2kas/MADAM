using MediatR;
using Tamro.Madam.Models.Customers.Wholesale.Clsf;
using Tamro.Madam.Repository.Common;

namespace Tamro.Madam.Application.Queries.Customers.Wholesale.Clsf;

public class WholesaleCustomerClsfQuery : WholesaleCustomerClsfFilter, IRequest<PaginatedData<WholesaleCustomerClsfModel>>
{
}
