using System.ComponentModel;
using Tamro.Madam.Models.ItemMasterdata.Items;

namespace Tamro.Madam.Models.ItemMasterdata.Barcodes;

public class BarcodeModel : BarcodeBaseModel
{
    [DisplayName("Item")]
    public ItemClsfModel Item { get; set; }
}
