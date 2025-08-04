using System.ComponentModel;

namespace Tamro.Madam.Models.Audit;

public class AuditBaseModel
{
    [DisplayName("Audit Id")]
    public int Id { get; set; }
    [DisplayName("Resource Id")]
    public string EntityId { get; set; }
    [DisplayName("Resource")]
    public string EntityTypeName { get; set; }
    [DisplayName("Action")]
    public string StateName { get; set; }
    [DisplayName("Author")]
    public string CreatedBy { get; set; }
    [DisplayName("Date")]
    public DateTime CreatedDate { get; set; }
}
