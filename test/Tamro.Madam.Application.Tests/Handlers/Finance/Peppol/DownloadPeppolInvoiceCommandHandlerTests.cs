using Microsoft.Extensions.Options;
using Moq;
using NUnit.Framework;
using Shouldly;
using Tamro.Madam.Application.Commands.Finance.Peppol;
using Tamro.Madam.Application.Handlers.Finance.Peppol;
using Tamro.Madam.Common.Configuration;
using TamroUtilities.MinIO;
using TamroUtilities.MinIO.Models;

namespace Tamro.Madam.Application.Tests.Handlers.Finance.Peppol;

public class DownloadPeppolInvoiceCommandHandlerTests
{
    private MockRepository _mockRepository;

    private Mock<IFileStorage> _fileStorage;
    private IOptions<MinioSettings> _minioSettings;

    private DownloadPeppolInvoiceCommandHandler _downloadPeppolInvoiceCommandHandler;

    [SetUp]
    public void SetUp()
    {
        _mockRepository = new MockRepository(MockBehavior.Loose);

        _fileStorage = _mockRepository.Create<IFileStorage>();
        _minioSettings = Options.Create(new MinioSettings
        {
            PeppolInvoicesBucketName = "test-bucket",
            PublicApiUrl = "http://localhost"
        });

        _downloadPeppolInvoiceCommandHandler = new DownloadPeppolInvoiceCommandHandler(_fileStorage.Object, _minioSettings);
    }

    [Test]
    public async Task Handle_ShouldReturnSuccessResult_WhenFileIsDownloadedSuccessfully()
    {
        // Arrange
        var command = new DownloadPeppolInvoiceCommand("123");

        _fileStorage.Setup(fs => fs.Get(It.Is<BaseFileRequest>(y => y.BucketName == "test-bucket" && y.Path == "lv/123.xml")))
            .ReturnsAsync(""u8.ToArray());

        // Act
        var result = await _downloadPeppolInvoiceCommandHandler.Handle(command, CancellationToken.None);

        // Assert
        result.Succeeded.ShouldBeTrue();
    }

    [Test]
    public async Task Handle_ShouldReturnFailureResult_WhenFileDownloadFails()
    {
        // Arrange
        var command = new DownloadPeppolInvoiceCommand("123");

        _fileStorage.Setup(fs => fs.Get(It.IsAny<BaseFileRequest>()))
            .ThrowsAsync(new Exception());

        // Act
        var result = await _downloadPeppolInvoiceCommandHandler.Handle(command, CancellationToken.None);

        // Assert
        result.Succeeded.ShouldBeFalse();
    }
}