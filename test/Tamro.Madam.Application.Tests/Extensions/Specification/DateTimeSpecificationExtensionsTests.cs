using Ardalis.Specification;
using Moq;
using NUnit.Framework;
using Shouldly;
using Tamro.Madam.Application.Extensions.Specification;
using Tamro.Madam.Common.Constants.SearchExpressionConstants;

namespace Tamro.Madam.Application.Tests.Extensions.Specification;

[TestFixture]
public class DateTimeSpecificationExtensionsTests
{
    [TestCaseSource(nameof(TestCases))]
    public void ApplyDateTimeFilter_AppliesFilterCorrectly(DateTimeSpecificationExtensionTestCase testCase)
    {
        // Arrange
        var dummyModel = new DummyDateTimeModel()
        {
            DateCreated = testCase.DateCreated,
        };
        var dummies = new List<DummyDateTimeModel>() { dummyModel, };

        var evaluator = new InMemorySpecificationEvaluator();
        var specificationBuilder = new SpecificationBuilder<DummyDateTimeModel>(new Mock<Specification<DummyDateTimeModel>>().Object);
        specificationBuilder.ApplyDateTimeFilter(testCase.FilterOperation, testCase.FilterValue, x => x.DateCreated);

        // Act
        var result = evaluator.Evaluate(dummies, specificationBuilder.Specification);

        // Assert
        result.Any().ShouldBe(testCase.ExpectedReturns);

    }

