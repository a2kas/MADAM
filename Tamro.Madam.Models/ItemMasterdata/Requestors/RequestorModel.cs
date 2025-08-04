using System.ComponentModel;
using Tamro.Madam.Models.Common;

namespace Tamro.Madam.Models.ItemMasterdata.Requestors;

public class RequestorModel : BaseDataGridModel<RequestorModel>
{
    public int Id { get; set; }
    [DisplayName("Requestor name")]
    public string Name { get; set; }
}
