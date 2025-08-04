using System.ComponentModel;
using Tamro.Madam.Models.Common;

namespace Tamro.Madam.Models.Hr.Jira.Administration;
public class JiraAccountModel : BaseDataGridModel<JiraAccountModel>
{
    public string Id { get; set; }
    [DisplayName("Display name")]
    public string DisplayName { get; set; }
    [DisplayName("Team")]
    public JiraAccountTeam? Team { get; set; }
    [DisplayName("Active")]
    public bool IsActive { get; set; }
}
