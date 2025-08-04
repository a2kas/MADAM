using System.ComponentModel;
using Tamro.Madam.Models.Common;

namespace Tamro.Madam.Models.Administration.Configuration.Ubl;
public class UblApiKeyModel : BaseDataGridModel<UblApiKeyModel>
{
    [DataGridIdentifier]
    [DisplayName("E1 sold to")]
    public int E1SoldTo { get; set; }
    [DisplayName("Customer name")]
    public string CustomerName { get; set; }
    [DisplayName("API key")]
    public string ApiKey { get; set; }
}
