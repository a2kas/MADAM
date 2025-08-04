using MediatR;
using Tamro.Madam.Application.Infrastructure.Attributes;
using Tamro.Madam.Application.Queries.Customers.E1.Clsf;
using Tamro.Madam.Models.Customers.Wholesale.Clsf;
using Tamro.Madam.Repository.Common;
using Tamro.Madam.Repository.Repositories.Customers.E1;

namespace Tamro.Madam.Application.Handlers.Customers.E1.Clsf;

[NoPermissionNeeded]
public class GetE1CustomerClsfHandler : IRequestHandler<E1CustomerClsfQuery, PaginatedData<WholesaleCustomerClsfModel>>
{
    private readonly IE1CustomerRepository _e1CustomerRepository;
    public GetE1CustomerClsfHandler(IE1CustomerRepository e1CustomerRepository)
    {
        _e1CustomerRepository = e1CustomerRepository;
    }

    public async Task<PaginatedData<WholesaleCustomerClsfModel>> Handle(E1CustomerClsfQuery request, CancellationToken cancellationToken)
    {
        return await _e1CustomerRepository.GetClsf(request.SearchTerm, 1, 20);
    }
}
