using AngleSharp.Diffing.Extensions;
using AutoFixture;
using MockQueryable;
using Moq;
using NUnit.Framework;
using Shouldly;
using Tamro.Madam.Application.Commands.ItemMasterdata.Items.QualityCheck.Upsert;
using Tamro.Madam.Models.ItemMasterdata.Items.QualityCheck;
using Tamro.Madam.Repository.Entities.ItemMasterdata.Items.QualityCheck;
using Tamro.Madam.Repository.UnitOfWork;

namespace Tamro.Madam.Application.Tests.Commands.ItemMasterdata.Items.QualityCheck.Upsert;

[TestFixture]
public class UpsertQualityCheckCommandHandlerTests
{
    private Fixture _fixture;
    private MockRepository _mockRepository;

    private Mock<IMadamUnitOfWork> _madamUnitOfWork;

    private UpsertQualityCheckCommandHandler _handler;

    [SetUp]
    public void SetUp()
    {
        _fixture = new Fixture();
        _fixture.Behaviors.OfType<ThrowingRecursionBehavior>().ToList()
            .ForEach(b => _fixture.Behaviors.Remove(b));
        _fixture.Behaviors.Add(new OmitOnRecursionBehavior());
        _mockRepository = new MockRepository(MockBehavior.Loose);

        _madamUnitOfWork = _mockRepository.Create<IMadamUnitOfWork>();

        _handler = new UpsertQualityCheckCommandHandler(_madamUnitOfWork.Object);
    }

    [Test]
    public async Task Handle_ShouldAddNewIssues_WhenExistingEntityHasNoMatchingIssues()
    {
        // Arrange
        var command = _fixture.Create<UpsertQualityCheckCommand>();
        var existingEntity = _fixture.Build<ItemQualityCheck>()
            .With(x => x.ItemId, command.Model.ItemId)
            .Create();
        existingEntity.ItemQualityCheckIssues.Clear(); // No existing issues

        _madamUnitOfWork.Setup(uow => uow.GetRepository<ItemQualityCheck>().AsQueryable())
            .Returns(new[] { existingEntity }.BuildMock());

        _madamUnitOfWork.Setup(uow => uow.GetRepository<ItemQualityCheck>().UpsertAsync(It.IsAny<ItemQualityCheck>()))
            .ReturnsAsync(existingEntity);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.Succeeded.ShouldBeTrue();
        result.Data.ShouldBe(existingEntity.Id);
        existingEntity.ItemQualityCheckIssues.Count.ShouldBe(command.Model.ItemQualityCheckIssues.Count);
        _madamUnitOfWork.Verify(uow => uow.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Test]
    public async Task Handle_ShouldNotAddDuplicateIssues_WhenExistingEntityHasMatchingIssues()
    {
        // Arrange
        var command = _fixture.Create<UpsertQualityCheckCommand>();
        foreach (var issue in command.Model.ItemQualityCheckIssues)
        {
            issue.IssueField = "dummyField";
        }
        var existingEntity = _fixture.Build<ItemQualityCheck>()
            .With(x => x.ItemId, command.Model.ItemId)
            .Create();

        var matchingIssues = _fixture.CreateMany<ItemQualityCheckIssue>(3).ToList();
        foreach (var issue in matchingIssues)
        {
            issue.IssueStatus = ItemQualityIssueStatus.New;
            issue.IssueField = "dummyField";
        }

        command.Model.ItemQualityCheckIssues = matchingIssues;
        existingEntity.ItemQualityCheckIssues.AddRange(matchingIssues);

        _madamUnitOfWork.Setup(uow => uow.GetRepository<ItemQualityCheck>().AsQueryable())
            .Returns(new[] { existingEntity }.BuildMock());

        _madamUnitOfWork.Setup(uow => uow.GetRepository<ItemQualityCheck>().UpsertAsync(It.IsAny<ItemQualityCheck>()))
            .ReturnsAsync(existingEntity);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.Succeeded.ShouldBeTrue();
        result.Data.ShouldBe(existingEntity.Id);
        existingEntity.ItemQualityCheckIssues.Count.ShouldBe(existingEntity.ItemQualityCheckIssues.Count);
        _madamUnitOfWork.Verify(uow => uow.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Test]
    public async Task Handle_ShouldCreateNewEntity_WhenNoExistingEntityFound()
    {
        // Arrange
        var command = _fixture.Create<UpsertQualityCheckCommand>();

        _madamUnitOfWork.Setup(uow => uow.GetRepository<ItemQualityCheck>().AsQueryable())
            .Returns(Enumerable.Empty<ItemQualityCheck>().BuildMock());

        _madamUnitOfWork.Setup(uow => uow.GetRepository<ItemQualityCheck>().UpsertAsync(It.IsAny<ItemQualityCheck>()))
            .ReturnsAsync(command.Model);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.Succeeded.ShouldBeTrue();
        result.Data.ShouldBe(command.Model.Id);
        _madamUnitOfWork.Verify(uow => uow.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }
}