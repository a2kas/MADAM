using AutoFixture;
using AutoMapper;
using Moq;
using NUnit.Framework;
using Shouldly;
using Tamro.Madam.Application.Commands.Administration.Configuration.Ubl;
using Tamro.Madam.Models.Administration.Configuration.Ubl;
using Tamro.Madam.Repository.Repositories.Administration.Configuration.Ubl;

namespace Tamro.Madam.Application.Tests.Commands.Administration.Configuration.Ubl;

[TestFixture]
public class UpsertUblApiKeyCommandHandlerTests
{
    private Fixture _fixture;

    private Mock<IUblApiKeyRepository> _ublApiKeyRepository;
    private Mock<IMapper> _mapper;

    private UpsertUblApiKeyCommandHandler _upsertUblApiKeyCommandHandler;

    [SetUp]
    public void SetUp()
    {
        _fixture = new Fixture();

        _ublApiKeyRepository = new Mock<IUblApiKeyRepository>();
        _mapper = new Mock<IMapper>();

        _upsertUblApiKeyCommandHandler = new UpsertUblApiKeyCommandHandler(_ublApiKeyRepository.Object, _mapper.Object);
    }

    [Test]
    public async Task Handle_UpsertsMappedUblApiKeyModel()
    {
        // Arrange
        var command = _fixture.Create<UpsertUblApiKeyCommand>();
        var ublApiKeyModel = _fixture.Create<UblApiKeyModel>();
        var upsertResult = _fixture.Create<UblApiKeyModel>();
        _mapper.Setup(x => x.Map<UblApiKeyModel>(command.Model)).Returns(ublApiKeyModel);
        _ublApiKeyRepository.Setup(x => x.Upsert(ublApiKeyModel)).ReturnsAsync(upsertResult);

        // Act
        var result = await _upsertUblApiKeyCommandHandler.Handle(command, new CancellationToken());

        // Assert
        _ublApiKeyRepository.Verify(x => x.Upsert(ublApiKeyModel), Times.Once);
        _mapper.Verify(x => x.Map<UblApiKeyModel>(command.Model), Times.Once);
        result.Data.ShouldBe(upsertResult.E1SoldTo);
    }
}
