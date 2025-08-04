using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using Tamro.Madam.Models.ItemMasterdata.Items.Wholesale;
using Tamro.Madam.Models.ItemMasterdata.Items.Wholesale.Clsf;
using Tamro.Madam.Repository.Common;
using Tamro.Madam.Repository.Context.Wholesale;
using Tamro.Madam.Repository.Entities.Wholesale.Lv;

namespace Tamro.Madam.Repository.Repositories.ItemMasterdata.Items.Wholesale.Lv;

public class LvWholesaleItemRepository : IWholesaleItemRepository
{
    private readonly IWhRawLvDatabaseContext _whRawLvDatabaseContext;
    private readonly IMapper _mapper;

    public LvWholesaleItemRepository(IWhRawLvDatabaseContext whRawLvDatabaseContext, IMapper mapper)
    {
        _whRawLvDatabaseContext = whRawLvDatabaseContext ?? throw new ArgumentNullException(nameof(whRawLvDatabaseContext));
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
    }
    public async Task<PaginatedData<WholesaleItemClsfModel>> GetClsf(string searchTerm, int pageNumber, int pageSize)
    {
        var query = BuildQuery(searchTerm);
        var count = await query.CountAsync();
        var data = await query.Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ProjectTo<WholesaleItemClsfModel>(_mapper.ConfigurationProvider)
            .ToListAsync();
        return new PaginatedData<WholesaleItemClsfModel>(data, count, pageNumber, pageSize);
    }

    public async Task<PaginatedData<WholesaleItemClsfModel>> GetClsf(List<string> itemNo2s, int pageNumber, int pageSize)
    {
        var query = BuildQuery(itemNo2s);
        var count = await query.CountAsync();
        var data = await query.Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ProjectTo<WholesaleItemClsfModel>(_mapper.ConfigurationProvider)
            .ToListAsync();
        return new PaginatedData<WholesaleItemClsfModel>(data, count, pageNumber, pageSize);
    }

    public async Task<List<WholesaleItemModel>> GetMany(WholesaleItemSearchModel search)
    {
        var query = BuildQuery(search);

        var result = await query.ToListAsync();

        return _mapper.Map<List<WholesaleItemModel>>(result);
    }

    private IQueryable<LvWholesaleItem> BuildQuery(string searchTerm)
    {
        var query = _whRawLvDatabaseContext
            .Items
            .AsNoTracking();
        if (!string.IsNullOrEmpty(searchTerm))
        {
            query = query.Where(x => x.ItemDescription.Contains(searchTerm) || x.ItemNo.Contains(searchTerm));
        }
        return query;
    }

    private IQueryable<LvWholesaleItem> BuildQuery(List<string> itemNo2s)
    {
        var query = _whRawLvDatabaseContext
            .Items
            .AsNoTracking();
        query = query.Where(x => itemNo2s.Contains(x.ItemNo));
        return query;
    }

    private IQueryable<LvWholesaleItem> BuildQuery(WholesaleItemSearchModel search)
    {
        var query = _whRawLvDatabaseContext
            .Items
            .AsNoTracking();

        if (search.ItemNo2s != null)
        {
            query = query.Where(x => search.ItemNo2s.Contains(x.ItemNo));
        }

        return query;
    }
}