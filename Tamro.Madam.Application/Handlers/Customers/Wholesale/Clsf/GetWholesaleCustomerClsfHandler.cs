using MediatR;
using Tamro.Madam.Application.Infrastructure.Attributes;
using Tamro.Madam.Application.Queries.Customers.Wholesale.Clsf;
using Tamro.Madam.Application.Services.Customers.Factories;
using Tamro.Madam.Models.Customers.Wholesale.Clsf;
using Tamro.Madam.Repository.Common;

namespace Tamro.Madam.Application.Handlers.Customers.Wholesale.Clsf;

[NoPermissionNeeded]
public class GetWholesaleCustomerClsfHandler : IRequestHandler<WholesaleCustomerClsfQuery, PaginatedData<WholesaleCustomerClsfModel>>
{
    private readonly IWholesaleCustomerRepositoryFactory _wholesaleCustomerRepositoryFactory;

    public GetWholesaleCustomerClsfHandler(IWholesaleCustomerRepositoryFactory wholesaleCustomerRepositoryFactory)
    {
        _wholesaleCustomerRepositoryFactory = wholesaleCustomerRepositoryFactory;
    }

    public async Task<PaginatedData<WholesaleCustomerClsfModel>> Handle(WholesaleCustomerClsfQuery request, CancellationToken cancellationToken)
    {
        var repository = _wholesaleCustomerRepositoryFactory.Get(request.Country);

        return await repository.GetClsf(searchTerm: request.SearchTerm, customerType: request.CustomerType, 1, 20);
    }
}
