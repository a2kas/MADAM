using System.ComponentModel;
using Tamro.Madam.Models.Common;

namespace Tamro.Madam.Models.Suppliers;

public class SupplierGridModel : BaseDataGridModel<SupplierGridModel>
{
    public int Id { get; set; }
    [DisplayName("Registration number")]
    public string RegistrationNumber { get; set; }
    public string Name { get; set; }
    [DisplayName("Created date")]
    public DateTime CreatedDate { get; set; }
}
