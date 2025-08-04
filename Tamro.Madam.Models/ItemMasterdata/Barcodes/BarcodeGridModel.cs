using System.ComponentModel;
using Tamro.Madam.Models.Common;

namespace Tamro.Madam.Models.ItemMasterdata.Barcodes;

public class BarcodeGridModel : BaseDataGridModel<BarcodeGridModel>
{
    [DisplayName("Item name")]
    public string ItemName { get; set; }
    [DisplayName("Item id")]
    public int ItemId { get; set; }
    public int Id { get; set; }
    [DisplayName("EAN code")]
    public string Ean { get; set; }
    public bool Measure { get; set; }
    [DisplayName("Last edited")]
    public DateTime? RowVer { get; set; }
}
