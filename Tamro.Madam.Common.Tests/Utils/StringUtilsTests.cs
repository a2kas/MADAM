using NUnit.Framework;
using Shouldly;
using Tamro.Madam.Common.Utils;

namespace Tamro.Madam.Common.Tests.Utils;

[TestFixture]
public class StringUtilsTests
{
    [TestCase("WillNotBeSent", "Will not be sent")]
    [TestCase("Sent", "Sent")]
    [TestCase("WillSend", "Will send")]
    public void ConvertCamelCaseToSentence_Converts_Correctly(string input, string expectedSentence)
    {
        // Act
        var result = StringUtils.ConvertCamelCaseToSentence(input);

        // Assert
        result.ShouldBe(expectedSentence);
    }
}