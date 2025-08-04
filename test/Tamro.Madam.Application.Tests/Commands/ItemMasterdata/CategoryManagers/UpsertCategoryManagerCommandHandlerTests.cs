using AutoFixture;
using AutoMapper;
using Moq;
using NUnit.Framework;
using Shouldly;
using Tamro.Madam.Application.Commands.ItemMasterdata.CategoryManagers.Upsert;
using Tamro.Madam.Repository.Entities.ItemMasterdata.CategoryManagers;
using Tamro.Madam.Repository.UnitOfWork;

namespace Tamro.Madam.Application.Tests.Commands.ItemMasterdata.CategoryManagers;

[TestFixture]
public class UpsertCategoryManagerCommandHandlerTests
{
    private Mock<IMadamUnitOfWork> _mockUnitOfWork;
    private Mock<IMapper> _mockMapper;
    private UpsertCategoryManagerCommandHandler _handler;
    private Fixture _fixture;

    [SetUp]
    public void SetUp()
    {
        _fixture = new Fixture();
        _fixture.Behaviors.OfType<ThrowingRecursionBehavior>().ToList()
                .ForEach(b => _fixture.Behaviors.Remove(b));
        _fixture.Behaviors.Add(new OmitOnRecursionBehavior());
        _mockUnitOfWork = new Mock<IMadamUnitOfWork>();
        _mockMapper = new Mock<IMapper>();
        _handler = new UpsertCategoryManagerCommandHandler(_mockUnitOfWork.Object, _mockMapper.Object);
    }

    [Test]
    public async Task Handle_ShouldReturnSuccessResult_WhenUpsertIsSuccessful()
    {
        // Arrange
        var command = _fixture.Create<UpsertCategoryManagerCommand>();

        var categoryManagerMappingResult = _fixture.Create<CategoryManager>();

        var categoryManagerUpsertingResult = _fixture.Create<CategoryManager>();
        categoryManagerUpsertingResult.Id = 1;

        _mockMapper.Setup(m => m.Map<CategoryManager>(command)).Returns(categoryManagerMappingResult);

        _mockUnitOfWork.Setup(u => u
            .GetRepository<CategoryManager>()
                .UpsertAsync(categoryManagerMappingResult))
        .ReturnsAsync(categoryManagerUpsertingResult);

        _mockUnitOfWork.Setup(u => u.SaveChangesAsync(It.IsAny<CancellationToken>())).ReturnsAsync(1);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.ShouldNotBeNull();
        result.Succeeded.ShouldBeTrue();
        result.Data.ShouldBe(categoryManagerUpsertingResult.Id);
    }
}
