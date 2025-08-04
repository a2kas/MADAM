using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Tamro.Madam.Models.ItemMasterdata.Items.Wholesale;
using Tamro.Madam.Repository.Context.Wholesale;
using Tamro.Madam.Repository.Repositories.ItemMasterdata.Items.Wholesale;

namespace Tamro.Madam.Repository.Repositories.ItemMasterdata.Items.Wholesale.Ee;

public class EeWholesaleItemAvailabilityRepository : IWholesaleItemAvailabilityRepository
{
    private readonly IWhRawEeDatabaseContext _whRawEeDatabaseContext;
    private readonly IMapper _mapper;

    public EeWholesaleItemAvailabilityRepository(IWhRawEeDatabaseContext whRawEeDatabaseContext, IMapper mapper)
    {
        _whRawEeDatabaseContext = whRawEeDatabaseContext ?? throw new ArgumentNullException(nameof(whRawEeDatabaseContext));
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
    }

    public async Task<IEnumerable<ItemAvailabilityModel>> GetAll()
    {
        var itemAvailabilities = await _whRawEeDatabaseContext.ItemAvailabilities.AsNoTracking().ToListAsync();

        return _mapper.Map<IEnumerable<ItemAvailabilityModel>>(itemAvailabilities);
    }

    public async Task<IEnumerable<ItemAvailabilityModel>> Get(List<string> itemNo2s)
    {
        var itemAvailabilities = await _whRawEeDatabaseContext.ItemAvailabilities.AsNoTracking()
            .Where(x => itemNo2s.Contains(x.ItemNo))
            .ToListAsync();

        return _mapper.Map<IEnumerable<ItemAvailabilityModel>>(itemAvailabilities);
    }
}
