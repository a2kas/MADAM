using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Tamro.Madam.Models.ItemMasterdata.Items.Wholesale;
using Tamro.Madam.Repository.Context.Wholesale;
using Tamro.Madam.Repository.Repositories.ItemMasterdata.Items.Wholesale;

namespace Tamro.Madam.Repository.Repositories.ItemMasterdata.Items.Wholesale.Lt;

public class LtWholesaleItemAvailabilityRepository : IWholesaleItemAvailabilityRepository
{
    private readonly IWhRawLtDatabaseContext _whRawLtDatabaseContext;
    private readonly IMapper _mapper;

    public LtWholesaleItemAvailabilityRepository(IWhRawLtDatabaseContext whRawLtDatabaseContext, IMapper mapper)
    {
        _whRawLtDatabaseContext = whRawLtDatabaseContext ?? throw new ArgumentNullException(nameof(whRawLtDatabaseContext));
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
    }

    public async Task<IEnumerable<ItemAvailabilityModel>> GetAll()
    {
        var itemAvailabilities = await _whRawLtDatabaseContext.ItemAvailabilities.AsNoTracking().ToListAsync();

        return _mapper.Map<IEnumerable<ItemAvailabilityModel>>(itemAvailabilities);
    }

    public async Task<IEnumerable<ItemAvailabilityModel>> Get(List<string> itemNo2s)
    {
        var itemAvailabilities = await _whRawLtDatabaseContext.ItemAvailabilities.AsNoTracking()
            .Where(x => itemNo2s.Contains(x.ItemNo))
            .ToListAsync();

        return _mapper.Map<IEnumerable<ItemAvailabilityModel>>(itemAvailabilities);
    }
}
