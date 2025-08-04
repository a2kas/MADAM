using AutoFixture;
using Moq;
using NUnit.Framework;
using Shouldly;
using Tamro.Madam.Application.Services.Items.QualityCheck;
using Tamro.Madam.Models.ItemMasterdata.Items.QualityCheck;
using Tamro.Madam.Repository.Common;
using Tamro.Madam.Repository.Entities.ItemMasterdata.Items;
using Tamro.Madam.Repository.Entities.ItemMasterdata.Items.Bindings;
using Tamro.Madam.Repository.Repositories.ItemMasterdata.Items;

namespace Tamro.Madam.Application.Tests.Services.Items.QualityCheck;

[TestFixture]
public class ItemMasterdataQualityCheckServiceTests
{
    private Fixture _fixture;
    private MockRepository _mockRepository;

    private Mock<IItemRepository> _itemRepository;
    private Mock<IIssueEntityResolver> _issueEntityResolver;
    private Mock<IIssueSeverityResolver> _issueSeverityResolver;
    private Mock<IActualValueResolver> _actualValueResolver;

    private ItemMasterdataQualityCheckService _itemMasterdataQualityCheckService;

    [SetUp]
    public void SetUp()
    {
        _fixture = new Fixture();
        _fixture.Behaviors.OfType<ThrowingRecursionBehavior>().ToList()
            .ForEach(b => _fixture.Behaviors.Remove(b));
        _fixture.Behaviors.Add(new OmitOnRecursionBehavior());

        _mockRepository = new MockRepository(MockBehavior.Loose);

        _itemRepository = _mockRepository.Create<IItemRepository>();
        _issueEntityResolver = _mockRepository.Create<IIssueEntityResolver>();
        _issueSeverityResolver = _mockRepository.Create<IIssueSeverityResolver>();
        _actualValueResolver = _mockRepository.Create<IActualValueResolver>();

        _itemMasterdataQualityCheckService = new ItemMasterdataQualityCheckService(_itemRepository.Object, _issueEntityResolver.Object, _issueSeverityResolver.Object, _actualValueResolver.Object);
    }

    [Test]
    public async Task GetItems_ShouldReturnItemsWithBindings()
    {
        // Arrange
        var items = _fixture.CreateMany<Item>(10).ToList();
        items.ForEach(item => item.Bindings = _fixture.CreateMany<ItemBinding>(1).ToList());

        _itemRepository.Setup(repo => repo.GetList(It.IsAny<System.Linq.Expressions.Expression<Func<Item, bool>>>(), It.IsAny<List<IncludeOperation<Item>>>(), It.IsAny<bool>(),
            It.IsAny<int>(),
            It.IsAny<CancellationToken>()
        )).ReturnsAsync(items);

        // Act
        var result = await _itemMasterdataQualityCheckService.GetItems();

        // Assert
        result.ShouldNotBeNull();
        result.Count().ShouldBe(10);
        _itemRepository.Verify(repo => repo.GetList(
            It.IsAny<System.Linq.Expressions.Expression<Func<Item, bool>>>(), It.IsAny<List<IncludeOperation<Item>>>(), It.IsAny<bool>(), It.IsAny<int>(), It.IsAny<CancellationToken>()
        ), Times.Once);
    }

