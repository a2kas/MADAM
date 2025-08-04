using System.ComponentModel.DataAnnotations.Schema;

namespace Tamro.Madam.Repository.Entities.Wholesale.Ee;

[Table("ItemAvailability")]
public class EeItemAvailability
{
    [Column("ItemNo3")]
    public string ItemNo { get; set; }
    public int? AvailableQuantity { get; set; }
}
