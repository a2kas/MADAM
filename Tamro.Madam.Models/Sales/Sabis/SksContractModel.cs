using System.ComponentModel;
using Tamro.Madam.Models.Customers.Wholesale.Clsf;

namespace Tamro.Madam.Models.Sales.Sabis;

public class SksContractModel
{
    public int Id { get; set; }
    [DisplayName("Company Id")]
    public string CompanyId { get; set; }
    [DisplayName("Customer")]
    public WholesaleCustomerClsfModel Customer { get; set; }
    [DisplayName("Tamro contract")]
    public string ContractTamro { get; set; }
    [DisplayName("SABIS contract")]
    public string ContractSabis { get; set; }
    [DisplayName("Last edited at")]
    public DateTime EditedAt { get; set; }
    [DisplayName("Last edited by")]
    public string EditedBy { get; set; }
    [DisplayName("Last edited")]
    public string LastEdited
    {
        get
        {
            return $"{EditedAt.ToString("yyyy-MM-dd hh:mm:ss")} by {EditedBy}";
        }
    }
}
