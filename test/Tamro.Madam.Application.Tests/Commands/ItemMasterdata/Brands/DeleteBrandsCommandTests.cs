using Shouldly;
using NUnit.Framework;
using Tamro.Madam.Application.Commands.Brands;

namespace Tamro.Madam.Application.Tests.Commands.Brands;

[TestFixture]
public class DeleteBrandsCommandTests
{
    [Test]
    public void DeleteBrandsCommand_Ctor_Sets_Id()
    {
        // Arrange
        var ids = new List<int>() { 4, 5, 6 };

        // Act
        var cmd = new DeleteBrandsCommand(ids);

        // Assert
        cmd.Id.ShouldBeEquivalentTo(ids);
    }
}