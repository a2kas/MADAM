using System.Reflection;
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
using Tamro.Madam.Repository.Entities.ItemMasterdata.Items;
using Tamro.Madam.Repository.Repositories.ItemMasterdata.Items;

namespace Tamro.Madam.Application.Tests.Handlers.ItemMasterdata.Items;

[TestFixture]
public class InitializeParallelItemCommandHandlerTests
{
    private MockRepository _mockRepository;

    private Mock<IItemRepository> _itemRepository;
    private IMapper _mapper;

    private InitializeParallelItemCommandHandler _initializeParallelItemCommandHandler;
    private Fixture _fixture;

    [SetUp]
    public void SetUp()
    {
        _mockRepository = new MockRepository(MockBehavior.Loose);

        _itemRepository = _mockRepository.Create<IItemRepository>();
        _mapper = new Mapper(new MapperConfiguration(cfg => cfg.AddMaps(Assembly.GetAssembly(typeof(ItemProfile)))));

        _initializeParallelItemCommandHandler = new InitializeParallelItemCommandHandler(_itemRepository.Object, _mapper);
        _fixture = new Fixture();
        _fixture.Behaviors.OfType<ThrowingRecursionBehavior>().ToList()
            .ForEach(b => _fixture.Behaviors.Remove(b));
        _fixture.Behaviors.Add(new OmitOnRecursionBehavior());
    }

    [Test(Description = "When creating a parallel item, it is important to clear the fields that are exclusive to the item it is a parallel to. These fields are: Id, EditedAt, EditedBy." +
        "Also, it is important to postfix ItemName with P and set ParallelParentItemId to the correct value")]
    public async Task Command_InitializesParallelVersionOfItem_Correctly()
    {
        // Arrange
        const int itemId = 6;
        var command = new InitializeParallelItemCommand(itemId);
        var item = _fixture.Create<ItemModel>();
        _itemRepository.Setup(x => x.Get(6, It.IsAny<List<IncludeOperation<Item>>>()))
            .ReturnsAsync(_mapper.Map<Item>(item));

        // Act
        var result = (await _initializeParallelItemCommandHandler.Handle(command, CancellationToken.None)).Data;

        // Assert
        result.Id.ShouldBe(default);
        result.ItemName.ShouldEndWith(" P");
        result.EditedAt.ShouldBe(null);
        result.EditedBy.ShouldBe(string.Empty);
        result.ParallelParentItemId.ShouldBe(itemId);
    }
}