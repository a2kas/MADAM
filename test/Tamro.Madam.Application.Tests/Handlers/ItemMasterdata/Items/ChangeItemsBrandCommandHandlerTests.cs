using System.Linq.Expressions;
using System.Reflection;
using AutoFixture;
using AutoMapper;
using Shouldly;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using Tamro.Madam.Application.Commands.ItemMasterdata.Items;
using Tamro.Madam.Application.Handlers.ItemMasterdata.Items;
using Tamro.Madam.Application.Profiles.ItemMasterdata.Items;
using Tamro.Madam.Models.ItemMasterdata.Brands;
using Tamro.Madam.Models.ItemMasterdata.Items;
using Tamro.Madam.Repository.Common;
using Tamro.Madam.Repository.Entities.ItemMasterdata.Items;
using Tamro.Madam.Repository.Repositories.ItemMasterdata.Items;

namespace Tamro.Madam.Application.Tests.Handlers.ItemMasterdata.Items;

[TestFixture]
public class ChangeItemsBrandCommandHandlerTests
{
    private MockRepository _mockRepository;
    private Mock<IItemRepository> _itemRepository;
    private Mock<ILogger<ChangeItemsBrandCommandHandler>> _logger;
    private IMapper _mapper;
    private Fixture _fixture;
    private ChangeItemsBrandCommandHandler _handler;

    [SetUp]
    public void SetUp()
    {
        _mockRepository = new MockRepository(MockBehavior.Loose);
        _itemRepository = _mockRepository.Create<IItemRepository>();
        _logger = _mockRepository.Create<ILogger<ChangeItemsBrandCommandHandler>>();
        _mapper = new Mapper(new MapperConfiguration(cfg => cfg.AddMaps(Assembly.GetAssembly(typeof(ItemProfile)))));
        _fixture = new Fixture();
        _handler = new ChangeItemsBrandCommandHandler(_itemRepository.Object, _mapper);
    }

    [Test]
    public async Task Handle_UpdatesBrandCorrectly_AndReturnsSuccessResult()
    {
        // Arrange
        var items = _fixture.CreateMany<ItemModel>(2).ToList();
        var newBrand = new BrandClsfModel()
        {
            Id = 25,
        };
        var cmd = new ChangeItemsBrandCommand(items, newBrand);
        _itemRepository.Setup(x => x.GetList(It.IsAny<Expression<Func<Item, bool>>>(), It.IsAny<List<IncludeOperation<Item>>>(), It.IsAny<bool>(), It.IsAny<int>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(_mapper.Map<List<Item>>(items));

        // Act
        var result = await _handler.Handle(cmd, CancellationToken.None);

        // Assert
        _itemRepository.Verify(x => x.UpdateMany(It.Is<List<Item>>(y => y.Count == 2 && y.TrueForAll(t => t.BrandId == 25)), CancellationToken.None), Times.Once);
        result.Succeeded.ShouldBeTrue();
    }
}