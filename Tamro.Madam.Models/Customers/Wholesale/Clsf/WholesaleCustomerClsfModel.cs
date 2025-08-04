namespace Tamro.Madam.Models.Customers.Wholesale.Clsf;

public class WholesaleCustomerClsfModel
{
    public int AddressNumber { get; set; }
    public string Name { get; set; }
    public string DisplayName { get { return $"{AddressNumber} - {Name}"; } }
}
