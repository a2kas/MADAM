using System.ComponentModel.DataAnnotations;

namespace Tamro.Madam.Repository.Entities.Customers;
public class CustomerNotification
{
    [Key]
    public int Id { get; set; }

    [Required]
    public bool SendCanceledOrderNotification { get; set; }

    [Required]
    public int CustomerId { get; set; }

    [Required]
    public DateTime CreatedDate { get; set; } = DateTime.UtcNow;

    [Required]
    public DateTime RowVer { get; set; } = DateTime.UtcNow;
}
