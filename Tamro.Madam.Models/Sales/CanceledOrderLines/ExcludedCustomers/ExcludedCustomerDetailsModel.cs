using Tamro.Madam.Models.Customers.Wholesale.Clsf;
using Tamro.Madam.Models.General;

namespace Tamro.Madam.Models.Sales.CanceledOrderLines.ExcludedCustomers;

public class ExcludedCustomerDetailsModel
{
    public int? Id { get; set; }
    public WholesaleCustomerClsfModel Customer { get; set; }
    public BalticCountry Country { get; set; }
    private ExclusionLevel exclusionLevel;
    public ExclusionLevel ExclusionType 
    {
        get { return exclusionLevel; }
        set 
        { 
            exclusionLevel = value; 
        } 
    }
    public List<int> SelectedShipToAddresses { get; set; } = new();
    public List<CustomerLocationModel> AvailableLocations { get; set; } = new();
}
