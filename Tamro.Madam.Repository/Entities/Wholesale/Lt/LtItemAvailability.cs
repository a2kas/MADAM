using System.ComponentModel.DataAnnotations.Schema;

namespace Tamro.Madam.Repository.Entities.Wholesale.Lt;

[Table("ItemAvailability")]
public class LtItemAvailability
{
    [Column("ItemNo2")]
    public string ItemNo { get; set; }
    public int? AvailableQuantity { get; set; }
}
