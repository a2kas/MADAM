using Moq;
using NUnit.Framework;
using Tamro.Madam.Application.Commands.Commerce.ItemAssortmentSalesChannels;
using Tamro.Madam.Application.Handlers.Commerce.ItemAssortmentSalesChannels;
using Tamro.Madam.Models.Commerce.ItemAssortmentSalesChannels;
using Tamro.Madam.Repository.Repositories.Commerce.ItemAssortmentSalesChannels;

namespace Tamro.Madam.Application.Tests.Handlers.Commerce.ItemAssortmentSalesChannels;

[TestFixture]
public class UpsertItemAssortmentSalesChannelCommandHandlerTests
{
    private MockRepository _mockRepository;

    private Mock<IItemAssortmentSalesChannelRepository> _itemAssortmentSalesChannelRepository;

    private UpsertItemAssortmentSalesChannelCommandHandler _upsertItemAssortmentSalesChannelCommandHandler;

    [SetUp]
    public void SetUp()
    {
        _mockRepository = new MockRepository(MockBehavior.Loose);

        _itemAssortmentSalesChannelRepository = _mockRepository.Create<IItemAssortmentSalesChannelRepository>();

        _upsertItemAssortmentSalesChannelCommandHandler = new UpsertItemAssortmentSalesChannelCommandHandler(_itemAssortmentSalesChannelRepository.Object);
    }

    [Test]
    public async Task Handle_UpsertsGraph_ItemAssortmentSalesChannelCommand()
    {
        // Arrange
        var model = new ItemAssortmentSalesChannelDetailsModel();
        var request = new UpsertItemAssortmentSalesChannelCommand(model);

        // Act
        await _upsertItemAssortmentSalesChannelCommandHandler.Handle(request, CancellationToken.None);

        // Assert
        _itemAssortmentSalesChannelRepository.Verify(x => x.UpsertGraph(model), Times.Once());
    }
}