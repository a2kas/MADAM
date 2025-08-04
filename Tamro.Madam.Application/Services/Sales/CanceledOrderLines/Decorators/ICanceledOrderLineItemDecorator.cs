using Tamro.Madam.Models.General;
using Tamro.Madam.Models.Sales.CanceledOrderLines;

namespace Tamro.Madam.Application.Services.Sales.CanceledOrderLines.Decorators;
public interface ICanceledOrderLineItemDecorator
{
    Task Decorate(IEnumerable<IItemDetailsModel> lines, BalticCountry country);
}
