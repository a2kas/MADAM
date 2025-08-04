using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using Tamro.Madam.Models.Commerce.ItemAssortmentSalesChannels;
using Tamro.Madam.Repository.Entities.Commerce.Assortment;
using Tamro.Madam.Repository.UnitOfWork;

namespace Tamro.Madam.Repository.Repositories.Commerce.ItemAssortmentSalesChannels;

public class ItemAssortmentSalesChannelRepository : IItemAssortmentSalesChannelRepository
{
    private readonly IMadamUnitOfWork _uow;
    private readonly IMapper _mapper;

    public ItemAssortmentSalesChannelRepository(IMadamUnitOfWork uow, IMapper mapper)
    {
        _uow = uow;
        _mapper = mapper;
    }

    public Task<ItemAssortmentSalesChannelDetailsModel> Get(int id)
    {
        return _uow.GetRepository<ItemAssortmentSalesChannel>()
            .AsReadOnlyQueryable()
            .Include(x => x.ItemAssortmentBindingMaps)
            .ThenInclude(x => x.ItemBinding)
            .ThenInclude(x => x.Item)
            .AsNoTracking()
            .ProjectTo<ItemAssortmentSalesChannelDetailsModel>(_mapper.ConfigurationProvider)
            .SingleOrDefaultAsync(x => x.Id == id);
    }

    public async Task<ItemAssortmentSalesChannelDetailsModel> UpsertGraph(ItemAssortmentSalesChannelDetailsModel model)
    {
        var repo = _uow.GetRepository<ItemAssortmentSalesChannel>();
        if (model.Id == default)
        {
            repo.Create(_mapper.Map<ItemAssortmentSalesChannel>(model));
        }
        else
        {
            var entity = await repo
                .AsQueryable()
                .Include(x => x.ItemAssortmentBindingMaps)
                .AsTracking()
                .FirstOrDefaultAsync(x => x.Id == model.Id);
            _mapper.Map(model, entity);
        }
        await _uow.SaveChangesAsync();

        return _mapper.Map<ItemAssortmentSalesChannelDetailsModel>(model);
    }
}
