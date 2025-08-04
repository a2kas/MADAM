namespace Tamro.Madam.Models.ItemMasterdata.Items.QualityCheck;

public class ItemQualityCheckExtractedFieldModel
{
    public string Field { get; set; }
    public string? Value { get; set; }
    public string? IssuesFlagged { get; set; }
    public ItemQualityCheckIssueType? IssuesType { get; set; }
}
