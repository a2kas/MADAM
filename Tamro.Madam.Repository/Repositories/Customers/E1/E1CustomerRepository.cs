using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using Tamro.Madam.Models.Customers.Wholesale.Clsf;
using Tamro.Madam.Repository.Common;
using Tamro.Madam.Repository.Context.E1Gateway;
using Tamro.Madam.Repository.Entities.Customers.E1;

namespace Tamro.Madam.Repository.Repositories.Customers.E1;

public class E1CustomerRepository : IE1CustomerRepository
{
    private readonly IE1DbContext _context;
    private readonly IMapper _mapper;

    public E1CustomerRepository(IE1DbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<PaginatedData<WholesaleCustomerClsfModel>> GetClsf(string searchTerm, int pageNumber, int pageSize)
    {
        var query = BuildQuery(searchTerm);
        var count = await query.CountAsync();
        var data = await query.Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ProjectTo<WholesaleCustomerClsfModel>(_mapper.ConfigurationProvider)
            .ToListAsync();
        return new PaginatedData<WholesaleCustomerClsfModel>(data, count, pageNumber, pageSize);
    }


    private IQueryable<Customer> BuildQuery(string searchTerm)
    {
        var query = _context
            .Customers
            .AsNoTracking();
        
        if (!string.IsNullOrEmpty(searchTerm))
        {
            query = query.Where(x => x.MailingName.Contains(searchTerm) || x.AddressNumber.ToString().Contains(searchTerm));
        }

        return query;
    }
}
