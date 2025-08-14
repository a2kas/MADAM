using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Tamro.Madam.Repository.Context.Madam;
using TamroUtilities.EFCore.Models;

namespace Tamro.Madam.Repository.Entities.Customers;

public class Customer : IMadamEntity<int>, IBaseEntity
{
    [Key]
    public int Id { get; set; }
    [Required]
    public int CustomerLegalEntityId { get; set; }
    [Required]
    public int E1ShipTo { get; set; }
    [Required]
    public bool IsActive { get; set; } = true;
    [Required]
    public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
    [Required]
    public DateTime RowVer { get; set; } = DateTime.UtcNow;

    [ForeignKey("CustomerLegalEntityId")]
    public virtual CustomerLegalEntity CustomerLegalEntity { get; set; }
    public virtual CustomerNotification CustomerNotification { get; set; }
}
