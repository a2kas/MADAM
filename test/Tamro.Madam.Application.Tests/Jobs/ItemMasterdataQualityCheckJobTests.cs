using AutoFixture;
using MediatR;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using Shouldly;
using Tamro.DocuQueryService.Client;
using Tamro.Madam.Application.Commands.ItemMasterdata.Items.QualityCheck.Upsert;
using Tamro.Madam.Application.Jobs;
using Tamro.Madam.Application.Models.Common;
using Tamro.Madam.Repository.Entities.ItemMasterdata.Items;
using Tamro.Madam.Application.Services.Items.QualityCheck;
using Tamro.Madam.Models.ItemMasterdata.Items.QualityCheck;
using Tamro.Madam.Repository.Entities.ItemMasterdata.Items.QualityCheck;

namespace Tamro.Madam.Application.Tests.Jobs;

[TestFixture]
public class ItemMasterdataQualityCheckJobTests
{
    private Fixture _fixture;
    private Mock<IItemMasterdataQualityCheckService> _qualityCheckService;
    private Mock<IQualityCheckAiConsumerService> _aiConsumerService;
    private Mock<IExtractionClient> _extractionClient;
    private Mock<ILogger<ItemMasterdataQualityCheckJob>> _logger;
    private Mock<IMediator> _mediator;
    private ItemMasterdataQualityCheckJob _job;

    [SetUp]
    public void SetUp()
    {
        _fixture = new Fixture();
        _fixture.Behaviors.OfType<ThrowingRecursionBehavior>().ToList()
            .ForEach(b => _fixture.Behaviors.Remove(b));
        _fixture.Behaviors.Add(new OmitOnRecursionBehavior());
        _qualityCheckService = new Mock<IItemMasterdataQualityCheckService>();
        _aiConsumerService = new Mock<IQualityCheckAiConsumerService>();
        _extractionClient = new Mock<IExtractionClient>();
        _logger = new Mock<ILogger<ItemMasterdataQualityCheckJob>>();
        _mediator = new Mock<IMediator>();

        _job = new ItemMasterdataQualityCheckJob(
            _qualityCheckService.Object,
            _aiConsumerService.Object,
            _extractionClient.Object,
            _logger.Object,
            _mediator.Object
        );
    }

    [Test]
    public async Task Execute_ShouldProcessItemsAndReturnSuccessResult()
    {
        // Arrange
        var items = _fixture.CreateMany<Item>(3);
        var reference = _fixture.Create<ItemQualityCheckReferenceModel>();
        var apiResponse = _fixture.Create<ItemQualityCheckApiResponseModel>();
        var qualityCheckResult = _fixture.Create<ItemQualityCheck>();

        _qualityCheckService.Setup(s => s.GetItems()).ReturnsAsync(items);
        _aiConsumerService.Setup(s => s.ConstructReference(It.IsAny<Item>())).ReturnsAsync(reference);
        _extractionClient.Setup(c => c.ExtractAsync(null, reference.Text, It.IsAny<string>(), It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(apiResponse);
        _qualityCheckService.Setup(s => s.PerformQualityCheck(It.IsAny<ItemQualityCheckReferenceModel>(), It.IsAny<ItemQualityCheckApiResponseModel>())).Returns(qualityCheckResult);
        _mediator.Setup(m => m.Send(It.IsAny<UpsertQualityCheckCommand>(), default))
            .ReturnsAsync(Result<int>.Success(qualityCheckResult.Id));

        // Act
        var result = await _job.Execute();

        // Assert
        result.Succeeded.ShouldBeTrue();
        result.Data.ShouldBe(items.Count());
        _mediator.Verify(m => m.Send(It.IsAny<UpsertQualityCheckCommand>(), default), Times.Exactly(items.Count()));
    }

    [Test]
    public async Task Execute_ErrorForOneItem_ShouldContinueProcessing()
    {
        // Arrange
        var items = _fixture.CreateMany<Item>(3);
        var reference = _fixture.Create<ItemQualityCheckReferenceModel>();
        var apiResponse = _fixture.Create<ItemQualityCheckApiResponseModel>();
        var qualityCheckResult = _fixture.Create<ItemQualityCheck>();

        _qualityCheckService.Setup(s => s.GetItems()).ReturnsAsync(items);
        _aiConsumerService.Setup(s => s.ConstructReference(It.IsAny<Item>())).ReturnsAsync(reference);
        _extractionClient.Setup(c => c.ExtractAsync(null, reference.Text, It.IsAny<string>(), It.IsAny<string>(), It.IsAny<CancellationToken>()))
                        .ThrowsAsync(new Exception("Extraction failed"));
        _qualityCheckService.Setup(s => s.PerformQualityCheck(It.IsAny<ItemQualityCheckReferenceModel>(), It.IsAny<ItemQualityCheckApiResponseModel>())).Returns(qualityCheckResult);
        _mediator.Setup(m => m.Send(It.IsAny<UpsertQualityCheckCommand>(), default))
            .ReturnsAsync(Result<int>.Success(qualityCheckResult.Id));

        // Act
        var result = await _job.Execute();

        // Assert
        result.Succeeded.ShouldBeTrue();
        result.Data.ShouldBe(items.Count());
        _extractionClient.Verify(x => x.ExtractAsync(null, reference.Text, It.IsAny<string>(), It.IsAny<string>(), It.IsAny<CancellationToken>()), Times.Exactly(items.Count()));
    }

    [Test]
    public async Task Execute_ShouldNotSendCommand_WhenNoQualityCheckIssues()
    {
        // Arrange
        var items = _fixture.CreateMany<Item>(3);
        var reference = _fixture.Create<ItemQualityCheckReferenceModel>();
        var apiResponse = _fixture.Create<ItemQualityCheckApiResponseModel>();
        var qualityCheckResult = _fixture.Build<ItemQualityCheck>()
            .With(q => q.ItemQualityCheckIssues, new List<ItemQualityCheckIssue>())
            .Create();

        _qualityCheckService.Setup(s => s.GetItems()).ReturnsAsync(items);
        _aiConsumerService.Setup(s => s.ConstructReference(It.IsAny<Item>())).ReturnsAsync(reference);
        _extractionClient.Setup(c => c.ExtractAsync(null, reference.Text, It.IsAny<string>(), It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(apiResponse);
        _qualityCheckService.Setup(s => s.PerformQualityCheck(reference, apiResponse)).Returns(qualityCheckResult);

        // Act
        var result = await _job.Execute();

        // Assert
        result.Succeeded.ShouldBeTrue();
        result.Data.ShouldBe(items.Count());
        _mediator.Verify(m => m.Send(It.IsAny<UpsertQualityCheckCommand>(), default), Times.Never);
    }
}