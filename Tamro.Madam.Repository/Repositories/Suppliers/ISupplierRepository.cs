using Tamro.Madam.Models.Suppliers;

namespace Tamro.Madam.Repository.Repositories.Suppliers;

public interface ISupplierRepository
{
    Task<SupplierDetailsModel> UpsertGraph(SupplierDetailsModel model);
    Task<SupplierDetailsModel> Get(int id);
}
