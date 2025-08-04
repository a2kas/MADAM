using System.Linq.Expressions;
using AutoMapper;
using EFCore.BulkExtensions;
using Microsoft.EntityFrameworkCore;
using Tamro.Madam.Models.ItemMasterdata.Items.SafetyStocks;
using Tamro.Madam.Repository.Common;
using Tamro.Madam.Repository.Entities.ItemMasterdata.Items.SafetyStocks;
using Tamro.Madam.Repository.UnitOfWork;

namespace Tamro.Madam.Repository.Repositories.ItemMasterdata.Items.SafetyStocks;

public class SafetyStockItemRepository : ISafetyStockItemRepository
{
    private readonly IMadamUnitOfWork _uow;
    private readonly IMapper _mapper;

    public SafetyStockItemRepository(IMadamUnitOfWork uow, IMapper mapper)
    {
        _uow = uow;
        _mapper = mapper;
    }

    public async Task<IEnumerable<SafetyStockItemModel>> GetMany(Expression<Func<SafetyStockItem, bool>>? filter, List<IncludeOperation<SafetyStockItem>>? includes = null, bool track = true, CancellationToken cancellationToken = default)
    {
        var query = _uow.GetRepository<SafetyStockItem>()
            .AsQueryable(); 

        if (includes != null)
        {
            foreach (var include in includes)
            {
                query = include.ApplyInclude(query);
            }
        }

        if (filter != null)
        {
            query = query.Where(filter);
        }

        if (!track)
        {
            query = query.AsNoTracking();
        }

        var result = await query.ToListAsync(cancellationToken);

        return _mapper.Map<IEnumerable<SafetyStockItemModel>>(result);
    }

    public async Task<SafetyStockItemModel> Get(Expression<Func<SafetyStockItem, bool>>? filter, List<IncludeOperation<SafetyStockItem>>? includes = null, bool track = true, CancellationToken cancellationToken = default)
    {
        var query = GetQuery(filter, includes, track);
        var result = await query.SingleOrDefaultAsync(cancellationToken);

        return _mapper.Map<SafetyStockItemModel>(result);
    }

    public async Task<SafetyStockItemModel> UpsertGraph(SafetyStockItemModel model)
    {
        var repo = _uow.GetRepository<SafetyStockItem>();
        if (model.Id == default)
        {
            repo.Create(_mapper.Map<SafetyStockItem>(model));
        }
        else
        {
            var entity = await repo
                .AsQueryable()
                .Include(x => x.SafetyStockConditions)
                .Include(x => x.SafetyStock)
                .AsTracking()
                .FirstOrDefaultAsync(x => x.Id == model.Id);
            _mapper.Map(model, entity);
        }
        await _uow.SaveChangesAsync();

        return _mapper.Map<SafetyStockItemModel>(model);
    }

    public async Task<int> DeleteMany(Expression<Func<SafetyStockItem, bool>> filter, CancellationToken cancellationToken)
    {
        var repo = _uow.GetRepository<SafetyStockItem>();

        var entities = repo
            .AsReadOnlyQueryable()
            .Where(filter)
            .ToList();

        repo.DeleteMany(entities);

        return await _uow.SaveChangesAsync(cancellationToken);
    }

    private IQueryable<SafetyStockItem> GetQuery(Expression<Func<SafetyStockItem, bool>> filter, List<IncludeOperation<SafetyStockItem>>? includes = null, bool track = false)
    {
        var query = _uow.GetRepository<SafetyStockItem>()
            .AsQueryable();

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

        return query;
    }

    public async Task<IEnumerable<SafetyStockItemModel>> CreateRange(IEnumerable<SafetyStockItemModel> models)
    {
        var repo = _uow.GetRepository<SafetyStockItem>();

        var entities = _mapper.Map<IEnumerable<SafetyStockItem>>(models);
        repo.CreateMany(entities);
        await _uow.SaveChangesAsync();

        return _mapper.Map<IEnumerable<SafetyStockItemModel>>(models);
    }

    public async Task<IEnumerable<SafetyStockItemModel>> UpsertBulkRange(IEnumerable<SafetyStockItemModel> models, BulkConfig config)
    {
        var repo = _uow.GetRepository<SafetyStockItem>();
        var entities = _mapper.Map<IEnumerable<SafetyStockItem>>(models);

        await DbContextBulkExtensions.BulkInsertOrUpdateAsync(_uow.Context, entities, config);

        return _mapper.Map<IEnumerable<SafetyStockItemModel>>(entities);
    }
}
