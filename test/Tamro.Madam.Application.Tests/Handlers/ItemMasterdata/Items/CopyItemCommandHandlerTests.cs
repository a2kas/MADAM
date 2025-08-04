using AutoFixture;
using AutoMapper;
using Moq;
using NUnit.Framework;
using Shouldly;
using Tamro.Madam.Application.Commands.ItemMasterdata.Items;
using Tamro.Madam.Application.Handlers.ItemMasterdata.Items;
using Tamro.Madam.Application.Profiles.ItemMasterdata.Items;
using Tamro.Madam.Models.ItemMasterdata.Items;
using Tamro.Madam.Repository.Common;
using Tamro.Madam.Repository.Entities.ItemMasterdata.Barcodes;
using Tamro.Madam.Repository.Entities.ItemMasterdata.Items;
using Tamro.Madam.Repository.Entities.ItemMasterdata.Items.Bindings;
using Tamro.Madam.Repository.Repositories.ItemMasterdata.Items;

namespace Tamro.Madam.Application.Tests.Handlers.ItemMasterdata.Items;

[TestFixture]
public class CopyItemCommandHandlerTests
{
    private MockRepository _mockRepository;

    private Mock<IItemRepository> _itemRepository;
    private IMapper _mapper;

    private CopyItemCommandHandler _copyItemCommandHandler;
    private Fixture _fixture;

    [SetUp]
    public void SetUp()
    {
        _mockRepository = new MockRepository(MockBehavior.Loose);

        _itemRepository = _mockRepository.Create<IItemRepository>();
        _mapper = new Mapper(new MapperConfiguration(cfg => cfg.AddMaps(System.Reflection.Assembly.GetAssembly(typeof(ItemProfile)))));

        _copyItemCommandHandler = new CopyItemCommandHandler(_itemRepository.Object, _mapper);
        _fixture = new Fixture();
        _fixture.Behaviors.OfType<ThrowingRecursionBehavior>().ToList()
            .ForEach(b => _fixture.Behaviors.Remove(b));
        _fixture.Behaviors.Add(new OmitOnRecursionBehavior());
    }

    [Test]
    public async Task Handle_CopiesItem_Correctly()
    {
        // Arrange
        var itemModel = _fixture.Create<ItemModel>();
        var command = new CopyItemCommand(itemModel.Id);
        var item = GetDefaultItemWithEmptyCollections(itemModel.Id);

        _itemRepository.Setup(x => x.Get(It.IsAny<int>(), It.IsAny<List<IncludeOperation<Item>>>()))
            .ReturnsAsync(item);

        // Act
        var result = await _copyItemCommandHandler.Handle(command, CancellationToken.None);

        // Assert
        result.Succeeded.ShouldBeTrue();
        result.Data.Id.ShouldBe(default);
        result.Data.Barcodes.ShouldBeEmpty();
        result.Data.Bindings.ShouldBeEmpty();
    }

    private Item GetDefaultItemWithEmptyCollections(int itemId)
    {
        return _fixture.Build<Item>()
            .With(i => i.Id, itemId)
            .With(i => i.Barcodes, new List<Barcode>())
            .With(i => i.Bindings, new List<ItemBinding>())
            .Create();
    }
}
