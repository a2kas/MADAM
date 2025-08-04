using System.ComponentModel;
using Tamro.Madam.Models.ItemMasterdata.Atcs;
using Tamro.Madam.Models.ItemMasterdata.Barcodes;
using Tamro.Madam.Models.ItemMasterdata.Brands;
using Tamro.Madam.Models.ItemMasterdata.Forms;
using Tamro.Madam.Models.ItemMasterdata.Items.Bindings;
using Tamro.Madam.Models.ItemMasterdata.MeasurementUnits;
using Tamro.Madam.Models.ItemMasterdata.Nicks;
using Tamro.Madam.Models.ItemMasterdata.Requestors;
using Tamro.Madam.Models.ItemMasterdata.Producers;
using System.Globalization;
using System.Text;

namespace Tamro.Madam.Models.ItemMasterdata.Items;

public class ItemModel
{
    [DisplayName("Item id")]
    public int Id { get; set; }
    [DisplayName("Baltic item name")]
    public string ItemName => GenerateItemName();
    [DisplayName("Active")]
    public bool Active { get; set; }
    [DisplayName("Baltic item description")]
    public string? Description { get; set; }
    [DisplayName("Brand sub-brand")]
    public BrandClsfModel Brand { get; set; }
    public string? Strength { get; set; }
    public FormClsfModel Form { get; set; }
    public decimal? Measure { get; set; }
    [DisplayName("Measurement unit")]
    public MeasurementUnitClsfModel? MeasurementUnit { get; set; }
    public int? Numero { get; set; }
    [DisplayName("Producer name")]
    public ProducerClsfModel Producer { get; set; }
    [DisplayName("Baltic nick")]
    public NickClsfModel? SupplierNick { get; set; }
    [DisplayName("ATC")]
    public AtcClsfModel Atc { get; set; }
    [DisplayName("Active substance")]
    public string? ActiveSubstance { get; set; }
    [DisplayName("Requestor")]
    public RequestorClsfModel? Requestor { get; set; }
    public DateTime? EditedAt { get; set; }
    public string EditedBy { get; set; }
    public int ParallelParentItemId { get; set; }
    [DisplayName("Item type")]
    public ItemType ItemType { get; set; }

    [DisplayName("Parallel")]
    public bool IsParallel
    {
        get
        {
            return ParallelParentItemId != default;
        }
    }

    [DisplayName("Barcodes")]
    public IEnumerable<BarcodeModel> Barcodes { get; set; }
    [DisplayName("Bindings")]
    public IEnumerable<ItemBindingModel> Bindings { get; set; }

    public byte[] RowVer { get; set; }

    private string GenerateItemName()
    {
        var constructedItemName = new StringBuilder();
        string separator = " ";

        if (!string.IsNullOrEmpty(Brand?.Name))
        {
            constructedItemName.Append(Brand.Name + separator);
        }

        if (!string.IsNullOrEmpty(Description))
        {
            constructedItemName.Append(Description + separator);
        }

        if (!string.IsNullOrEmpty(Strength))
        {
            constructedItemName.Append(Strength + separator);
        }

        if (!string.IsNullOrEmpty(Form?.Name))
        {
            constructedItemName.Append(Form.Name + separator);
        }

        if (Measure.HasValue && MeasurementUnit != null)
        {
            constructedItemName.Append(Measure.Value.ToString("0.##", CultureInfo.InvariantCulture) + separator + MeasurementUnit.Name + separator);
        }

        if (Numero.HasValue && Numero.Value != 0)
        {
            constructedItemName.Append($"N{Numero.Value}{separator}");
        }

        if (!string.IsNullOrEmpty(Producer?.Name) && ParallelParentItemId == 0)
        {
            constructedItemName.Append($"({Producer.Name}){separator}");
        }

        if (ParallelParentItemId != 0)
        {
            constructedItemName.Append($"({SupplierNick?.Name ?? string.Empty}) P");
        }

        return constructedItemName.ToString();
    }
}
