using System.ComponentModel;
using Tamro.Madam.Models.Common;

namespace Tamro.Madam.Models.Sales.CanceledOrderLines.ExcludedCustomers;

public class ExcludedCustomerGridModel : BaseDataGridModel<ExcludedCustomerGridModel>
{
    public int Id { get; set; }
    [DisplayName("Customer sold to")]
    public int E1SoldTo { get; set; }
    [DisplayName("Customer name")]
    public string Name { get; set; }
    [DisplayName("Exclusion Level")]
    public ExclusionLevel ExclusionLevel { get; set; }
}
