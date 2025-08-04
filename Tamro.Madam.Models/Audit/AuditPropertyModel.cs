namespace Tamro.Madam.Models.Audit;

public class AuditPropertyModel
{
    public string RelationName { get; set; }
    public string PropertyName { get; set; }
    public string OldValue { get; set; }
    public string NewValue { get; set; }
}
