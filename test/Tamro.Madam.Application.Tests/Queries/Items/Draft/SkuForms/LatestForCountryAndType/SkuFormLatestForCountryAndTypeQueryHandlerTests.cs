using Microsoft.Extensions.Options;
using MockQueryable;
using Moq;
using NUnit.Framework;
using Shouldly;
using Tamro.Madam.Application.Queries.Items.Draft.SkuForms.LatestForCountry;
using Tamro.Madam.Application.Queries.Items.Draft.SkuForms.LatestForCountryAndType;
using Tamro.Madam.Common.Configuration;
using Tamro.Madam.Models.General;
using Tamro.Madam.Models.ItemMasterdata.Draft.SkuForm;
using Tamro.Madam.Repository.Entities.ItemMasterdata.Draft.SkuForms;
using Tamro.Madam.Repository.Repositories;
using Tamro.Madam.Repository.UnitOfWork;
using TamroUtilities.MinIO;
using TamroUtilities.MinIO.Models;

namespace Tamro.Madam.Application.Tests.Queries.Items.Draft.SkuForms.LatestForCountryAndType;

[TestFixture]
public class SkuFormLatestForCountryAndTypeQueryHandlerTests
{
    private Mock<IMadamUnitOfWork> _mockUnitOfWork;
    private Mock<IFileStorage> _mockFileStorage;
    private Mock<IOptions<MinioSettings>> _mockMinioSettings;
    private SkuFormLatestForCountryAndTypeQueryHandler _handler;

    [SetUp]
    public void SetUp()
    {
        _mockUnitOfWork = new Mock<IMadamUnitOfWork>();
        _mockFileStorage = new Mock<IFileStorage>();
        _mockMinioSettings = new Mock<IOptions<MinioSettings>>();

        _mockMinioSettings.Setup(m => m.Value).Returns(new MinioSettings
        {
            MasterdataBucketName = "test-bucket"
        });

        _handler = new SkuFormLatestForCountryAndTypeQueryHandler(
            _mockUnitOfWork.Object,
            _mockFileStorage.Object,
            _mockMinioSettings.Object
        );
    }

    [Test]
    public async Task Handle_ShouldReturnFileWithName_WhenSkuFormIsFound()
    {
        // Arrange
        var query = new SkuFormLatestForCountryAndTypeQuery
        {
            Country = BalticCountry.LT,
            Type = SkuFormType.OtcAndNonMedicine,
        };

        var skuForm = new SkuForm
        {
            Country = BalticCountry.LT,
            Type = SkuFormType.OtcAndNonMedicine,
            VersionMajor = 1,
            VersionMinor = 0,
            FileReference = "/test/file/path"
        };

        var fileData = new byte[] { 1, 2, 3, 4, 5 };
        var skuFormsFound = new[] { skuForm };

        var mockFormsQueryable = skuFormsFound.AsQueryable().BuildMock();

        var mockRepo = new Mock<IRepository<SkuForm>>();
        mockRepo.Setup(r => r.AsReadOnlyQueryable())
            .Returns(mockFormsQueryable);

        _mockUnitOfWork.Setup(u => u.GetRepository<SkuForm>())
            .Returns(mockRepo.Object);

        _mockFileStorage.Setup(fs => fs.Get(It.IsAny<BaseFileRequest>()))
            .ReturnsAsync(fileData);

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        result.ShouldNotBeNull();
        result.Succeeded.ShouldBeTrue();
        result.Data.ShouldNotBeNull();
        result.Data.Name.ShouldBe("NEW_SKU_Tamro_LT_OtcAndNonMedicine_v1.0");
        result.Data.Stream.ShouldNotBeNull();

        using var memoryStream = new MemoryStream();
        await result.Data.Stream.CopyToAsync(memoryStream);
        memoryStream.ToArray().ShouldBe(fileData);
    }
}