using Moq;
using NUnit.Framework;
using Tamro.Madam.Application.Commands.Suppliers;
using Tamro.Madam.Application.Handlers.Suppliers;
using Tamro.Madam.Repository.Repositories.Suppliers;

namespace Tamro.Madam.Application.Tests.Handlers.Suppliers;

[TestFixture]
public class GetSupplierCommandHandlerTests
{
    private MockRepository _mockRepository;

    private Mock<ISupplierRepository> _supplierRepository;

    private GetSupplierCommandHandler _getSupplierCommandHandler;

    [SetUp]
    public void SetUp()
    {
        _mockRepository = new MockRepository(MockBehavior.Loose);

        _supplierRepository = _mockRepository.Create<ISupplierRepository>();

        _getSupplierCommandHandler = new GetSupplierCommandHandler(_supplierRepository.Object);
    }

    [Test]
    public async Task Handle_GetsSupplier()
    {
        // Arrange
        var cmd = new GetSupplierCommand(5);

        // Act
        await _getSupplierCommandHandler.Handle(cmd, CancellationToken.None);

        // Assert
        _supplierRepository.Verify(x => x.Get(5), Times.Once);
    }
}