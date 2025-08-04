using AutoFixture;
using MockQueryable;
using Moq;
using NUnit.Framework;
using Shouldly;
using Tamro.Madam.Application.Commands.Hr.Jira.Administration;
using Tamro.Madam.Repository.Entities.Jpg;
using Tamro.Madam.Repository.UnitOfWork;

namespace Tamro.Madam.Application.Tests.Commands.Hr.Jira.Administration;
[TestFixture]
public class SetIsAccountsIsActivatedCommandHandlerTests
{
    private Mock<IJpgUnitOfWork> _uow;
    private SetAccountsIsActivatedCommandHandler _handler;
    private Fixture _fixture;

    [SetUp]
    public void SetUp()
    {
        _uow = new Mock<IJpgUnitOfWork>();
        _handler = new SetAccountsIsActivatedCommandHandler(_uow.Object);
        _fixture = new Fixture();
    }

    [TestCase(false)]
    [TestCase(true)]
    public async Task Handle_SetsIsActivatedAndReturnsSuccessResult(bool isActivated)
    {
        // Arrange
        var command = new SetAccountsIsActivatedCommand(["account1", "account2"], isActivated);

        var jiraAccounts = _fixture.Build<JiraAccount>()
            .With(x => x.IsActive, !isActivated)
            .CreateMany(2)
            .ToList();
        jiraAccounts[0].Id = "account1";
        jiraAccounts[1].Id = "account2";

        _uow.Setup(u => u.GetRepository<JiraAccount>().AsQueryable())
            .Returns(jiraAccounts.BuildMock());

        _uow.Setup(u => u.SaveChangesAsync(CancellationToken.None)).ReturnsAsync(2);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.ShouldNotBeNull();
        result.Succeeded.ShouldBeTrue();
        result.Data.ShouldBe(2);
        jiraAccounts.ForEach(account => account.IsActive.ShouldBe(isActivated));
    }
}
