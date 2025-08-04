using AutoMapper;
using Tamro.Madam.Models.General;
using Tamro.Madam.Models.ItemMasterdata.Items.Bindings.Retail;
using Tamro.Madam.Repository.Entities.ItemMasterdata.Items.Bindings.Retail;
using Tamro.Madam.Repository.UnitOfWork;
using Microsoft.EntityFrameworkCore;

namespace Tamro.Madam.Repository.Repositories.ItemMasterdata.Items.Bindings.Retail;

public class GeneratedRetailCodeRepository : IGeneratedRetailCodeRepository
{
    private readonly IMadamUnitOfWork _uow;
    private readonly IMapper _mapper;

    public GeneratedRetailCodeRepository(IMadamUnitOfWork uow, IMapper mapper)
    {
        _uow = uow;
        _mapper = mapper;
    }

    public async Task<long> GetLatestCode(BalticCountry country, string codePrefix)
    {
        var latestCode = await _uow.GetRepository<GeneratedRetailCode>()
            .AsReadOnlyQueryable()
            .Where(x => x.Country == country && x.CodePrefix == codePrefix)
            .OrderByDescending(x => x.Code)
            .FirstOrDefaultAsync();

        return latestCode?.Code ?? 0;
    }

    public async Task InsertMany(List<GeneratedRetailCodeModel> models, CancellationToken cancellationToken)
    {
        var entities = _mapper.Map<List<GeneratedRetailCode>>(models);
        var repo = _uow.GetRepository<GeneratedRetailCode>();

        repo.CreateMany(entities);

        await _uow.SaveChangesAsync(cancellationToken);
    }
}
