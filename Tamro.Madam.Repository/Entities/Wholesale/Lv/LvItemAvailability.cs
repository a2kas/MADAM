using System.ComponentModel.DataAnnotations.Schema;

namespace Tamro.Madam.Repository.Entities.Wholesale.Lv;

[Table("ItemAvailability")]
public class LvItemAvailability
{
    [Column("ItemNo3")]
    public string ItemNo { get; set; }
    public int? AvailableQuantity { get; set; }
}
