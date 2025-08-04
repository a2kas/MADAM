using Tamro.Madam.Models.Overview;
using Tamro.Madam.Repository.Common;
using TamroUtilities.EFCore.Models;

namespace Tamro.Madam.Repository.Repositories.Audit;

public interface IAuditRepository
{
    DbAuditEntry? Get(int id, List<IncludeOperation<DbAuditEntry>> includes);
    Task<IEnumerable<ItemMonthlyEditCountModel>> GetItemAuditEntriesCountByMonthForLastYear(CancellationToken cancellationToken);
    Task<IEnumerable<AuditEntriesByEntityCountModel>> GetAuditEntriesCountByEntityType(CancellationToken cancellationToken);
}
