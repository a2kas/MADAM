using Tamro.Madam.Models.Customers.Wholesale.Clsf;
using Tamro.Madam.Models.General;

namespace Tamro.Madam.Models.Sales.CanceledOrderLines.ExcludedCustomers;

public class ExcludedCustomerDetailsModel
{
    public WholesaleCustomerClsfModel Customer { get; set; }
    public BalticCountry Country { get; set; }
    public ExclusionLevel ExclusionType { get; set; }
}
