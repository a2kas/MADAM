using Tamro.Madam.Repository.Entities.Customers.E1;

namespace Tamro.Madam.Repository.Entities.Sales.Sabis;

public class SksContractMapping
{
    public int Id { get; set; }
    public string AdditionalTaxId { get; set; }
    public int AddressNumber { get; set; }
    public string ContractTamro { get; set; }
    public string ContractSabis { get; set; }
    public DateTime RowVer { get; set; }
    public string EditedBy { get; set; }

    public Customer? Customer { get; set; }
}
