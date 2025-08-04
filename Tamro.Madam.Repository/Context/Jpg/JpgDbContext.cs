using Microsoft.EntityFrameworkCore;
using Tamro.Madam.Repository.Entities.Jpg;

namespace Tamro.Madam.Repository.Context.Jpg;
public class JpgDbContext : BaseDbContextFindingItsEntities<IJpgEntity>
{
    public JpgDbContext(DbContextOptions<JpgDbContext> options) : base(options)
    {
    }
}
