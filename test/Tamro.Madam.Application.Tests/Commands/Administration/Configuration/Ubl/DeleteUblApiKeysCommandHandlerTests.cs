using AutoFixture;
using Moq;
using NUnit.Framework;
using Shouldly;
using Tamro.Madam.Application.Commands.Administration.Configuration.Ubl;
using Tamro.Madam.Repository.Repositories.Administration.Configuration.Ubl;

namespace Tamro.Madam.Application.Tests.Commands.Administration.Configuration.Ubl;
public class DeleteUblApiKeysCommandHandlerTests
{
    private Fixture _fixture;

    private Mock<IUblApiKeyRepository> _ublApiKeyRepository;

    private DeleteUblApiKeysCommandHandler _deleteUblApiKeysCommandHandler;

    [SetUp]
    public void SetUp()
    {
        _fixture = new Fixture();

        _ublApiKeyRepository = new Mock<IUblApiKeyRepository>();

        _deleteUblApiKeysCommandHandler = new DeleteUblApiKeysCommandHandler(_ublApiKeyRepository.Object);
    }

    [Test]
    public async Task Handle_DeletesMany()
    {
        // Arrange
        var command = _fixture.Create<DeleteUblApiKeysCommand>();
        var cancelationToken = _fixture.Create<CancellationToken>();
        _ublApiKeyRepository.Setup(x => x.DeleteMany(command.E1SoldTos, cancelationToken)).ReturnsAsync(command.E1SoldTos.Count());

        // Act
        var result = await _deleteUblApiKeysCommandHandler.Handle(command, cancelationToken);

        // Assert
        result.Data.ShouldBe(command.E1SoldTos.Count());
        _ublApiKeyRepository.Verify(x => x.DeleteMany(command.E1SoldTos, cancelationToken), Times.Once);
    }
}
