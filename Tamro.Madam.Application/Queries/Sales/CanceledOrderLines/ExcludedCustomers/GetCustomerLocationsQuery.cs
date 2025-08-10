using MediatR;
using Tamro.Madam.Application.Infrastructure.Behaviors;
using Tamro.Madam.Application.Models.Common;
using Tamro.Madam.Models.General;
using Tamro.Madam.Models.Sales.CanceledOrderLines.ExcludedCustomers;

namespace Tamro.Madam.Application.Queries.Sales.CanceledOrderLines.ExcludedCustomers;

public class GetCustomerLocationsQuery : IRequest<Result<IEnumerable<CustomerLocationModel>>>, IDefaultErrorMessage
{
    public GetCustomerLocationsQuery(int e1SoldTo, BalticCountry country)
    {
        E1SoldTo = e1SoldTo;
        Country = country;
    }

    public int E1SoldTo { get; set; }
    public BalticCountry Country { get; set; }
    public string ErrorMessage { get; set; } = "Failed to retrieve customer locations";
}