    private static IEnumerable<DateTimeSpecificationExtensionTestCase> TestCases()
    {
        yield return new DateTimeSpecificationExtensionTestCase(new DateTime(2024, 1, 1), SearchDateTimeConstants.Is, new DateTime(2024, 1, 1), true);
        yield return new DateTimeSpecificationExtensionTestCase(new DateTime(2024, 1, 1, 1, 2, 42), SearchDateTimeConstants.Is, new DateTime(2024, 1, 1, 1, 2, 0), true); // Filtering of date-times is with minute precision, thus we should ignore seconds
        yield return new DateTimeSpecificationExtensionTestCase(new DateTime(2024, 1, 1, 1, 2, 42), SearchDateTimeConstants.Is, new DateTime(2024, 1, 1), true); // If filter is provided with time being 00:00, we should filter only by date, ignoring the time 
        yield return new DateTimeSpecificationExtensionTestCase(new DateTime(2024, 1, 2), SearchDateTimeConstants.Is, new DateTime(2024, 1, 3), false);
        yield return new DateTimeSpecificationExtensionTestCase(new DateTime(2024, 1, 1, 1, 2, 42), SearchDateTimeConstants.Is, new DateTime(2024, 1, 1, 1, 3, 0), false);
        yield return new DateTimeSpecificationExtensionTestCase(new DateTime(2024, 1, 1), SearchDateTimeConstants.IsNot, new DateTime(2024, 1, 1), false);
        yield return new DateTimeSpecificationExtensionTestCase(new DateTime(2024, 1, 1, 1, 2, 42), SearchDateTimeConstants.IsNot, new DateTime(2024, 1, 1, 1, 2, 0), false); // Filtering of date-times is with minute precision, thus we should ignore seconds
        yield return new DateTimeSpecificationExtensionTestCase(new DateTime(2024, 1, 1, 1, 2, 42), SearchDateTimeConstants.IsNot, new DateTime(2024, 1, 1), false); // If filter is provided with time being 00:00, we should filter only by date, ignoring the time 
        yield return new DateTimeSpecificationExtensionTestCase(new DateTime(2024, 1, 2), SearchDateTimeConstants.IsNot, new DateTime(2024, 1, 3), true);
        yield return new DateTimeSpecificationExtensionTestCase(new DateTime(2024, 1, 1, 1, 2, 42), SearchDateTimeConstants.IsNot, new DateTime(2024, 1, 1, 1, 3, 0), true);
        yield return new DateTimeSpecificationExtensionTestCase(new DateTime(2024, 1, 1, 1, 2, 20), SearchDateTimeConstants.IsAfter, new DateTime(2024, 1, 1, 1, 2, 0), true);
        yield return new DateTimeSpecificationExtensionTestCase(new DateTime(2024, 1, 1, 1, 2, 20), SearchDateTimeConstants.IsAfter, new DateTime(2024, 1, 1, 1, 3, 0), false);
        yield return new DateTimeSpecificationExtensionTestCase(new DateTime(2024, 1, 2), SearchDateTimeConstants.IsAfter, new DateTime(2024, 1, 2), false);
        yield return new DateTimeSpecificationExtensionTestCase(new DateTime(2024, 1, 1, 1, 2, 20), SearchDateTimeConstants.IsOnOrAfter, new DateTime(2024, 1, 1, 1, 2, 0), true);
        yield return new DateTimeSpecificationExtensionTestCase(new DateTime(2024, 1, 1, 1, 2, 20), SearchDateTimeConstants.IsOnOrAfter, new DateTime(2024, 1, 1, 1, 3, 0), false);
        yield return new DateTimeSpecificationExtensionTestCase(new DateTime(2024, 1, 2), SearchDateTimeConstants.IsOnOrAfter, new DateTime(2024, 1, 2), true);
        yield return new DateTimeSpecificationExtensionTestCase(new DateTime(2024, 1, 1, 1, 1, 20), SearchDateTimeConstants.IsBefore, new DateTime(2024, 1, 1, 1, 2, 0), true);
        yield return new DateTimeSpecificationExtensionTestCase(new DateTime(2024, 1, 1, 1, 3, 20), SearchDateTimeConstants.IsBefore, new DateTime(2024, 1, 1, 1, 3, 0), false);
        yield return new DateTimeSpecificationExtensionTestCase(new DateTime(2024, 1, 2), SearchDateTimeConstants.IsBefore, new DateTime(2024, 1, 2), false);
        yield return new DateTimeSpecificationExtensionTestCase(new DateTime(2024, 1, 1, 1, 1, 20), SearchDateTimeConstants.IsOnOrBefore, new DateTime(2024, 1, 1, 1, 2, 0), true);
        yield return new DateTimeSpecificationExtensionTestCase(new DateTime(2024, 1, 1, 1, 3, 20), SearchDateTimeConstants.IsOnOrBefore, new DateTime(2024, 1, 1, 1, 3, 0), false);
        yield return new DateTimeSpecificationExtensionTestCase(new DateTime(2024, 1, 2), SearchDateTimeConstants.IsOnOrBefore, new DateTime(2024, 1, 2), true);
        yield return new DateTimeSpecificationExtensionTestCase(new DateTime(2024, 1, 2), SearchDateTimeConstants.IsEmpty, null, false);
        yield return new DateTimeSpecificationExtensionTestCase(null, SearchDateTimeConstants.IsEmpty, null, true);
        yield return new DateTimeSpecificationExtensionTestCase(null, SearchDateTimeConstants.IsNotEmpty, null, false);
        yield return new DateTimeSpecificationExtensionTestCase(new DateTime(2024, 1, 2), SearchDateTimeConstants.IsNotEmpty, null, true);
    }
}

public class DummyDateTimeModel
{
    public DateTime? DateCreated { get; set; }
}

public class DateTimeSpecificationExtensionTestCase
{
    public DateTime? DateCreated { get; set; }
    public string FilterOperation { get; set; }
    public DateTime? FilterValue { get; set; }
    public bool ExpectedReturns { get; set; }

    public DateTimeSpecificationExtensionTestCase(DateTime? dateCreated, string filterOperation, DateTime? filterValue, bool expectedReturns)
    {
        DateCreated = dateCreated;
        FilterOperation = filterOperation;
        FilterValue = filterValue;
        ExpectedReturns = expectedReturns;
    }
}
