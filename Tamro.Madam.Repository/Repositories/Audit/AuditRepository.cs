using Microsoft.EntityFrameworkCore;
using Tamro.Madam.Models.Overview;
using Tamro.Madam.Repository.Common;
using Tamro.Madam.Repository.Context.Madam;
using Tamro.Madam.Repository.Entities.ItemMasterdata.Items;
using TamroUtilities.EFCore.Models;

namespace Tamro.Madam.Repository.Repositories.Audit;

public class AuditRepository : IAuditRepository
{
    private readonly IMadamDbContext _context;

    public AuditRepository(IMadamDbContext context)
    {
        _context = context;
    }

    public DbAuditEntry? Get(int id, List<IncludeOperation<DbAuditEntry>> includes)
    {
        IQueryable<DbAuditEntry> query = _context.AuditEntry.AsQueryable();

        if (includes != null)
        {
            foreach (var include in includes)
            {
                query = include.ApplyInclude(query);
            }
        }

        return query
            .AsNoTracking()
            .SingleOrDefault(x => x.AuditEntryID == id);
    }

    public async Task<IEnumerable<ItemMonthlyEditCountModel>> GetItemAuditEntriesCountByMonthForLastYear(CancellationToken cancellationToken)
    {
        var startDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1).AddYears(-1).Date;

        return await _context.AuditEntry
            .AsNoTracking()
            .Where(x => x.CreatedDate >= startDate && x.EntityTypeName == nameof(Item))
            .GroupBy(x => new { x.CreatedDate.Year, x.CreatedDate.Month })
            .Select(g => new ItemMonthlyEditCountModel
            {
                Year = g.Key.Year,
                Month = g.Key.Month,
                Count = g.Count(),
            })
            .OrderBy(r => r.Year)
            .ThenBy(r => r.Month)
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<AuditEntriesByEntityCountModel>> GetAuditEntriesCountByEntityType(CancellationToken cancellationToken)
    {
        return await _context.AuditEntry
            .AsNoTracking()
            .GroupBy(x => x.EntityTypeName)
            .Select(g => new AuditEntriesByEntityCountModel
            {
                EntityName = g.Key,
                Count = g.Count(),
            })
            .ToListAsync(cancellationToken);
    }
}
