using Tamro.Madam.Models.General;
using Tamro.Madam.Models.ItemMasterdata.Items.Bindings.Retail;

namespace Tamro.Madam.Repository.Repositories.ItemMasterdata.Items.Bindings.Retail;

public interface IGeneratedRetailCodeRepository
{
    Task<long> GetLatestCode(BalticCountry country, string codePrefix);
    Task InsertMany(List<GeneratedRetailCodeModel> models, CancellationToken cancellationToken);
}
