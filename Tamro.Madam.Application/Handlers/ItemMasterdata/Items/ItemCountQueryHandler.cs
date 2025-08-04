using MediatR;
using Tamro.Madam.Application.Extensions;
using Tamro.Madam.Application.Infrastructure.Attributes;
using Tamro.Madam.Application.Queries.Items;
using Tamro.Madam.Repository.Entities.ItemMasterdata.Items;
using Tamro.Madam.Repository.UnitOfWork;

namespace Tamro.Madam.Application.Handlers.ItemMasterdata.Items;

[NoPermissionNeeded]
public class ItemCountQueryHandler : IRequestHandler<ItemCountQuery, int>
{
    private readonly IMadamUnitOfWork _uow;

    public ItemCountQueryHandler(IMadamUnitOfWork uow)
    {
        _uow = uow;
    }

    public async Task<int> Handle(ItemCountQuery request, CancellationToken cancellationToken)
    {
        var query = _uow.GetRepository<Item>().AsReadOnlyQueryable();

        return await query.Count(request.Specification, cancellationToken);
    }
}