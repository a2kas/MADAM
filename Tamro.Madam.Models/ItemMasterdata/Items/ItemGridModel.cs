using System.ComponentModel;
using Tamro.Madam.Models.Common;

namespace Tamro.Madam.Models.ItemMasterdata.Items;

public class ItemGridModel : BaseDataGridModel<ItemGridModel>
{
    [DisplayName("Id")]
    public int Id { get; set; }
    [DisplayName("Baltic name")]
    public string ItemName { get; set; }
    [DisplayName("Description")]
    public string? Description { get; set; }
    [DisplayName("Producer")]
    public string Producer { get; set; }
    [DisplayName("Brand")]
    public string Brand { get; set; }
    [DisplayName("Strength")]
    public string? Strength { get; set; }
    [DisplayName("Form")]
    public string? Form { get; set; }
    [DisplayName("Measure")]
    public decimal? Measure { get; set; }
    [DisplayName("MU")]
    public string? MeasurementUnit { get; set; }
    [DisplayName("ATC code")]
    public string? AtcCode { get; set; }
    [DisplayName("ATC name")]
    public string? AtcName { get; set; }
    [DisplayName("Baltic nick")]
    public string? SupplierNick { get; set; }
    [DisplayName("Numero")]
    public int? Numero { get; set; }
    [DisplayName("Active substance")]
    public string? ActiveSubstance { get; set; }
    [DisplayName("Active")]
    public bool Active { get; set; }
}
