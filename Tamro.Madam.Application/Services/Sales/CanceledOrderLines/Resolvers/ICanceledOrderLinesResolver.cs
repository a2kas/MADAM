using Tamro.Madam.Models.General;
using Tamro.Madam.Models.Sales.CanceledOrderLines;

namespace Tamro.Madam.Application.Services.Sales.CanceledOrderLines.Resolvers;

public interface ICanceledOrderLinesResolver
{
    int Priority { get; }
    Task Resolve(IEnumerable<CanceledOrderHeaderModel> orders, BalticCountry country);
}
