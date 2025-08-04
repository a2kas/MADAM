using Shouldly;
using NUnit.Framework;
using Tamro.Madam.Application.Commands.User.UserSettings;
using Tamro.Madam.Models.User.UserSettings;

namespace Tamro.Madam.Application.Tests.Commands.User.UserSettings;

public class UpsertUserSettingsCommandTests
{
    [Test]
    public async Task Ctor_SetsValues()
    {
        // Arrange
        var request = new UserSettingsModel
        {
            Usability = new UserUsabilitySettingsModel
            {
                RowsPerPage = 10,
            }
        };

        // Act
        var cmd = new UpsertUserSettingsCommand(request);

        // Assert
        cmd.Model.ShouldBe(request);
    }
}
