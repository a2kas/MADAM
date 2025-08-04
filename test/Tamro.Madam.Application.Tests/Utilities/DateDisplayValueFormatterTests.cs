using NUnit.Framework;
using Shouldly;
using Tamro.Madam.Application.Utilities;

namespace Tamro.Madam.Application.Tests.Utilities;

[TestFixture]
public class DateDisplayValueFormatterTests
{
    [TestCaseSource(nameof(MaskDatetimeValueTestCases))]
    public void FormatOrDefault_DateTime_ReturnsExpected(DateTime dateTime, string? maskedValue, string? dateFormat, string expected)
    {
        // Act
        var result = DateDisplayValueFormatter.FormatOrDefault(dateTime, maskedValue: maskedValue, dateFormat: dateFormat);

        // Assert
        result.ShouldBe(expected);
    }


    private static IEnumerable<TestCaseData> MaskDatetimeValueTestCases()
    {
        yield return new TestCaseData(new DateTime(0001, 01, 1), null, null, "");
        yield return new TestCaseData(new DateTime(0001, 01, 1), null, "yyyy-MM-dd", "");
        yield return new TestCaseData(new DateTime(0001, 01, 1), "---", null, "---");
        yield return new TestCaseData(new DateTime(2023, 10, 1), null ,"yyyy-MM-dd", "2023-10-01");
        yield return new TestCaseData(new DateTime(2023, 10, 1), null, "yyyy/MM/dd", "2023/10/01");
        yield return new TestCaseData(new DateTime(2023, 10, 1, 14, 30, 0), "---",  "yyyy-MM-dd HH:mm:ss", "2023-10-01 14:30:00");
    }
}
