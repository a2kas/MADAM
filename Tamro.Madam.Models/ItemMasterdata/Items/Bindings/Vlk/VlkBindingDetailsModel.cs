using System.ComponentModel;
using Tamro.Madam.Models.ItemMasterdata.Items.Bindings;

namespace Tamro.Madam.Models.ItemMasterdata.Items.Bindings.Vlk;

public class VlkBindingDetailsModel
{
    public int Id { get; set; }
    [DisplayName("Item")]
    public ItemBindingClsfModel? ItemBinding { get; set; }
    public int? NpakId7 { get; set; }
}
