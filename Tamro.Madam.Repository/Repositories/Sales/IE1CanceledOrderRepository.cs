using Tamro.Madam.Models.General;
using Tamro.Madam.Models.Sales.CanceledOrderLines;

namespace Tamro.Madam.Repository.Repositories.Sales;

public interface IE1CanceledOrderRepository
{
    Task<IEnumerable<CanceledOrderHeaderModel>> GetCanceledOrders(BalticCountry country, List<CanceledOrderLineEmailStatus> statuses);
    Task<CanceledOrderHeaderModel> Update(CanceledOrderHeaderModel model);
}
