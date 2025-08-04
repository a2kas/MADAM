using Tamro.Madam.Models.General;
using Tamro.Madam.Models.ItemMasterdata.Items.QualityCheck;
using Tamro.Madam.Repository.Context.Madam;
using Tamro.Madam.Repository.Entities.ItemMasterdata.Items.Bindings;
using TamroUtilities.EFCore.Models;

namespace Tamro.Madam.Repository.Entities.ItemMasterdata.Items.QualityCheck;
public class ItemQualityCheckIssue : IMadamEntity<int>, IBaseEntity
{
    public int Id { get; set; }
    public int ItemQualityCheckId { get; set; }
    public int ItemId { get; set; }
    public int? ItemBindingId { get; set; }
    public string IssueEntity { get; set; }
    public string IssueField { get; set; }
    public string? Description { get; set; }
    public string? ActualValue { get; set; }
    public string? ExpectedValue { get; set; }
    public BalticCountry? Country { get; set; }
    public ItemQualityIssueSeverity IssueSeverity { get; set; }
    public ItemQualityIssueStatus IssueStatus { get; set; }
    public DateTime CreatedDate { get; set; }
    public DateTime RowVer { get; set; }

    public ItemQualityCheck ItemQualityCheck { get; set; }
    public Item Item { get; set; }
    public ItemBinding? ItemBinding { get; set; }
}
