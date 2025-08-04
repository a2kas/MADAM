using MediatR;
using Tamro.Madam.Application.Access;
using Tamro.Madam.Application.Infrastructure.Attributes;
using Tamro.Madam.Application.Queries.Items.Wholesale.Clsf;
using Tamro.Madam.Application.Services.Items.Wholesale.Factories;
using Tamro.Madam.Models.ItemMasterdata.Items.Wholesale.Clsf;
using Tamro.Madam.Repository.Common;

namespace Tamro.Madam.Application.Handlers.ItemMasterdata.Items.Wholesale.Clsf;

[RequiresPermission(Permissions.CanViewSafetyStock)]
public class GetWholesaleItemClsfHandler : IRequestHandler<WholesaleItemClsfQuery, PaginatedData<WholesaleItemClsfModel>>
{
    private readonly IWholesaleItemRepositoryFactory _wholesaleItemRepositoryFactory;

    public GetWholesaleItemClsfHandler(IWholesaleItemRepositoryFactory wholesaleItemRepositoryFactory)
    {
        _wholesaleItemRepositoryFactory = wholesaleItemRepositoryFactory;
    }

    public async Task<PaginatedData<WholesaleItemClsfModel>> Handle(WholesaleItemClsfQuery request, CancellationToken cancellationToken)
    {
        var repository = _wholesaleItemRepositoryFactory.Get(request.Country);
        if (request.ItemNo2.Count() > 0)
        {
            return await repository.GetClsf(request.ItemNo2, 1, int.MaxValue);
        }
        return await repository.GetClsf(request.SearchTerm, 1, 20);
    }
}
