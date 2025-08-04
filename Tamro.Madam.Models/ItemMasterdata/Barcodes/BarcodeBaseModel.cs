using System.ComponentModel;

namespace Tamro.Madam.Models.ItemMasterdata.Barcodes;

public class BarcodeBaseModel
{
    public int Id { get; set; }
    [DisplayName("EAN code")]
    public string Ean { get; set; }
    public bool Measure { get; set; }
    [DisplayName("Last edited")]
    public DateTime? RowVer { get; set; }
}
