using AutoFixture;
using AutoMapper;
using Shouldly;
using Moq;
using NUnit.Framework;
using System.Linq.Expressions;
using System.Reflection;
using Tamro.Madam.Application.Commands.ItemMasterdata.Items.Bindings;
using Tamro.Madam.Application.Handlers.ItemMasterdata.Items.Bindings;
using Tamro.Madam.Application.Profiles.ItemMasterdata.Items.Bindings;
using Tamro.Madam.Repository.Common;
using Tamro.Madam.Repository.Entities.ItemMasterdata.Items.Bindings;
using Tamro.Madam.Repository.Repositories.ItemMasterdata.Items.Bindings;

namespace Tamro.Madam.Application.Tests.Handlers.ItemMasterdata.Items.Bindings;

[TestFixture]
public class GetItemItemBindingsCommandHandlerTests
{
    private MockRepository _mockRepository;

    private Mock<IItemBindingRepository> _itemBindingRepository;
    private IMapper _mapper;

    private GetItemItemBindingsCommandHandler _getItemItemBindingsCommandHandler;
    private Fixture _fixture;

    [SetUp]
    public void SetUp()
    {
        _mockRepository = new MockRepository(MockBehavior.Loose);

        _itemBindingRepository = _mockRepository.Create<IItemBindingRepository>();
        _mapper = new Mapper(new MapperConfiguration(cfg => cfg.AddMaps(Assembly.GetAssembly(typeof(ItemBindingProfile)))));

        _getItemItemBindingsCommandHandler = new GetItemItemBindingsCommandHandler(_itemBindingRepository.Object, _mapper);
        _fixture = new Fixture();
        _fixture.Behaviors.OfType<ThrowingRecursionBehavior>().ToList()
            .ForEach(b => _fixture.Behaviors.Remove(b));
        _fixture.Behaviors.Add(new OmitOnRecursionBehavior());
    }

    [Test]
    public async Task Handle_GetsBindings_Correctly()
    {
        // Arrange
        const int itemId = 6;
        const int bindingCount = 2;
        var command = new GetItemItemBindingsCommand(itemId);
        var bindings = _fixture.CreateMany<ItemBinding>(bindingCount);
        _itemBindingRepository.Setup(x => x.GetList(It.IsAny<Expression<Func<ItemBinding, bool>>>(), It.IsAny<List<IncludeOperation<ItemBinding>>>(), false, CancellationToken.None)).ReturnsAsync(bindings);

        // Act
        var result = await _getItemItemBindingsCommandHandler.Handle(command, CancellationToken.None);

        // Assert
        result.Succeeded.ShouldBeTrue();
        result.Data.Count().ShouldBe(bindingCount);
    }
}