using AutoFixture;
using Microsoft.EntityFrameworkCore;
using MockQueryable;
using Moq;
using NUnit.Framework;
using Shouldly;
using Tamro.Madam.Application.Commands.ItemMasterdata.CategoryManagers.Delete;
using Tamro.Madam.Repository.Entities.ItemMasterdata.CategoryManagers;
using Tamro.Madam.Repository.Repositories;
using Tamro.Madam.Repository.UnitOfWork;

namespace Tamro.Madam.Application.Tests.Commands.ItemMasterdata.CategoryManagers;

[TestFixture]
public class DeleteCategoryManagersCommandHandlerTests
{
    private Mock<IMadamUnitOfWork> _mockUnitOfWork;
    private DeleteCategoryManagersCommandHandler _handler;
    private Fixture _fixture;

    [SetUp]
    public void SetUp()
    {
        _fixture = new Fixture();
        _fixture.Behaviors.OfType<ThrowingRecursionBehavior>().ToList()
                .ForEach(b => _fixture.Behaviors.Remove(b));
        _fixture.Behaviors.Add(new OmitOnRecursionBehavior());
        _mockUnitOfWork = new Mock<IMadamUnitOfWork>();
        _handler = new DeleteCategoryManagersCommandHandler(_mockUnitOfWork.Object);
    }

    [Test]
    public async Task Handle_ShouldReturnSuccessResult_WhenEntitiesAreDeleted()
    {
        // Arrange
        var command = new DeleteCategoryManagersCommand([1, 2, 3]);
        var categoryManagers = _fixture.CreateMany<CategoryManager>(command.Ids.Count).ToList();

        foreach (var manager in categoryManagers)
        {
            manager.Id = command.Ids[categoryManagers.IndexOf(manager)];
        }
        var mockDataQueryable = categoryManagers.AsQueryable().BuildMock();

        var mockRepo = new Mock<IRepository<CategoryManager>>();
        mockRepo.Setup(r => r.AsReadOnlyQueryable()).Returns(mockDataQueryable);
        mockRepo.Setup(r => r.DeleteMany(It.IsAny<ICollection<CategoryManager>>()));

        var first = await mockDataQueryable.FirstOrDefaultAsync();

        _mockUnitOfWork.Setup(u => u.GetRepository<CategoryManager>()).Returns(mockRepo.Object);
        _mockUnitOfWork.Setup(u => u.SaveChangesAsync(It.IsAny<CancellationToken>())).ReturnsAsync(command.Ids.Count);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.ShouldNotBeNull();
        result.Succeeded.ShouldBeTrue();
        result.Data.ShouldBe(command.Ids.Count);
    }

    [Test]
    public void Handle_ShouldThrowArgumentException_WhenNoEntitiesFound()
    {
        // Arrange
        var command = _fixture.Create<DeleteCategoryManagersCommand>();

        var mockRepo = new Mock<IRepository<CategoryManager>>();
        mockRepo.Setup(r => r.AsReadOnlyQueryable())
            .Returns(Enumerable.Empty<CategoryManager>().BuildMock());

        _mockUnitOfWork.Setup(u => u.GetRepository<CategoryManager>()).Returns(mockRepo.Object);

        // Act & Assert
        Assert.ThrowsAsync<ArgumentException>(async () => await _handler.Handle(command, CancellationToken.None));
    }
}
