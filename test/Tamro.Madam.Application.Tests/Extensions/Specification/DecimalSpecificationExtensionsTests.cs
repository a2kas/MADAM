using Ardalis.Specification;
using Moq;
using NUnit.Framework;
using Shouldly;
using Tamro.Madam.Application.Extensions.Specification;
using Tamro.Madam.Common.Constants.SearchExpressionConstants;

namespace Tamro.Madam.Application.Tests.Extensions.Specification;

[TestFixture]
public class DecimalSpecificationExtensionsTests
{
    [TestCase(25, SearchNumberConstants.Equals, 25, true)]
    [TestCase(25, SearchNumberConstants.Equals, 26, false)]
    [TestCase(25, SearchNumberConstants.NotEquals, 25, false)]
    [TestCase(25, SearchNumberConstants.NotEquals, 26, true)]
    [TestCase(25, SearchNumberConstants.GreaterThan, 26, false)]
    [TestCase(25, SearchNumberConstants.GreaterThan, 24, true)]
    [TestCase(25, SearchNumberConstants.LessThan, 26, true)]
    [TestCase(25, SearchNumberConstants.LessThan, 24, false)]
    [TestCase(25, SearchNumberConstants.GreaterThanOrEqual, 25, true)]
    [TestCase(25, SearchNumberConstants.GreaterThanOrEqual, 26, false)]
    [TestCase(25, SearchNumberConstants.LessThanOrEqual, 25, true)]
    [TestCase(25, SearchNumberConstants.LessThanOrEqual, 26, true)]
    [TestCase(null, SearchNumberConstants.IsEmpty, null, true)]
    [TestCase(25, SearchNumberConstants.IsEmpty, null, false)]
    [TestCase(null, SearchNumberConstants.IsNotEmpty, null, false)]
    [TestCase(25, SearchNumberConstants.IsNotEmpty, null, true)]
    public void ApplyDecimalFilter_AppliesFilterCorrectly(decimal? weight, string op, decimal? filterValue, bool expectedReturns)
    {
        // Arrange
        var dummyModel = new DummyDecimalModel()
        {
            Weight = weight,
        };
        var dummies = new List<DummyDecimalModel>() { dummyModel, };

        var evaluator = new InMemorySpecificationEvaluator();
        var specificationBuilder = new SpecificationBuilder<DummyDecimalModel>(new Mock<Specification<DummyDecimalModel>>().Object);
        specificationBuilder.ApplyDecimalFilter(op, filterValue, x => x.Weight);

        // Act
        var result = evaluator.Evaluate(dummies, specificationBuilder.Specification);

        // Assert
        result.Any().ShouldBe(expectedReturns);
    }
}

public class DummyDecimalModel
{
    public decimal? Weight { get; set; }
}