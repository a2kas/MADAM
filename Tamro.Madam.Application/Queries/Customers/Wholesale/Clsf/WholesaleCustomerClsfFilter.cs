using Tamro.Madam.Models.Customers.Wholesale;
using Tamro.Madam.Models.General;

namespace Tamro.Madam.Application.Queries.Customers.Wholesale.Clsf;

public class WholesaleCustomerClsfFilter
{
    public string SearchTerm { get; set; }
    public BalticCountry Country { get; set; }
    public WholesaleCustomerType CustomerType { get; set; }
}
