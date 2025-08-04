using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Tamro.Madam.Models.Employees.Wholesale;
using Tamro.Madam.Repository.Context.Wholesale;
using Tamro.Madam.Repository.Entities.Wholesale.Lv;

namespace Tamro.Madam.Repository.Repositories.Customers.Wholesale.Lv;
public class LvWholesaleEmployeeRepository : IWholesaleEmployeeRepository
{
    private readonly IWhRawLvDatabaseContext _context;
    private readonly IMapper _mapper;

    public LvWholesaleEmployeeRepository(IWhRawLvDatabaseContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<List<WholesaleEmployeeModel>> GetMany(WholesaleEmployeeSearchModel search)
    {
        var query = BuildQuery(search);
        var result = await query.ToListAsync();
        return _mapper.Map<List<WholesaleEmployeeModel>>(result);
    }

    private IQueryable<LvWholesaleEmployee> BuildQuery(WholesaleEmployeeSearchModel search)
    {
        var query = _context
            .Employees
            .AsNoTracking();

        if (search.AddressNumbers != null)
        {
            query = query.Where(x => search.AddressNumbers.Contains(x.AddressNumber));
        }

        return query;
    }
}
