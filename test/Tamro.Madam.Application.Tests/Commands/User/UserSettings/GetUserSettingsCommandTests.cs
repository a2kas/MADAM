using Shouldly;
using NUnit.Framework;
using Tamro.Madam.Application.Commands.User.UserSettings;

namespace Tamro.Madam.Application.Tests.Commands.User.UserSettings;

[TestFixture]
public class GetUserSettingsCommandTests
{
    [Test]
    public void GetUserSettingsByUserIdCommand_Ctor_Sets_UserId()
    {
        // Arrange
        var userName = "John Doe";

        // Act
        var cmd = new GetUserSettingsCommand(userName);

        // Assert
        cmd.UserName.ShouldBe(userName);
    }
}
