using NUnit.Framework;
using Shouldly;
using Tamro.Madam.Application.Services.Items.QualityCheck;
using Tamro.Madam.Models.ItemMasterdata.Items.QualityCheck;

namespace Tamro.Madam.Application.Tests.Services.Items.QualityCheck;

[TestFixture]
public class IssueSeverityResolverTests
{
    private IssueSeverityResolver _resolver;

    [SetUp]
    public void SetUp()
    {
        _resolver = new IssueSeverityResolver();
    }

    [Test]
    [TestCase(ItemQualityCheckIssueType.MissingData, ItemQualityIssueSeverity.Low)]
    [TestCase(ItemQualityCheckIssueType.GrammarMistake, ItemQualityIssueSeverity.Low)]
    [TestCase(ItemQualityCheckIssueType.Uncategorized, ItemQualityIssueSeverity.Low)]
    [TestCase(ItemQualityCheckIssueType.ErroneousHtmlTagging, ItemQualityIssueSeverity.Medium)]
    [TestCase(ItemQualityCheckIssueType.TextStyle, ItemQualityIssueSeverity.Medium)]
    [TestCase(ItemQualityCheckIssueType.ConfusingText, ItemQualityIssueSeverity.Medium)]
    [TestCase(ItemQualityCheckIssueType.InconsistentText, ItemQualityIssueSeverity.Medium)]
    [TestCase(ItemQualityCheckIssueType.IncorrectData, ItemQualityIssueSeverity.High)]
    [TestCase(null, ItemQualityIssueSeverity.Low)]
    public void ResolveIssueSeverity_ShouldReturnExpectedSeverity(ItemQualityCheckIssueType? type, ItemQualityIssueSeverity expectedSeverity)
    {
        // Act
        var actualSeverity = _resolver.ResolveIssueSeverity(type);

        // Assert
        actualSeverity.ShouldBe(expectedSeverity);
    }
}