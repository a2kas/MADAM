using NUnit.Framework;
using Shouldly;
using Tamro.Madam.Application.Services.Items.QualityCheck;

namespace Tamro.Madam.Application.Tests.Services.Items.QualityCheck;

[TestFixture]
public class IssueEntityResolverTests
{
    private IssueEntityResolver _resolver;

    [SetUp]
    public void SetUp()
    {
        _resolver = new IssueEntityResolver();
    }

    [Test]
    [TestCase("strength", "Item")]
    [TestCase("activeIngredient", "Item")]
    [TestCase("name", "Item")]
    [TestCase("unknownField", "ItemBinding")]
    public void ResolveIssueEntity_ShouldReturnExpectedEntityName(string field, string expectedEntityName)
    {
        // Act
        var actualEntityName = _resolver.ResolveIssueEntity(field);

        // Assert
        actualEntityName.ShouldBe(expectedEntityName);
    }
}