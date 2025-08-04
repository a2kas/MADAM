using Microsoft.EntityFrameworkCore;
using Tamro.Madam.Repository.Context.Madam;

namespace Tamro.Madam.Repository.UnitOfWork;
public class MadamUnitOfWork : BaseUnitOfWork<IMadamEntity>, IMadamUnitOfWork
{
    public MadamUnitOfWork(DbContext _context) : base(_context)
    {
    }
}
