using Microsoft.Extensions.Options;
using Moq;
using NUnit.Framework;
using Shouldly;
using Tamro.Madam.Application.Commands.Suppliers.Contracts;
using Tamro.Madam.Application.Handlers.Suppliers.Contracts;
using Tamro.Madam.Common.Configuration;
using Tamro.Madam.Models.General;
using TamroUtilities.MinIO;
using TamroUtilities.MinIO.Models;

namespace Tamro.Madam.Tests.Handlers.Suppliers.Contracts;

[TestFixture]
public class UploadContractFileCommandHandlerTests
{
    private Mock<IFileStorage> _fileStorageMock;
    private IOptions<MinioSettings> _minioSettings;
    private UploadContractFileCommandHandler _handler;

    [SetUp]
    public void SetUp()
    {
        _fileStorageMock = new Mock<IFileStorage>();
        _minioSettings = Options.Create(new MinioSettings
        {
            SupplierContractsBucketName = "test-bucket",
            PublicApiUrl = "http://localhost"
        });

        _handler = new UploadContractFileCommandHandler(_fileStorageMock.Object, _minioSettings);
    }

    [TestCase("123456", "lt/123456/")]
    [TestCase("", "lt/tmp/")]
    public async Task Handle_ShouldReturnSuccessResult_WhenFileIsUploadedSuccessfully(string supplierRegistrationNumber, string expectedUrlPrefix)
    {
        // Arrange
        var command = new UploadContractFileCommand(
            country: BalticCountry.LT,
            supplierRegistrationNumber: supplierRegistrationNumber,
            fileName: "test",
            fileContent: new MemoryStream([1, 2, 3]),
            fileExtension: "pdf"
        );

        _fileStorageMock
            .Setup(fs => fs.Create(It.IsAny<CreateStreamedFileRequest>()))
            .Returns(Task.CompletedTask);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.ShouldNotBeNull();
        result.Succeeded.ShouldBeTrue();
        var expectedUrlSuffix = "_contract.pdf";
        result.Data.ShouldStartWith(expectedUrlPrefix);
        result.Data.ShouldEndWith(expectedUrlSuffix);
        _fileStorageMock.Verify(fs => fs.Create(It.IsAny<CreateStreamedFileRequest>()), Times.Once);
    }

    [Test]
    public async Task Handle_ShouldReturnFailureResult_WhenFileUploadFails()
    {
        // Arrange
        var command = new UploadContractFileCommand(
            country: BalticCountry.LT,
            supplierRegistrationNumber: "123456",
            fileName: "test",
            fileContent: new MemoryStream([1, 2, 3]),
            fileExtension: "pdf"
        );

        _fileStorageMock
            .Setup(fs => fs.Create(It.IsAny<CreateStreamedFileRequest>()))
            .ThrowsAsync(new Exception("Upload failed"));

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.Succeeded.ShouldBeFalse();
        result.ErrorMessage.ShouldBe("Upload failed");
        _fileStorageMock.Verify(fs => fs.Create(It.IsAny<CreateStreamedFileRequest>()), Times.Once);
    }
}