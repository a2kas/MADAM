using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Tamro.Madam.Models.ItemMasterdata.Items;
using Tamro.Madam.Repository.Context.Madam;
using Tamro.Madam.Repository.Entities.Atcs;
using Tamro.Madam.Repository.Entities.Brands;
using Tamro.Madam.Repository.Entities.ItemMasterdata.Barcodes;
using Tamro.Madam.Repository.Entities.ItemMasterdata.Forms;
using Tamro.Madam.Repository.Entities.ItemMasterdata.Items.Bindings;
using Tamro.Madam.Repository.Entities.ItemMasterdata.MeasurementUnits;
using Tamro.Madam.Repository.Entities.ItemMasterdata.Nicks;
using Tamro.Madam.Repository.Entities.ItemMasterdata.Producers;
using Tamro.Madam.Repository.Entities.ItemMasterdata.Requestors;
using TamroUtilities.EFCore.Models;

namespace Tamro.Madam.Repository.Entities.ItemMasterdata.Items;

[Table("Item")]
public class Item : IMadamEntity<int>, IAuditable
{
    [Key]
    public int Id { get; set; }
    [Column("ItemName")]
    public string? ItemName { get; set; }
    public string? Description { get; set; }
    public string? Strength { get; set; }
    public int Numero { get; set; }
    public decimal? Measure { get; set; }
    [Column("Active", TypeName = "int")]
    public bool Active { get; set; }
    [Timestamp]
    public byte[] RowVer { get; set; }
    public string? ActiveSubstance { get; set; }
    public DateTime? EditedAt { get; set; }
    public string EditedBy { get; set; }
    public int? ParallelParentItemId { get; set; }
    public int ProducerId { get; set; }
    public int BrandId { get; set; }
    public int? FormId { get; set; }
    public int? AtcId { get; set; }
    public int? SupplierNickId { get; set; }
    [Column("UOMId")]
    public int? MeasurementUnitId { get; set; }
    public int? RequestorId { get; set; }
    public ItemType ItemType { get; set; }

    public Producer Producer { get; set; }
    public Brand Brand { get; set; }
    public Form? Form { get; set; }
    public Atc? Atc { get; set; }
    public Nick? SupplierNick { get; set; }
    public MeasurementUnit? MeasurementUnit { get; set; }
    public Requestor? Requestor { get; set; }
    public ICollection<Barcode> Barcodes { get; set; }
    public ICollection<ItemBinding> Bindings { get; set; }
}
