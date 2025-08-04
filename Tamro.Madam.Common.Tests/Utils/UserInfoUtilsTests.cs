using Shouldly;
using NUnit.Framework;
using Tamro.Madam.Common.Utils;

namespace Tamro.Madam.Common.Tests.Utils;

[TestFixture]
public class UserInfoUtilsTests
{
    [TestCase("John.Doe", "JD")]
    [TestCase("", "")]
    [TestCase("John", "J")]
    [TestCase("John.Doe.Smith", "JDS")]

    public void GetInitials_ShouldReturnCorrectInitials(string input, string expected)
    {
        // Act
        string result = UserInfoUtils.GetInitials(input);

        // Assert
        result.ShouldBe(expected);
    }
}
