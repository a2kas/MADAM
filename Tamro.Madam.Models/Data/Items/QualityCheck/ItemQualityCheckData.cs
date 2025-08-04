using System.Collections.Immutable;
using Tamro.Madam.Models.ItemMasterdata.Items.QualityCheck;

namespace Tamro.Madam.Models.Data.Items.QualityCheck;

public static class ItemQualityCheckData
{
    public static ImmutableDictionary<string, string> Severity { get; } = ImmutableDictionary<string, string>.Empty
       .Add(ItemQualityIssueSeverity.Low.ToString(), "Low")
       .Add(ItemQualityIssueSeverity.Medium.ToString(), "Medium")
       .Add(ItemQualityIssueSeverity.High.ToString(), "High");

    public static ImmutableDictionary<string, string> Status { get; } = ImmutableDictionary<string, string>.Empty
        .Add(ItemQualityIssueStatus.New.ToString(), "New")
        .Add(ItemQualityIssueStatus.Resolved.ToString(), "Resolved")
        .Add(ItemQualityIssueStatus.FalsePositive.ToString(), "False positive");
}
