using MediatR;
using Tamro.Madam.Application.Infrastructure.Attributes;
using Tamro.Madam.Application.Queries.Audit;
using Tamro.Madam.Models.Overview;
using Tamro.Madam.Repository.Repositories.Audit;

namespace Tamro.Madam.Application.Handlers.Audit;

[NoPermissionNeeded]
public class ItemMonthlyEditCountQueryHandler : IRequestHandler<ItemMonthlyEditCountQuery, IEnumerable<ItemMonthlyEditCountModel>>
{
    private readonly IAuditRepository _auditRepository;

    public ItemMonthlyEditCountQueryHandler(IAuditRepository auditRepository)
    {
        _auditRepository = auditRepository;
    }

    public Task<IEnumerable<ItemMonthlyEditCountModel>> Handle(ItemMonthlyEditCountQuery request, CancellationToken cancellationToken)
    {
        return _auditRepository.GetItemAuditEntriesCountByMonthForLastYear(cancellationToken);
    }
}
