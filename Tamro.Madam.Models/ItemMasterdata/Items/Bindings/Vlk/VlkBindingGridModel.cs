using System.ComponentModel;
using Tamro.Madam.Models.Common;

namespace Tamro.Madam.Models.ItemMasterdata.Items.Bindings.Vlk;

public class VlkBindingGridModel : BaseDataGridModel<VlkBindingGridModel>
{
    public int Id { get; set; }
    [DisplayName("Item No.")]
    public string ItemNo2 { get; set; }
    [DisplayName("Item name")]
    public string ItemName { get; set; }
    public int ItemBindingId { get; set; }
    public int? NpakId7 { get; set; }
}
