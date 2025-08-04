using Tamro.Madam.Models.ItemMasterdata.Items.QualityCheck;

namespace Tamro.Madam.Application.Services.Items.QualityCheck;

public interface IIssueSeverityResolver
{
    ItemQualityIssueSeverity ResolveIssueSeverity(ItemQualityCheckIssueType? type);
}
