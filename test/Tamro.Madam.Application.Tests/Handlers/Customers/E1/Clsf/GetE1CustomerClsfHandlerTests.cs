using Moq;
using NUnit.Framework;
using Tamro.Madam.Application.Handlers.Customers.E1.Clsf;
using Tamro.Madam.Application.Queries.Customers.E1.Clsf;
using Tamro.Madam.Repository.Repositories.Customers.E1;

namespace Tamro.Madam.Application.Tests.Handlers.Customers.E1.Clsf;

[TestFixture]
public class GetE1CustomerClsfHandlerTests
{
    private MockRepository _mockRepository;

    private Mock<IE1CustomerRepository> _e1CustomerRepository;

    private GetE1CustomerClsfHandler _getE1CustomerClsfHandler;

    [SetUp]
    public void SetUp()
    {
        _mockRepository = new MockRepository(MockBehavior.Loose);

        _e1CustomerRepository = _mockRepository.Create<IE1CustomerRepository>();

        _getE1CustomerClsfHandler = new GetE1CustomerClsfHandler(_e1CustomerRepository.Object);
    }

    [Test]
    public async Task Handle_GetsClsf()
    {
        // Arrange
        var request = new E1CustomerClsfQuery()
        {
            SearchTerm = "Test",
        };

        // Act
        await _getE1CustomerClsfHandler.Handle(request, CancellationToken.None);

        // Assert
        _e1CustomerRepository.Verify(x => x.GetClsf("Test", 1, 20), Times.Once);
    }
}