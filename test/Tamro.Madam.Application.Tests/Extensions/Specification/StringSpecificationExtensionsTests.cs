using Ardalis.Specification;
using Moq;
using NUnit.Framework;
using Shouldly;
using Tamro.Madam.Application.Extensions.Specification;
using Tamro.Madam.Common.Constants.SearchExpressionConstants;

namespace Tamro.Madam.Application.Tests.Extensions.Specification;

[TestFixture]
public class StringSpecificationExtensionsTests
{
    [TestCase("Test", SearchStringConstants.Contains, "Test", true)]
    [TestCase("Test", SearchStringConstants.Contains, "Tes", true)]
    [TestCase("Test", SearchStringConstants.Contains, "Nest", false)]
    [TestCase("Test", SearchStringConstants.Contains, "", true)]
    [TestCase("Test", SearchStringConstants.Contains, " ", false)]
    [TestCase("Test", SearchStringConstants.NotContains, "Test", false)]
    [TestCase("Test", SearchStringConstants.NotContains, "Tes", false)]
    [TestCase("Test", SearchStringConstants.NotContains, "Nest", true)]
    [TestCase("Test", SearchStringConstants.NotContains, "", true)]
    [TestCase("Test", SearchStringConstants.NotContains, " ", true)]
    [TestCase("Test", SearchStringConstants.EndsWith, "Test", true)]
    [TestCase("Test", SearchStringConstants.EndsWith, "Tes", false)]
    [TestCase("Test", SearchStringConstants.EndsWith, "est", true)]
    [TestCase("Test", SearchStringConstants.EndsWith, "", true)]
    [TestCase("Test", SearchStringConstants.EndsWith, " ", false)]
    [TestCase("Test", SearchStringConstants.StartsWith, "Test", true)]
    [TestCase("Test", SearchStringConstants.StartsWith, "Tes", true)]
    [TestCase("Test", SearchStringConstants.StartsWith, "est", false)]
    [TestCase("Test", SearchStringConstants.StartsWith, "", true)]
    [TestCase("Test", SearchStringConstants.StartsWith, " ", false)]
    [TestCase("Test", SearchStringConstants.Equals, "Test", true)]
    [TestCase("Test", SearchStringConstants.Equals, "Tes", false)]
    [TestCase("Test", SearchStringConstants.Equals, "est", false)]
    [TestCase("Test", SearchStringConstants.Equals, "", true)]
    [TestCase("Test", SearchStringConstants.Equals, " ", false)]
    [TestCase("Test", SearchStringConstants.NotEquals, "Test", false)]
    [TestCase("Test", SearchStringConstants.NotEquals, "Tes", true)]
    [TestCase("Test", SearchStringConstants.NotEquals, "est", true)]
    [TestCase("Test", SearchStringConstants.NotEquals, "", true)]
    [TestCase("Test", SearchStringConstants.NotEquals, " ", true)]
    [TestCase("", SearchStringConstants.IsEmpty, "", true)]
    [TestCase("Test", SearchStringConstants.IsEmpty, "", false)]
    [TestCase("", SearchStringConstants.IsNotEmpty, "", false)]
    [TestCase("Test", SearchStringConstants.IsNotEmpty, "", true)]
    public void ApplyStringFilter_AppliesFilterCorrectly(string name, string op, string value, bool expectedReturns)
    {
        // Arrange
        var dummyModel = new DummyModel()
        {
            Name = name,
        };
        var dummies = new List<DummyModel>() { dummyModel, };

        var evaluator = new InMemorySpecificationEvaluator();
        var specificationBuilder = new SpecificationBuilder<DummyModel>(new Mock<Specification<DummyModel>>().Object);
        specificationBuilder.ApplyStringFilter(op, value, x => x.Name);

        // Act
        var result = evaluator.Evaluate(dummies, specificationBuilder.Specification);

        // Assert
        result.Any().ShouldBe(expectedReturns);
    }

    [TestCase(SearchStringConstants.Contains, "Test", SearchStringConstants.StartsWith, "ing", true)]
    [TestCase(SearchStringConstants.Contains, "Test", SearchStringConstants.StartsWith, "ng", false)]
    [TestCase(SearchStringConstants.Contains, "Nest", SearchStringConstants.StartsWith, "in", false)]
    [TestCase(SearchStringConstants.Contains, "T", SearchStringConstants.StartsWith, "i", true)]
    public void ApplyStringFilter_AppliesMultipleFiltersAs_And(string filterOperation1, string filterValue1, string filterOperation2, string filterValue2, bool expectedReturns)
    {
        // Arrange
        var dummyModel = new DummyModel()
        {
            Name = "Test",
            Value = "ing",
        };
        var dummies = new List<DummyModel>() { dummyModel, };

        var evaluator = new InMemorySpecificationEvaluator();
        var specificationBuilder = new SpecificationBuilder<DummyModel>(new Mock<Specification<DummyModel>>().Object);
        specificationBuilder.ApplyStringFilter(filterOperation1, filterValue1, x => x.Name);
        specificationBuilder.ApplyStringFilter(filterOperation2, filterValue2, x => x.Value);

        // Act
        var result = evaluator.Evaluate(dummies, specificationBuilder.Specification);

        // Assert
        result.Any().ShouldBe(expectedReturns);
    }
}

public class DummyModel
{
    public string Name { get; set; }
    public string Value { get; set; }
}