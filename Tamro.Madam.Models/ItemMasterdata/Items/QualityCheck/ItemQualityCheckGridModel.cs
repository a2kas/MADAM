using System.ComponentModel;
using Tamro.Madam.Models.Common;

namespace Tamro.Madam.Models.ItemMasterdata.Items.QualityCheck;
public class ItemQualityCheckGridModel : BaseDataGridModel<ItemQualityCheckGridModel>
{
    public int Id { get; set; }
    [DisplayName("Item Id")]
    public int ItemId { get; set; }
    [DisplayName("Item name")]
    public string ItemName { get; set; }
    [DisplayName("Last scan date")]
    public DateTime ScanDate { get; set; }
    [DisplayName("Amount of issues")]
    public int IssueCount { get; set; }
    [DisplayName("Amount of unresolved issues")]
    public int UnresolvedIssuesCount { get; set; }
    [DisplayName("Severity of unresolved issues")]
    public List<ItemQualityIssueSeverity> UnresolvedSeverities { get; set; }
    [DisplayName("Status")]
    public ItemQualityIssueStatus Status { get; set; }
    public List<ItemQualityCheckIssueGridModel> Issues { get; set; }
}
