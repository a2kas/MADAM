using Moq;
using NUnit.Framework;
using Tamro.Madam.Application.Commands.Suppliers;
using Tamro.Madam.Application.Handlers.Suppliers;
using Tamro.Madam.Models.Suppliers;
using Tamro.Madam.Repository.Repositories.Suppliers;

namespace Tamro.Madam.Application.Tests.Handlers.Suppliers;

[TestFixture]
public class UpsertSupplierCommandHandlerTests
{
    private MockRepository _mockRepository;

    private Mock<ISupplierRepository> _supplierRepository;

    private UpsertSupplierCommandHandler _upsertSupplierCommandHandler;

    [SetUp]
    public void SetUp()
    {
        _mockRepository = new MockRepository(MockBehavior.Loose);

        _supplierRepository = _mockRepository.Create<ISupplierRepository>();

        _upsertSupplierCommandHandler = new UpsertSupplierCommandHandler(_supplierRepository.Object);
    }

    [Test]
    public async Task Handle_Upserts_Supplier()
    {
        // Arrange
        var model = new SupplierDetailsModel();
        var request = new UpsertSupplierCommand(model);

        // Act
        await _upsertSupplierCommandHandler.Handle(request, CancellationToken.None);

        // Assert
        _supplierRepository.Verify(x => x.UpsertGraph(model), Times.Once);
    }
}