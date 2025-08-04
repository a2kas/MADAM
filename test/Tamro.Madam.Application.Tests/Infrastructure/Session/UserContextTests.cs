using Shouldly;
using NUnit.Framework;
using Tamro.Madam.Application.Infrastructure.Session;

namespace Tamro.Madam.Application.Tests.Infrastructure.Session;

[TestFixture]
public class UserContextTests
{
    [Test]
    public void HasPermission_UserHasPermission_ReturnsTrue()
    {
        // Arrange
        var ctx = new UserContext
        {
            Permissions = ["CanView", "CanEdit",]
        };

        // Act
        var result = ctx.HasPermission("CanEdit");

        // Assert
        result.ShouldBe(true);
    }

    [Test]
    public void HasPermission_UserDoesNotHavePermission_ReturnsFalse()
    {
        // Arrange
        var ctx = new UserContext
        {
            Permissions = new string[] { "CanView", }
        };

        // Act
        var result = ctx.HasPermission("CanEdit");

        // Assert
        result.ShouldBe(false);
    }
}