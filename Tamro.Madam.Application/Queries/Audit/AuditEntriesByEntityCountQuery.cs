using MediatR;
using Tamro.Madam.Models.Overview;

namespace Tamro.Madam.Application.Queries.Audit;

public class AuditEntriesByEntityCountQuery : IRequest<IEnumerable<AuditEntriesByEntityCountModel>>
{
}
