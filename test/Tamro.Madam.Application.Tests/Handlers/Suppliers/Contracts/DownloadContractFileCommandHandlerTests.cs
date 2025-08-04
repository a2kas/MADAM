using Shouldly;
using Microsoft.Extensions.Options;
using Moq;
using NUnit.Framework;
using Tamro.Madam.Application.Commands.Suppliers.Contracts;
using Tamro.Madam.Application.Handlers.Suppliers.Contracts;
using Tamro.Madam.Common.Configuration;
using TamroUtilities.MinIO;
using TamroUtilities.MinIO.Models;

namespace Tamro.Madam.Application.Tests.Handlers.Suppliers.Contracts;

[TestFixture]
public class DownloadContractFileCommandHandlerTests
{
    private MockRepository _mockRepository;

    private Mock<IFileStorage> _fileStorage;
    private IOptions<MinioSettings> _minioSettings;

    private DownloadContractFileCommandHandler _downloadContractFileCommandHandler;

    [SetUp]
    public void SetUp()
    {
        _mockRepository = new MockRepository(MockBehavior.Loose);

        _fileStorage = _mockRepository.Create<IFileStorage>();
        _minioSettings = Options.Create(new MinioSettings
        {
            SupplierContractsBucketName = "test-bucket",
            PublicApiUrl = "http://localhost"
        });

        _downloadContractFileCommandHandler = new DownloadContractFileCommandHandler(_fileStorage.Object, _minioSettings);
    }

    [Test]
    public async Task Handle_ShouldReturnSuccessResult_WhenFileIsDownloadedSuccessfully()
    {
        // Arrange
        var command = new DownloadContractFileCommand("lv/test/");

        _fileStorage.Setup(fs => fs.Get(It.Is<BaseFileRequest>(y => y.BucketName == "test-bucket" && y.Path == "lv/test/")))
            .ReturnsAsync(""u8.ToArray());

        // Act
        var result = await _downloadContractFileCommandHandler.Handle(command, CancellationToken.None);

        // Assert
        result.Succeeded.ShouldBeTrue();
    }

    [Test]
    public async Task Handle_ShouldReturnFailureResult_WhenFileDownloadFails()
    {
        // Arrange
        var command = new DownloadContractFileCommand("lv/test/");

        _fileStorage.Setup(fs => fs.Get(It.IsAny<BaseFileRequest>()))
            .ThrowsAsync(new Exception());

        // Act
        var result = await _downloadContractFileCommandHandler.Handle(command, CancellationToken.None);

        // Assert
        result.Succeeded.ShouldBeFalse();
    }
}