    [Test]
    public void PerformQualityCheck_ShouldReturnQualityCheckResultWithIssues()
    {
        // Arrange
        var reference = _fixture.Create<ItemQualityCheckReferenceModel>();
        var response = _fixture.Create<ItemQualityCheckApiResponseModel>();
        var extraction = _fixture.Create<ItemQualityCheckExtractedInformationModel>();
        extraction.Section = "LV_001";
        extraction.Fields = _fixture.CreateMany<ItemQualityCheckExtractedFieldModel>(3).ToList();
        extraction.Fields.ForEach(field => field.IssuesType = ItemQualityCheckIssueType.MissingData);
        response.ExtractedInformation = new List<ItemQualityCheckExtractedInformationModel> { extraction };

        var binding = _fixture.Build<ItemBindingQualityCheckReferenceModel>()
            .With(b => b.LocalId, "001")
            .Create();
        reference.Bindings = [binding];

        _issueEntityResolver.Setup(resolver => resolver.ResolveIssueEntity(It.IsAny<string>()))
            .Returns(nameof(Item));
        _issueSeverityResolver.Setup(resolver => resolver.ResolveIssueSeverity(It.IsAny<ItemQualityCheckIssueType?>()))
            .Returns(ItemQualityIssueSeverity.Low);
        _actualValueResolver.Setup(resolver => resolver.ResolveActualValue(It.IsAny<string>(), It.IsAny<ItemBindingQualityCheckReferenceModel>(), It.IsAny<ItemQualityCheckReferenceModel>()))
            .Returns("ActualValue");

        // Act
        var result = _itemMasterdataQualityCheckService.PerformQualityCheck(reference, response);

        // Assert
        result.ShouldNotBeNull();
        result.ItemQualityCheckIssues.ShouldNotBeEmpty();
        result.ItemQualityCheckIssues.Count.ShouldBe(3);
        result.ItemQualityCheckIssues.All(issue => issue.IssueSeverity == ItemQualityIssueSeverity.Low).ShouldBeTrue();
        result.ItemQualityCheckIssues.All(issue => issue.IssueEntity == nameof(Item)).ShouldBeTrue();
        result.ItemQualityCheckIssues.All(issue => issue.ActualValue == "ActualValue").ShouldBeTrue();
        _issueEntityResolver.Verify(resolver => resolver.ResolveIssueEntity(It.IsAny<string>()), Times.Exactly(3));
        _issueSeverityResolver.Verify(resolver => resolver.ResolveIssueSeverity(It.IsAny<ItemQualityCheckIssueType?>()), Times.Exactly(3));
        _actualValueResolver.Verify(resolver => resolver.ResolveActualValue(It.IsAny<string>(), It.IsAny<ItemBindingQualityCheckReferenceModel>(), It.IsAny<ItemQualityCheckReferenceModel>()), Times.Exactly(3));
    }

    [Test]
    public void PerformQualityCheck_ShouldHandleMultipleExtractions()
    {
        // Arrange
        var reference = _fixture.Create<ItemQualityCheckReferenceModel>();
        var response = _fixture.Create<ItemQualityCheckApiResponseModel>();
        var extraction1 = _fixture.Create<ItemQualityCheckExtractedInformationModel>();
        extraction1.Section = "LV_001";
        extraction1.Fields = _fixture.CreateMany<ItemQualityCheckExtractedFieldModel>(2).ToList();
        extraction1.Fields.ForEach(field => field.IssuesType = ItemQualityCheckIssueType.MissingData);

        var extraction2 = _fixture.Create<ItemQualityCheckExtractedInformationModel>();
        extraction2.Section = "EE_002";
        extraction2.Fields = _fixture.CreateMany<ItemQualityCheckExtractedFieldModel>(2).ToList();
        extraction2.Fields.ForEach(field => field.IssuesType = ItemQualityCheckIssueType.ErroneousHtmlTagging);

        response.ExtractedInformation = [extraction1, extraction2];

        var binding1 = _fixture.Build<ItemBindingQualityCheckReferenceModel>()
            .With(b => b.LocalId, "001")
            .Create();
        var binding2 = _fixture.Build<ItemBindingQualityCheckReferenceModel>()
            .With(b => b.LocalId, "002")
            .Create();
        reference.Bindings = [binding1, binding2];

        _issueEntityResolver.Setup(resolver => resolver.ResolveIssueEntity(It.IsAny<string>()))
            .Returns(nameof(ItemBinding));
        _issueSeverityResolver.Setup(resolver => resolver.ResolveIssueSeverity(It.IsAny<ItemQualityCheckIssueType?>()))
            .Returns(ItemQualityIssueSeverity.Medium);
        _actualValueResolver.Setup(resolver => resolver.ResolveActualValue(It.IsAny<string>(), It.IsAny<ItemBindingQualityCheckReferenceModel>(), It.IsAny<ItemQualityCheckReferenceModel>()))
            .Returns("ActualValue");

        // Act
        var result = _itemMasterdataQualityCheckService.PerformQualityCheck(reference, response);

        // Assert
        result.ShouldNotBeNull();
        result.ItemQualityCheckIssues.ShouldNotBeEmpty();
        result.ItemQualityCheckIssues.Count.ShouldBe(4);
        result.ItemQualityCheckIssues.All(issue => issue.IssueSeverity == ItemQualityIssueSeverity.Medium).ShouldBeTrue();
        result.ItemQualityCheckIssues.All(issue => issue.IssueEntity == nameof(ItemBinding)).ShouldBeTrue();
        result.ItemQualityCheckIssues.All(issue => issue.ActualValue == "ActualValue").ShouldBeTrue();
        _issueEntityResolver.Verify(resolver => resolver.ResolveIssueEntity(It.IsAny<string>()), Times.Exactly(4));
        _issueSeverityResolver.Verify(resolver => resolver.ResolveIssueSeverity(It.IsAny<ItemQualityCheckIssueType?>()), Times.Exactly(4));
        _actualValueResolver.Verify(resolver => resolver.ResolveActualValue(It.IsAny<string>(), It.IsAny<ItemBindingQualityCheckReferenceModel>(), It.IsAny<ItemQualityCheckReferenceModel>()), Times.Exactly(4));
    }

