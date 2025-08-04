using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Tamro.Madam.Repository.Context.Madam;
using Tamro.Madam.Repository.Entities.ItemMasterdata.Items;
using TamroUtilities.EFCore.Models;

namespace Tamro.Madam.Repository.Entities.ItemMasterdata.Barcodes;

[Table("Barcode")]
public class Barcode : IMadamEntity<int>, IAuditable
{
    [Key]
    public int Id { get; set; }
    [Column("EAN")]
    public string Ean { get; set; }
    [Column("Measure", TypeName = "int")]
    public bool Measure { get; set; }
    [Column("EditedAt", TypeName = "datetime")]
    public DateTime? RowVer { get; set; }

    public int ItemId { get; set; }
    public Item? Item { get; set; }
}
