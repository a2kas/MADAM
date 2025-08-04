using MediatR;
using Tamro.Madam.Application.Infrastructure.Attributes;
using Tamro.Madam.Application.Queries.Audit;
using Tamro.Madam.Models.Overview;
using Tamro.Madam.Repository.Repositories.Audit;

namespace Tamro.Madam.Application.Handlers.Audit;

[NoPermissionNeeded]
public class AuditEntriesByEntityCountQueryHandler : IRequestHandler<AuditEntriesByEntityCountQuery, IEnumerable<AuditEntriesByEntityCountModel>>
{
    private readonly IAuditRepository _auditRepository;

    public AuditEntriesByEntityCountQueryHandler(IAuditRepository auditRepository)
    {
        _auditRepository = auditRepository;
    }

    public Task<IEnumerable<AuditEntriesByEntityCountModel>> Handle(AuditEntriesByEntityCountQuery request, CancellationToken cancellationToken)
    {
        return _auditRepository.GetAuditEntriesCountByEntityType(cancellationToken);
    }
}
