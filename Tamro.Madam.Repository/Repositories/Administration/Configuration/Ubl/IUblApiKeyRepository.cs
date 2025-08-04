using Tamro.Madam.Models.Administration.Configuration.Ubl;

namespace Tamro.Madam.Repository.Repositories.Administration.Configuration.Ubl;
public interface IUblApiKeyRepository
{
    Task<UblApiKeyModel> Upsert(UblApiKeyModel model);
    Task<int> DeleteMany(IEnumerable<int> e1SoldTos, CancellationToken cancellationToken);
}
