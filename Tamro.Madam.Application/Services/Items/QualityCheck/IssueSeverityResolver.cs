using Tamro.Madam.Models.ItemMasterdata.Items.QualityCheck;

namespace Tamro.Madam.Application.Services.Items.QualityCheck;

public class IssueSeverityResolver : IIssueSeverityResolver
{
    public ItemQualityIssueSeverity ResolveIssueSeverity(ItemQualityCheckIssueType? type)
    {
        return type switch
        {
            ItemQualityCheckIssueType.MissingData or ItemQualityCheckIssueType.GrammarMistake or ItemQualityCheckIssueType.Uncategorized => ItemQualityIssueSeverity.Low,
            ItemQualityCheckIssueType.ErroneousHtmlTagging or ItemQualityCheckIssueType.TextStyle or ItemQualityCheckIssueType.ConfusingText or ItemQualityCheckIssueType.InconsistentText => ItemQualityIssueSeverity.Medium,
            ItemQualityCheckIssueType.IncorrectData => ItemQualityIssueSeverity.High,
            _ => ItemQualityIssueSeverity.Low,
        };
    }
}
