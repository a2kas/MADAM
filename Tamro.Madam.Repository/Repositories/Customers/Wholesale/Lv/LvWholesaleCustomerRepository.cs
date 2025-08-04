using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using Tamro.Madam.Models.Customers.Wholesale;
using Tamro.Madam.Models.Customers.Wholesale.Clsf;
using Tamro.Madam.Repository.Common;
using Tamro.Madam.Repository.Context.Wholesale;
using Tamro.Madam.Repository.Entities.Wholesale.Lv;

namespace Tamro.Madam.Repository.Repositories.Customers.Wholesale.Lv;

public class LvWholesaleCustomerRepository : IWholesaleCustomerRepository
{
    private readonly IWhRawLvDatabaseContext _context;
    private readonly IMapper _mapper;

    public LvWholesaleCustomerRepository(IWhRawLvDatabaseContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<PaginatedData<WholesaleCustomerClsfModel>> GetClsf(IEnumerable<int> addressNumbers, WholesaleCustomerType customerType, int pageNumber, int pageSize)
    {
        var query = BuildQuery(addressNumbers, customerType);
        var count = query.Count();
        var data = query.Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ProjectTo<WholesaleCustomerClsfModel>(_mapper.ConfigurationProvider)
            .ToList();
        return new PaginatedData<WholesaleCustomerClsfModel>(data, count, pageNumber, pageSize);
    }

    public async Task<PaginatedData<WholesaleCustomerClsfModel>> GetClsf(string searchTerm, WholesaleCustomerType customerType, int pageNumber, int pageSize)
    {
        var query = BuildQuery(searchTerm, customerType);
        var count = query.Count();
        var data = query.Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ProjectTo<WholesaleCustomerClsfModel>(_mapper.ConfigurationProvider)
            .ToList();
        return new PaginatedData<WholesaleCustomerClsfModel>(data, count, pageNumber, pageSize);
    }

    public async Task<List<WholesaleCustomerModel>> GetMany(WholesaleCustomerSearchModel search)
    {
        var query = BuildQuery(search);
        var result = await query.ToListAsync();
        return _mapper.Map<List<WholesaleCustomerModel>>(result);
    }

    private IQueryable<LvWholesaleCustomer> BuildQuery(IEnumerable<int> addressNumbers, WholesaleCustomerType customerType)
    {
        var query = _context
            .Customers
            .AsNoTracking();
        if (addressNumbers.Any())
        {
            query = query.Where(x => addressNumbers.Contains(x.AddressNumber));
        }

        if (customerType == WholesaleCustomerType.LegalEntity)
        {
            query = query.Where(x => x.AddressNumber == x.AddressNumber2);
        }
        if (customerType == WholesaleCustomerType.LegalEntityBranch)
        {
            query = query.Where(x => x.AddressNumber != x.AddressNumber2);
        }

        return query;
    }

    private IQueryable<LvWholesaleCustomer> BuildQuery(string searchTerm, WholesaleCustomerType customerType)
    {
        var query = _context
            .Customers
            .AsNoTracking();

        if (!string.IsNullOrEmpty(searchTerm))
        {
            query = query.Where(x => x.AddressNumber.ToString().Contains(searchTerm) || x.MailingName.Contains(searchTerm));
        }
        if (customerType == WholesaleCustomerType.LegalEntity)
        {
            query = query.Where(x => x.AddressNumber == x.AddressNumber2);
        }
        if (customerType == WholesaleCustomerType.LegalEntityBranch)
        {
            query = query.Where(x => x.AddressNumber != x.AddressNumber2);
        }

        return query;
    }

    private IQueryable<LvWholesaleCustomer> BuildQuery(WholesaleCustomerSearchModel search)
    {
        var query = _context
            .Customers
            .AsNoTracking();

        if (search.AddressNumbers != null)
        {
            query = query.Where(x => search.AddressNumbers.Contains(x.AddressNumber));
        }

        return query;
    }
}