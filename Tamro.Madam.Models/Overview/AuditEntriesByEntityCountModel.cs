using Tamro.Madam.Models.Data;

namespace Tamro.Madam.Models.Overview;

public class AuditEntriesByEntityCountModel
{
    public string EntityName { get; set; }
    public int Count { get; set; }
    public string DisplayName 
    {
        get
        {
            if (AuditData.AuditEntryDisplayNames.TryGetValue(EntityName, out var displayName))
            {
                return displayName;
            }
            else
            {
                return EntityName;
            }
        }
    }
}
