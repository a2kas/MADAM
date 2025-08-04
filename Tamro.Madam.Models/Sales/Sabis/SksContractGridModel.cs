using System.ComponentModel;
using Tamro.Madam.Models.Common;

namespace Tamro.Madam.Models.Sales.Sabis;

public class SksContractGridModel : BaseDataGridModel<SksContractGridModel>
{
    public int Id { get; set; }
    [DisplayName("Customer name")]
    public string CustomerName { get; set; }
    [DisplayName("Company Id")]
    public string CompanyId { get; set; }
    [DisplayName("Address number")]
    public int AddressNumber { get; set; }
    [DisplayName("Tamro contract")]
    public string ContractTamro { get; set; }
    [DisplayName("SABIS contract")]
    public string ContractSabis { get; set; }
    [DisplayName("Last edited at")]
    public DateTime EditedAt { get; set; }
    [DisplayName("Last edited by")]
    public string EditedBy { get; set; }
}
