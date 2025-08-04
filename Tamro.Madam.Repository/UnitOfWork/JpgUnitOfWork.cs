using Microsoft.EntityFrameworkCore;
using Tamro.Madam.Repository.Entities.Jpg;

namespace Tamro.Madam.Repository.UnitOfWork;
public class JpgUnitOfWork : BaseUnitOfWork<IJpgEntity>, IJpgUnitOfWork
{
    public JpgUnitOfWork(DbContext _context) : base(_context)
    {
    }
}
