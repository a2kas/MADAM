using AutoMapper;
using EFCore.BulkExtensions;
using Tamro.Madam.Models.ItemMasterdata.Items.SafetyStocks;
using Tamro.Madam.Repository.Entities.ItemMasterdata.Items.SafetyStocks;
using Tamro.Madam.Repository.UnitOfWork;

namespace Tamro.Madam.Repository.Repositories.ItemMasterdata.Items.SafetyStocks;

public class SafetyStockRepository : ISafetyStockRepository
{
    private readonly IMadamUnitOfWork _uow;
    private readonly IMapper _mapper;

    public SafetyStockRepository(IMadamUnitOfWork uow, IMapper mapper)
    {
        _uow = uow;
        _mapper = mapper;
    }

    public async Task<IEnumerable<SafetyStockModel>> UpsertBulkRange(IEnumerable<SafetyStockModel> models)
    {
        var repo = _uow.GetRepository<SafetyStock>();
        var entities = _mapper.Map<IEnumerable<SafetyStock>>(models);

        await DbContextBulkExtensions.BulkInsertOrUpdateAsync(_uow.Context, entities);

        return _mapper.Map<IEnumerable<SafetyStockModel>>(entities);
    }
}
