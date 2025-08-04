using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Tamro.Madam.Models.ItemMasterdata.Items.SafetyStocks;
using Tamro.Madam.Repository.Entities.ItemMasterdata.Items.SafetyStocks;
using Tamro.Madam.Repository.UnitOfWork;

namespace Tamro.Madam.Repository.Repositories.ItemMasterdata.Items.SafetyStocks;

public class SafetyStockConditionRepository : ISafetyStockConditionRepository
{
    private readonly IMadamUnitOfWork _uow;
    private readonly IMapper _mapper;

    public SafetyStockConditionRepository(IMadamUnitOfWork uow, IMapper mapper)
    {
        _uow = uow;
        _mapper = mapper;
    }

    public async Task<SafetyStockConditionModel> Get(int id)
    {
        var result = await _uow.GetRepository<SafetyStockCondition>()
            .AsReadOnlyQueryable()
            .Include(x => x.SafetyStockItem)
            .SingleOrDefaultAsync(x => x.Id == id);

        return _mapper.Map<SafetyStockConditionModel>(result);
    }

    public async Task<int> DeleteMany(int[] ids, CancellationToken cancellationToken)
    {
        var repo = _uow.GetRepository<SafetyStockCondition>();

        var entities = repo
            .AsReadOnlyQueryable()
            .Where(e => ids.Contains(e.Id))
            .ToList();

        repo.DeleteMany(entities);

        return await _uow.SaveChangesAsync(cancellationToken);
    }

    public async Task<SafetyStockConditionModel> Upsert(SafetyStockConditionModel model)
    {
        var entity = _mapper.Map<SafetyStockCondition>(model);
        var trackedEntity = await _uow.GetRepository<SafetyStockCondition>().UpsertAsync(entity);
        await _uow.SaveChangesAsync();

        return _mapper.Map<SafetyStockConditionModel>(trackedEntity);
    }
}
