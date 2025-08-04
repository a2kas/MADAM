using AutoFixture;
using AutoMapper;
using Microsoft.Extensions.Options;
using Moq;
using NUnit.Framework;
using Shouldly;
using Tamro.Madam.Application.Commands.ItemMasterdata.CategoryManagers.Upsert;
using Tamro.Madam.Application.Commands.ItemMasterdata.Draft.NewProductOffers.Upsert;
using Tamro.Madam.Application.Validation;
using Tamro.Madam.Common.Configuration;
using Tamro.Madam.Models.ItemMasterdata.Draft.NewProductOffers;
using Tamro.Madam.Repository.Entities.ItemMasterdata.Draft.NewProductOffers;
using Tamro.Madam.Repository.UnitOfWork;
using TamroUtilities.MinIO;

namespace Tamro.Madam.Application.Tests.Commands.ItemMasterdata.Draft.NewProductOffers
{
    [TestFixture]
    public class UpsertNewProductOfferCommandHandlerTests
    {
        private Mock<IMadamUnitOfWork> _mockUnitOfWork;
        private Mock<IMapper> _mockMapper;
        private Mock<IFileStorage> _mockFileStorage;
        private Mock<IHandlerValidator> _mockValidationService;
        private Mock<IOptions<MinioSettings>> _mockMinioSettings;
        private UpsertNewProductOfferCommandHandler _handler;
        private Fixture _fixture;

        [SetUp]
        public void SetUp()
        {
            _fixture = new Fixture();
            _fixture.Behaviors.OfType<ThrowingRecursionBehavior>().ToList()
                    .ForEach(b => _fixture.Behaviors.Remove(b));
            _fixture.Behaviors.Add(new OmitOnRecursionBehavior());
            _fixture.Customizations.Add(new MemoryStreamSpecimenBuilder());

            _mockUnitOfWork = new Mock<IMadamUnitOfWork>();
            _mockMapper = new Mock<IMapper>();
            _mockFileStorage = new Mock<IFileStorage>();
            _mockValidationService = new Mock<IHandlerValidator>();
            _mockMinioSettings = new Mock<IOptions<MinioSettings>>();

            _mockMinioSettings.Setup(m => m.Value).Returns(new MinioSettings
            {
                MasterdataBucketName = "test-bucket",
                ReferenceBaseUrl = "http://minio.example.com"
            });

            _handler = new UpsertNewProductOfferCommandHandler(
                _mockUnitOfWork.Object,
                _mockMapper.Object,
                _mockFileStorage.Object,
                _mockValidationService.Object,
                _mockMinioSettings.Object);
        }

        [Test]
        public async Task Handle_ShouldReturnSuccessResult_WhenUpsertIsSuccessful()
        {
            // Arrange
            var command = _fixture.Create<UpsertNewProductOfferCommand>();

            var newProductOfferMappingResult = _fixture.Create<NewProductOffer>();
            var newProductOfferUpsertingResult = _fixture.Create<NewProductOffer>();
            newProductOfferUpsertingResult.Id = 1;
            var requestResult = _fixture.Create<UpsertNewProductOfferResult>();
            requestResult.Id = 1;

            _mockMapper.Setup(m => m.Map<NewProductOffer>(command)).Returns(newProductOfferMappingResult);
            _mockMapper.Setup(m => m.Map<UpsertNewProductOfferResult>(newProductOfferUpsertingResult)).Returns(requestResult);


            _mockUnitOfWork.Setup(u => u
                .GetRepository<NewProductOffer>()
                .UpsertAsync(newProductOfferMappingResult))
            .ReturnsAsync(newProductOfferUpsertingResult);

            _mockUnitOfWork.Setup(u => u.SaveChangesAsync(It.IsAny<CancellationToken>())).ReturnsAsync(1);

            var uploadSetup = _mockFileStorage.Setup(f => f.Create(It.IsAny<TamroUtilities.MinIO.Models.CreateStreamedFileRequest>()))
                .Returns(Task.CompletedTask);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            result.ShouldNotBeNull();
            result.Succeeded.ShouldBeTrue();
            result.Data.Id.ShouldBe(newProductOfferUpsertingResult.Id);
            _mockFileStorage.Verify(f => f.Create(It.IsAny<TamroUtilities.MinIO.Models.CreateStreamedFileRequest>()), Times.Once);
        }
    }
}