using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using Tamro.Madam.Models.Suppliers;
using Tamro.Madam.Repository.Entities.Suppliers;
using Tamro.Madam.Repository.UnitOfWork;

namespace Tamro.Madam.Repository.Repositories.Suppliers;

public class SupplierRepository : ISupplierRepository
{
    private readonly IMadamUnitOfWork _uow;
    private readonly IMapper _mapper;

    public SupplierRepository(IMadamUnitOfWork uow, IMapper mapper)
    {
        _uow = uow;
        _mapper = mapper;
    }

    public async Task<SupplierDetailsModel> Get(int id)
    {
        return await _uow.GetRepository<Supplier>()
            .AsReadOnlyQueryable()
            .Include(x => x.Contracts)
            .AsNoTracking()
            .ProjectTo<SupplierDetailsModel>(_mapper.ConfigurationProvider)
            .SingleOrDefaultAsync(x => x.Id == id);
    }

    public async Task<SupplierDetailsModel> UpsertGraph(SupplierDetailsModel model)
    {
        var repo = _uow.GetRepository<Supplier>();
        if (model.Id == default)
        {
            repo.Create(_mapper.Map<Supplier>(model));
        }
        else
        {
            var entity = await repo
                .AsQueryable()
                .Include(x => x.Contracts)
                .AsTracking()
                .FirstOrDefaultAsync(x => x.Id == model.Id);
            _mapper.Map(model, entity);
        }
        await _uow.SaveChangesAsync();

        return _mapper.Map<SupplierDetailsModel>(model);
    }
}
