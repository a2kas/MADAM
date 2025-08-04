using System.ComponentModel;
using Tamro.Madam.Models.Common;

namespace Tamro.Madam.Models.ItemMasterdata.Atcs;

public class AtcModel : BaseDataGridModel<AtcModel>
{
    public int Id { get; set; }
    [DisplayName("ATC value")]
    public string Value { get; set; }
    [DisplayName("ATC name")]
    public string Name { get; set; }
}
