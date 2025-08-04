using Tamro.Madam.Models.General;
using Tamro.Madam.Models.Sales.CanceledOrderLines.Statistics;

namespace Tamro.Madam.Repository.Repositories.Sales;
public interface ICanceledLineStatisticRepository
{
    Task<IEnumerable<CanceledLineStatisticModel>> GetStatistics(BalticCountry country, DateTime dateFrom, DateTime dateTill);
}
