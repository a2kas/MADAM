using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Tamro.Madam.Models.ItemMasterdata.Items.Wholesale;
using Tamro.Madam.Repository.Context.Wholesale;
using Tamro.Madam.Repository.Repositories.ItemMasterdata.Items.Wholesale;

namespace Tamro.Madam.Repository.Repositories.ItemMasterdata.Items.Wholesale.Lv;

public class LvWholesaleItemAvailabilityRepository : IWholesaleItemAvailabilityRepository
{
    private readonly IWhRawLvDatabaseContext _whRawLvDatabaseContext;
    private readonly IMapper _mapper;

    public LvWholesaleItemAvailabilityRepository(IWhRawLvDatabaseContext whRawLvDatabaseContext, IMapper mapper)
    {
        _whRawLvDatabaseContext = whRawLvDatabaseContext ?? throw new ArgumentNullException(nameof(whRawLvDatabaseContext));
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
    }

    public async Task<IEnumerable<ItemAvailabilityModel>> GetAll()
    {
        var itemAvailabilities = await _whRawLvDatabaseContext.ItemAvailabilities.AsNoTracking().ToListAsync();

        return _mapper.Map<IEnumerable<ItemAvailabilityModel>>(itemAvailabilities);
    }

    public async Task<IEnumerable<ItemAvailabilityModel>> Get(List<string> itemNo2s)
    {
        var itemAvailabilities = await _whRawLvDatabaseContext.ItemAvailabilities.AsNoTracking()
            .Where(x => itemNo2s.Contains(x.ItemNo))
            .ToListAsync();

        return _mapper.Map<IEnumerable<ItemAvailabilityModel>>(itemAvailabilities);
    }
}
