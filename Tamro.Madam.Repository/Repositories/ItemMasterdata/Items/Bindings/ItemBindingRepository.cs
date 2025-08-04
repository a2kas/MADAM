using System.Linq.Expressions;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using Tamro.Madam.Models.Commerce.ItemAssortmentSalesChannels.Import;
using Tamro.Madam.Models.Overview;
using Tamro.Madam.Repository.Common;
using Tamro.Madam.Repository.Entities.ItemMasterdata.Items.Bindings;
using Tamro.Madam.Repository.UnitOfWork;

namespace Tamro.Madam.Repository.Repositories.ItemMasterdata.Items.Bindings;

public class ItemBindingRepository : IItemBindingRepository
{
    private readonly IMadamUnitOfWork _uow;
    private readonly IMapper _mapper;

    public ItemBindingRepository(IMadamUnitOfWork uow, IMapper mapper)
    {
        _uow = uow;
        _mapper = mapper;
    }

    public async Task<IEnumerable<ItemBinding>> GetList(Expression<Func<ItemBinding, bool>> filter, List<IncludeOperation<ItemBinding>>? includes = null, bool track = true, CancellationToken cancellationToken = default)
    {
        IQueryable<ItemBinding> query = _uow.GetRepository<ItemBinding>().AsQueryable();

        if (includes != null)
        {
            foreach (var include in includes)
            {
                query = include.ApplyInclude(query);
            }
        }

        query = query.Where(filter);

        if (!track)
        {
            query = query.AsNoTracking();
        }

        return await query.ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<CompanyBindingCountModel>> GetCountByCompany(CancellationToken cancellationToken)
    {
        return await _uow.GetRepository<ItemBinding>()
            .AsReadOnlyQueryable()
            .Where(x => x.Company != null)
            .GroupBy(x => x.Company)
            .Select(group => new CompanyBindingCountModel
            {
                Company = group.Key,
                Count = group.Count()
            })
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<ItemAssortmentItemModel>> GetSalesChannelItemAssortmentOverview(IEnumerable<string> itemNumbers, IEnumerable<string> companies, CancellationToken cancellationToken)
    {
        return await _uow.GetRepository<ItemBinding>()
            .AsReadOnlyQueryable()
            .Include(x => x.Item)
            .Where(x => companies.Contains(x.Company))
            .Where(x => itemNumbers.Contains(x.LocalId))
            .ProjectTo<ItemAssortmentItemModel>(_mapper.ConfigurationProvider)
            .ToListAsync(cancellationToken);
    }

}
