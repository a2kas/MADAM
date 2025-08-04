using Ardalis.Specification.EntityFrameworkCore;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Tamro.Madam.Models.ItemMasterdata.Items.SafetyStocks.PharmacyChains;
using Tamro.Madam.Repository.Entities.ItemMasterdata.Items.SafetyStocks;
using Tamro.Madam.Repository.UnitOfWork;

namespace Tamro.Madam.Repository.Repositories.ItemMasterdata.Items.SafetyStocks;

public class SafetyStockPharmacyChainRepository : ISafetyStockPharmacyChainRepository
{
    private readonly IMadamUnitOfWork _uow;
    private readonly IMapper _mapper;

    public SafetyStockPharmacyChainRepository(IMadamUnitOfWork uow, IMapper mapper)
    {
        _uow = uow;
        _mapper = mapper;
    }

    public async Task<IEnumerable<PharmacyChainModel>> GetAll()
    {
        var pharmacyChains = await _uow.GetRepository<SafetyStockPharmacyChain>()
             .AsReadOnlyQueryable()
             .ToListAsync();

        return _mapper.Map<IEnumerable<PharmacyChainModel>>(pharmacyChains);
    }
}
