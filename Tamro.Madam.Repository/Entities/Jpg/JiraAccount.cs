using Tamro.Madam.Models.Hr.Jira.Administration;

namespace Tamro.Madam.Repository.Entities.Jpg;
public class JiraAccount : IJpgEntity
{
    public string Id { get; set; }
    public string DisplayName { get; set; }
    public JiraAccountTeam? Team { get; set; }
    public bool IsActive { get; set; }
}