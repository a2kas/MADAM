using Moq;
using NUnit.Framework;
using Tamro.Madam.Application.Handlers.Customers.Wholesale.Clsf;
using Tamro.Madam.Application.Queries.Customers.Wholesale.Clsf;
using Tamro.Madam.Application.Services.Customers.Factories;
using Tamro.Madam.Models.Customers.Wholesale;
using Tamro.Madam.Models.General;
using Tamro.Madam.Repository.Repositories.Customers.Wholesale;

namespace Tamro.Madam.Application.Tests.Handlers.Customers.Wholesale.Clsf;

[TestFixture]
public class GetWholesaleCustomerClsfHandlerTests
{
    private MockRepository _mockRepository;

    private Mock<IWholesaleCustomerRepositoryFactory> _wholesaleCustomerRepositoryFactory;

    private GetWholesaleCustomerClsfHandler _getWholesaleCustomerClsfHandler;

    [SetUp]
    public void SetUp()
    {
        _mockRepository = new MockRepository(MockBehavior.Loose);

        _wholesaleCustomerRepositoryFactory = _mockRepository.Create<IWholesaleCustomerRepositoryFactory>();

        _getWholesaleCustomerClsfHandler = new GetWholesaleCustomerClsfHandler(_wholesaleCustomerRepositoryFactory.Object);
    }

    [Test]
    public async Task Handle_GetsCountryDependantClsf()
    {
        // Arrange
        var request = new WholesaleCustomerClsfQuery()
        {
            Country = BalticCountry.EE,
            SearchTerm = "Te",
            CustomerType = WholesaleCustomerType.LegalEntity,
        };
        var repositoryMock = _mockRepository.Create<IWholesaleCustomerRepository>();
        _wholesaleCustomerRepositoryFactory.Setup(x => x.Get(BalticCountry.EE)).Returns(repositoryMock.Object);

        // Act
        await _getWholesaleCustomerClsfHandler.Handle(request, CancellationToken.None);

        // Assert
        repositoryMock.Verify(x => x.GetClsf("Te", WholesaleCustomerType.LegalEntity, 1, 20), Times.Once);
    }
}