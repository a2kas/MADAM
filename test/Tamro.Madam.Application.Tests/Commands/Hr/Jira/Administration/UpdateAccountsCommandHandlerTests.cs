using System.Reflection;
using AutoFixture;
using AutoMapper;
using MockQueryable;
using Moq;
using NUnit.Framework;
using Shouldly;
using Tamro.Madam.Application.Commands.Hr.Jira.Administration;
using Tamro.Madam.Application.Profiles.Jpg;
using Tamro.Madam.Models.Hr.Jira.Administration;
using Tamro.Madam.Repository.Entities.Jpg;
using Tamro.Madam.Repository.Repositories;
using Tamro.Madam.Repository.UnitOfWork;

namespace Tamro.Madam.Application.Tests.Commands.Hr.Jira.Administration;
[TestFixture]
public class UpdateAccountsCommandHandlerTests
{
    private Fixture _fixture;

    private Mock<IJpgUnitOfWork> _uow;
    private IMapper _mapper;
    private UpdateAccountsCommandHandler _handler;

    [SetUp]
    public void SetUp()
    {
        _fixture = new Fixture();

        _uow = new Mock<IJpgUnitOfWork>();
        _mapper = new Mapper(new MapperConfiguration(cfg => cfg.AddMaps(Assembly.GetAssembly(typeof(JiraProfile)))));

        _handler = new UpdateAccountsCommandHandler(_uow.Object, _mapper);
    }

    [Test]
    public async Task Handle_UpdatesAccountAndReturnsSuccessResult()
    {
        // Arrange
        var accountModel = _fixture.Create<JiraAccountModel>();
        var command = new UpdateAccountsCommand(accountModel);

        var jiraAccount = _fixture.Build<JiraAccount>()
            .With(x => x.Id, accountModel.Id)
            .Create();

        var repo = new Mock<IRepository<JiraAccount>>();
        repo.Setup(r => r.AsQueryable())
            .Returns(new List<JiraAccount> { jiraAccount }.BuildMock());
        repo.Setup(r => r.UpsertAsync(It.IsAny<JiraAccount>()))
            .ReturnsAsync(jiraAccount);

        _uow.Setup(u => u.GetRepository<JiraAccount>()).Returns(repo.Object);
        _uow.Setup(u => u.SaveChangesAsync(CancellationToken.None)).ReturnsAsync(1);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.ShouldNotBeNull();
        result.Succeeded.ShouldBeTrue();
        result.Data.Id.ShouldBe(accountModel.Id);
    }

    [Test]
    public async Task Handle_FailsToUpdateAccount_ReturnsFailureResult()
    {
        // Arrange
        var accountModel = _fixture.Create<JiraAccountModel>();
        var command = new UpdateAccountsCommand(accountModel);

        var jiraAccount = _fixture.Build<JiraAccount>()
            .With(x => x.Id, accountModel.Id)
            .Create();

        var repo = new Mock<IRepository<JiraAccount>>();
        repo.Setup(r => r.AsQueryable())
            .Returns(new List<JiraAccount> { jiraAccount }.BuildMock());
        repo.Setup(r => r.UpsertAsync(It.IsAny<JiraAccount>()))
            .ReturnsAsync(jiraAccount);

        _uow.Setup(u => u.GetRepository<JiraAccount>()).Returns(repo.Object);
        _uow.Setup(u => u.SaveChangesAsync(CancellationToken.None)).ReturnsAsync(0);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.ShouldNotBeNull();
        result.Succeeded.ShouldBeFalse();
        result.Errors.ShouldNotBeEmpty();
    }
}