    [Test]
    public void PerformQualityCheck_ShouldAssignItemBindingId_WhenIssueEntityIsItemBinding()
    {
        // Arrange
        var reference = _fixture.Create<ItemQualityCheckReferenceModel>();
        var response = _fixture.Create<ItemQualityCheckApiResponseModel>();
        var extraction = _fixture.Create<ItemQualityCheckExtractedInformationModel>();
        extraction.Section = "LV_001";
        extraction.Fields = _fixture.CreateMany<ItemQualityCheckExtractedFieldModel>(1).ToList();
        extraction.Fields.ForEach(field => field.IssuesType = ItemQualityCheckIssueType.MissingData);
        response.ExtractedInformation = [extraction];

        var binding = _fixture.Build<ItemBindingQualityCheckReferenceModel>()
            .With(b => b.LocalId, "001")
            .Create();
        reference.Bindings = [binding];

        _issueEntityResolver.Setup(resolver => resolver.ResolveIssueEntity(It.IsAny<string>()))
            .Returns(nameof(ItemBinding));
        _issueSeverityResolver.Setup(resolver => resolver.ResolveIssueSeverity(It.IsAny<ItemQualityCheckIssueType?>()))
            .Returns(ItemQualityIssueSeverity.Low);
        _actualValueResolver.Setup(resolver => resolver.ResolveActualValue(It.IsAny<string>(), It.IsAny<ItemBindingQualityCheckReferenceModel>(), It.IsAny<ItemQualityCheckReferenceModel>()))
            .Returns("ActualValue");

        // Act
        var result = _itemMasterdataQualityCheckService.PerformQualityCheck(reference, response);

        // Assert
        result.ShouldNotBeNull();
        result.ItemQualityCheckIssues.ShouldNotBeEmpty();
        result.ItemQualityCheckIssues.Count.ShouldBe(1);
        result.ItemQualityCheckIssues.First().ItemBindingId.ShouldBe(binding.Id);
        _issueEntityResolver.Verify(resolver => resolver.ResolveIssueEntity(It.IsAny<string>()), Times.Once);
        _issueSeverityResolver.Verify(resolver => resolver.ResolveIssueSeverity(It.IsAny<ItemQualityCheckIssueType?>()), Times.Once);
        _actualValueResolver.Verify(resolver => resolver.ResolveActualValue(It.IsAny<string>(), It.IsAny<ItemBindingQualityCheckReferenceModel>(), It.IsAny<ItemQualityCheckReferenceModel>()), Times.Once);
    }
}