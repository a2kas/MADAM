using System.ComponentModel;
using Tamro.Madam.Models.General;

namespace Tamro.Madam.Models.Suppliers;

public class SupplierDetailsModel
{
    public int Id { get; set; }
    [DisplayName("Registration number")]
    public string RegistrationNumber { get; set; }
    public BalticCountry Country { get; set; }
    public string Name { get; set; }
    public List<SupplierContractModel> Contracts { get; set; } = new();
}
