using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Tamro.Madam.Models.Administration.Configuration.Ubl;
using Tamro.Madam.Repository.Context.E1Gateway;
using Tamro.Madam.Repository.Entities.Administration.Configuration.Ubl;
using TamroUtilities.EFCore.Repository;

namespace Tamro.Madam.Repository.Repositories.Administration.Configuration.Ubl;
public class UblApiKeyRepository : BaseRepository<UblApiKeyModel, UblApiKey, E1GatewayDbContext>, IUblApiKeyRepository
{
    public UblApiKeyRepository(E1GatewayDbContext context, IMapper mapper) : base(context, mapper)
    {
    }

    public async Task<int> DeleteMany(IEnumerable<int> e1SoldTos, CancellationToken cancellationToken)
    {
        var entities = await DbContext.UblApiKeys
            .Where(x => e1SoldTos.Contains(x.E1SoldTo))
            .ToListAsync(cancellationToken);

        foreach (var entity in entities)
        {
            DbContext.UblApiKeys.Remove(entity);
        }

        var result = await DbContext.SaveChangesAsync(cancellationToken);

        return result;
    }

    protected override Task<UblApiKey> GetEntity(UblApiKeyModel model)
    {
        return DbContext.UblApiKeys.SingleOrDefaultAsync(x => x.E1SoldTo == model.E1SoldTo);
    }
}
