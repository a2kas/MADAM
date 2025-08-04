using NUnit.Framework;
using Shouldly;
using Tamro.Madam.Application.Utilities;

namespace Tamro.Madam.Application.Tests.Utilities;
public enum TestEnum
{
    Foo,
    Bar
}

[TestFixture]
public class EnumParserTests
{
    [TestCase("Foo", TestEnum.Foo)]
    [TestCase("", null)]
    [TestCase(null, null)]
    [TestCase("SomethingTotallyNotExisting", null)]
    public void ParseNullable_ParsesCorrectly(string? value, TestEnum? expected)
    {
        // Act
        var result = EnumParser.ParseNullable<TestEnum>(value);

        // Assert
        result.ShouldBe(expected);
    }

}
