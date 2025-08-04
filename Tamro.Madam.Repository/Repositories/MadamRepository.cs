using Microsoft.EntityFrameworkCore;
using Tamro.Madam.Repository.Context.Madam;

namespace Tamro.Madam.Repository.Repositories;
public class MadamRepository
    : BaseRepository<IMadamEntity>
{ 
    public MadamRepository(DbSet<IMadamEntity> _dbSet) : base(_dbSet)
    {
    }
}