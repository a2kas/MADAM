using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Tamro.Madam.Repository.Entities.Wholesale.Lt;

[Table("Items")]
public class LtWholesaleItem
{
    [Key]
    [Column("ItemNo2")]
    public string ItemNo { get; set; }
    [Column("ItemDescription1")]
    public string ItemDescription { get; set; }
}