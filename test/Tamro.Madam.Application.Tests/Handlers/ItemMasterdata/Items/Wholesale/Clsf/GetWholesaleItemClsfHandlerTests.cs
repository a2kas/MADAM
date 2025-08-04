using Moq;
using NUnit.Framework;
using Tamro.Madam.Application.Handlers.ItemMasterdata.Items.Wholesale.Clsf;
using Tamro.Madam.Application.Queries.Items.Wholesale.Clsf;
using Tamro.Madam.Application.Services.Items.Wholesale.Factories;
using Tamro.Madam.Models.General;
using Tamro.Madam.Repository.Repositories.ItemMasterdata.Items.Wholesale;

namespace Tamro.Madam.Application.Tests.Handlers.ItemMasterdata.Items.Wholesale.Clsf;

[TestFixture]
public class GetWholesaleItemClsfHandlerTests
{
    private MockRepository _mockRepository;

    private Mock<IWholesaleItemRepositoryFactory> _wholesaleItemRepositoryFactory;

    private GetWholesaleItemClsfHandler _getWholesaleItemClsfHandler;

    [SetUp]
    public void SetUp()
    {
        _mockRepository = new MockRepository(MockBehavior.Loose);

        _wholesaleItemRepositoryFactory = _mockRepository.Create<IWholesaleItemRepositoryFactory>();

        _getWholesaleItemClsfHandler = new GetWholesaleItemClsfHandler(_wholesaleItemRepositoryFactory.Object);
    }

    [Test]
    public async Task Handle_GetsCountryDependantClsf()
    {
        // Arrange
        var request = new WholesaleItemClsfQuery()
        {
            Country = BalticCountry.EE,
            SearchTerm = "Te",
        };
        var repositoryMock = _mockRepository.Create<IWholesaleItemRepository>();
        _wholesaleItemRepositoryFactory.Setup(x => x.Get(BalticCountry.EE)).Returns(repositoryMock.Object);

        // Act
        await _getWholesaleItemClsfHandler.Handle(request, CancellationToken.None);

        // Assert
        repositoryMock.Verify(x => x.GetClsf("Te", 1, 20), Times.Once);
    }

    [Test]
    public async Task Handle_ItemNosAreProvided_GetsClsfByItemNo()
    {
        // Arrange
        var itemNo2s = new List<string>() { "Uwuwu", };
        var request = new WholesaleItemClsfQuery()
        {
            Country = BalticCountry.EE,
            ItemNo2 = itemNo2s,
        };
        var repositoryMock = _mockRepository.Create<IWholesaleItemRepository>();
        _wholesaleItemRepositoryFactory.Setup(x => x.Get(BalticCountry.EE)).Returns(repositoryMock.Object);

        // Act
        await _getWholesaleItemClsfHandler.Handle(request, CancellationToken.None);

        // Assert
        repositoryMock.Verify(x => x.GetClsf(itemNo2s, 1, int.MaxValue), Times.Once);
    }
}