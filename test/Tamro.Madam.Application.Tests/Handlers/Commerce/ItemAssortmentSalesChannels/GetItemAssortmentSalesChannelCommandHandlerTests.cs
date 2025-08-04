using Moq;
using NUnit.Framework;
using Tamro.Madam.Application.Commands.Commerce.ItemAssortmentSalesChannels;
using Tamro.Madam.Application.Handlers.Commerce.ItemAssortmentSalesChannels;
using Tamro.Madam.Repository.Repositories.Commerce.ItemAssortmentSalesChannels;

namespace Tamro.Madam.Application.Tests.Handlers.Commerce.ItemAssortmentSalesChannels;

[TestFixture]
public class GetItemAssortmentSalesChannelCommandHandlerTests
{
    private MockRepository _mockRepository;

    private Mock<IItemAssortmentSalesChannelRepository> _itemAssortmentSalesChannelRepository;

    private GetItemAssortmentSalesChannelCommandHandler _getItemAssortmentSalesChannelCommandHandler;

    [SetUp]
    public void SetUp()
    {
        _mockRepository = new MockRepository(MockBehavior.Loose);

        _itemAssortmentSalesChannelRepository = _mockRepository.Create<IItemAssortmentSalesChannelRepository>();

        _getItemAssortmentSalesChannelCommandHandler = new GetItemAssortmentSalesChannelCommandHandler(_itemAssortmentSalesChannelRepository.Object);
    }

    [Test]
    public async Task Handle_Gets_ItemAssortmentSalesChannel()
    {
        // Arrange
        var request = new GetItemAssortmentSalesChannelCommand(4);

        // Act
        await _getItemAssortmentSalesChannelCommandHandler.Handle(request, CancellationToken.None);

        // Assert
        _itemAssortmentSalesChannelRepository.Verify(x => x.Get(4), Times.Once);
    }
}