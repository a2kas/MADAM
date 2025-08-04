using Moq;
using NUnit.Framework;
using Tamro.Madam.Application.Handlers.ItemMasterdata.Items.Bindings;
using Tamro.Madam.Repository.Repositories.ItemMasterdata.Items.Bindings;

namespace Tamro.Madam.Application.Tests.Handlers.ItemMasterdata.Items.Bindings;

[TestFixture]
public class ItemBindingCountByCompanyQueryHandlerTests
{
    private MockRepository _mockRepository;
    private Mock<IItemBindingRepository> _itemBindingRepository;
    private ItemBindingCountByCompanyQueryHandler _itemBindingCountByCompanyQueryHandler;

    [SetUp]
    public void SetUp()
    {
        _mockRepository = new MockRepository(MockBehavior.Loose);

        _itemBindingRepository = _mockRepository.Create<IItemBindingRepository>();

        _itemBindingCountByCompanyQueryHandler = new ItemBindingCountByCompanyQueryHandler(_itemBindingRepository.Object);
    }

    [Test]
    public async Task Handle_GetsCountByCompany()
    {
        // Act
        await _itemBindingCountByCompanyQueryHandler.Handle(null, CancellationToken.None);

        // Assert
        _itemBindingRepository.Verify(x => x.GetCountByCompany(CancellationToken.None));
    }
}