using System.Linq.Expressions;
using System.Reflection;
using AutoMapper;
using Shouldly;
using Moq;
using NUnit.Framework;
using Tamro.Madam.Application.Commands.ItemMasterdata.Brands;
using Tamro.Madam.Application.Handlers.ItemMasterdata.Brands;
using Tamro.Madam.Application.Models.Common;
using Tamro.Madam.Application.Profiles.ItemMasterdata;
using Tamro.Madam.Models.ItemMasterdata.Brands;
using Tamro.Madam.Repository.Common;
using Tamro.Madam.Repository.Entities.ItemMasterdata.Items;
using Tamro.Madam.Repository.Repositories.ItemMasterdata.Items;

namespace Tamro.Madam.Application.Tests.Handlers.ItemMasterdata.Brands;

[TestFixture]
public class GetBrandsDeletionOverviewCommandHandlerTests
{
    private Mock<IItemRepository> _itemRepository;
    private IMapper _mapper;
    private GetBrandsDeletionOverviewCommandHandler _handler;

    [SetUp]
    public void Setup()
    {
        _itemRepository = new Mock<IItemRepository>();

        _mapper = new Mapper(new MapperConfiguration(cfg => cfg.AddMaps(Assembly.GetAssembly(typeof(BrandProfile)))));
        _handler = new GetBrandsDeletionOverviewCommandHandler(_itemRepository.Object, _mapper);
    }

    [Test]
    [Ignore("TODO: OTHERS-2886")]
    public async Task Handle_Should_ReturnSuccessResult_WithCorrectlyJoinedBrandDeletionOverviewModels()
    {
        // Arrange
        var request = new GetBrandsDeletionOverviewCommand
        (
            [
                new BrandModel { Id = 1, Name = "Brand1" },
                new BrandModel { Id = 2, Name = "Brand2" }
            ]
        );
        var items = new List<Item>
        {
            new() { BrandId = 2, ItemName = "Item1" },
            new() { BrandId = 2, ItemName = "Item2" },
        };
        _itemRepository.Setup(x => x.GetList(It.IsAny<Expression<Func<Item, bool>>>(), It.IsAny<List<IncludeOperation<Item>>>(), It.IsAny<bool>(), 10, It.IsAny<CancellationToken>()))
            .ReturnsAsync(items);


        // Act
        var result = await _handler.Handle(request, CancellationToken.None);

        // Assert
        result.ShouldBeOfType<Result<IEnumerable<BrandDeletionOverviewModel>>>();
        result.Succeeded.ShouldBeTrue();
        result.Data.Count().ShouldBe(2);
        result.Data.Any(x => x.AttachedItems.Count == 2).ShouldBe(true);
        result.Data.Any(x => x.AttachedItems.Count == 0).ShouldBe(true);
    }
}