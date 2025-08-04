using System.ComponentModel;
using Tamro.Madam.Models.Common;

namespace Tamro.Madam.Models.ItemMasterdata.Producers;

public class ProducerModel : BaseDataGridModel<ProducerModel>
{
    public int Id { get; set; }
    [DisplayName("Producer name")]
    public string Name { get; set; }
}
