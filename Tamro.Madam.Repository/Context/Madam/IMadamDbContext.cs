using TamroUtilities.EFCore;

namespace Tamro.Madam.Repository.Context.Madam;

public interface IMadamDbContext : IAuditableContext
{
    Task<int> SaveChangesAsync(CancellationToken cancellationToken);
}
