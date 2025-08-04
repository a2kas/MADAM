using System.ComponentModel.DataAnnotations.Schema;
using Tamro.Madam.Repository.Context.Madam;

namespace Tamro.Madam.Repository.Entities.ItemMasterdata.Items;

[Table("ItemLog")]
public class ItemLog : IMadamEntity<int>
{
    public int Id { get; set; }
    public string? Description { get; set; }
    public int ProducerId { get; set; }
    public int BrandId { get; set; }
    public string? Strength { get; set; }
    public int? FormId { get; set; }
    public decimal? Measure { get; set; }
    [Column("UOMId")]
    public int? UomId { get; set; }
    [Column("AtcId")]
    public int? AtcId { get; set; }
    public int? SupplierNickId { get; set; }
    public int Numero { get; set; }
    public int? Dose { get; set; }
    public string? ActiveSubstance { get; set; }
    public int Active { get; set; }
    public int? ParallelParentItemId { get; set; }
    public int? RequestorId { get; set; }
    public string? ItemName { get; set; }
    public DateTime? EditedAt { get; set; }
    public string EditedBy { get; set; }
    public int? Deleted { get; set; }
}
