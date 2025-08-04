using System.ComponentModel;
using Tamro.Madam.Models.Customers.Wholesale.Clsf;
using Tamro.Madam.Models.General;

namespace Tamro.Madam.Models.Administration.Configuration.Ubl;
public class UblApiKeyEditModel
{
    [DisplayName("API key")]
    public string ApiKey { get; set; }
    public WholesaleCustomerClsfModel Customer { get; set; }
    public BalticCountry Country { get; set; }
}
