using AutoFixture;
using Shouldly;
using Moq;
using NUnit.Framework;
using System.Linq.Expressions;
using Tamro.Madam.Application.Commands.ItemMasterdata.Items;
using Tamro.Madam.Application.Handlers.ItemMasterdata.Items;
using Tamro.Madam.Models.State.General;
using Tamro.Madam.Repository.Common;
using Tamro.Madam.Repository.Entities.ItemMasterdata.Items;
using Tamro.Madam.Repository.Repositories.ItemMasterdata.Items;

namespace Tamro.Madam.Application.Tests.Handlers.ItemMasterdata.Items;

[TestFixture]
public class ActivateItemsCommandHandlerTests
{
    private Fixture _fixture;
    private UserProfileStateModel _user;

    private Mock<IItemRepository> _itemRepository;

    private ActivateItemsCommandHandler _handler;

    [SetUp]
    public void SetUp()
    {
        _fixture = new Fixture();
        _fixture.Behaviors.OfType<ThrowingRecursionBehavior>().ToList().ForEach(b => _fixture.Behaviors.Remove(b));
        _fixture.Behaviors.Add(new OmitOnRecursionBehavior());

        _user = new UserProfileStateModel { DisplayName = "Test.Test" };

        _itemRepository = new Mock<IItemRepository>();

        _handler = new ActivateItemsCommandHandler(_itemRepository.Object);
    }

    [Test]
    public async Task Handle_GetsTrackedItemsList()
    {
        // Arrange
        var itemsToActivate = _fixture.Build<Item>().With(x => x.Active, false).CreateMany(5).ToList();
        _itemRepository.Setup(x => x.GetList(It.IsAny<Expression<Func<Item, bool>>>(), It.IsAny<List<IncludeOperation<Item>>>(), It.IsAny<bool>(), It.IsAny<int>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(itemsToActivate);

        var command = new ActivateItemsCommand(itemsToActivate.Select(x => x.Id).ToArray(), _user);
        var cancellationToken = new CancellationToken();

        // Act
        await _handler.Handle(command, cancellationToken);

        // Assert
        _itemRepository.Verify(x => x.GetList(It.IsAny<Expression<Func<Item, bool>>>(), It.IsAny<List<IncludeOperation<Item>>>(), true, It.IsAny<int>(), cancellationToken), Times.Once);
    }

    [Test]
    public async Task Handle_SavesActivatedItems()
    {
        // Arrange
        var itemsToActivate = _fixture.Build<Item>()
            .With(x => x.Active, false)
            .With(x => x.EditedBy, "John.Doe")
            .CreateMany(5).ToList();
        _itemRepository.Setup(x => x.GetList(It.IsAny<Expression<Func<Item, bool>>>(), It.IsAny<List<IncludeOperation<Item>>>(), It.IsAny<bool>(), It.IsAny<int>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(itemsToActivate);

        var command = new ActivateItemsCommand(itemsToActivate.Select(x => x.Id).ToArray(), _user);
        var cancellationToken = new CancellationToken();

        // Act
        await _handler.Handle(command, cancellationToken);

        // Assert
        _itemRepository.Verify(x => x.SaveChangesAsync(cancellationToken), Times.Once);
        itemsToActivate.All(x => x.Active).ShouldBeTrue();
        itemsToActivate.All(x => x.EditedBy == _user.DisplayName).ShouldBeTrue();
    }
}
