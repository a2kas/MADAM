using System.ComponentModel;
using Tamro.Madam.Models.General;

namespace Tamro.Madam.Models.ItemMasterdata.Items.QualityCheck;

public class ItemQualityCheckIssueGridModel
{
    private string _issueField;

    public int Id { get; set; }
    [DisplayName("Scope")]
    public string IssueEntity { get; set; }
    [DisplayName("Field")]
    public string IssueField
    {
        get => CapitalizeAndFormat(_issueField);
        set => _issueField = value;
    }
    [DisplayName("Description")]
    public string IssueDescription { get; set; }
    [DisplayName("Actual value")]
    public string ActualValue { get; set; }
    public int ItemId { get; set; }
    public string? ItemBindingId { get; set; }
    [DisplayName("Identifier")]
    public string Identifier
    {
        get
        {
            return !string.IsNullOrEmpty(ItemBindingId) ? ItemBindingId : ItemId.ToString();
        }
    }
    [DisplayName("Expected value")]
    public string ExpectedValue { get; set; }
    [DisplayName("Country")]
    public BalticCountry? Country { get; set; }
    [DisplayName("Severity")]
    public ItemQualityIssueSeverity Severity { get; set; }
    [DisplayName("Status")]
    public ItemQualityIssueStatus Status { get; set; }
    [DisplayName("Found date")]
    public DateTime RowVer { get; set; }

    private static string CapitalizeAndFormat(string input)
    {
        if (string.IsNullOrEmpty(input))
        {
            return input;
        }

        var words = System.Text.RegularExpressions.Regex.Split(input, @"(?<!^)(?=[A-Z])");

        return string.Join(" ", words.Select(word => char.ToUpper(word[0]) + word.Substring(1)));
    }
}
