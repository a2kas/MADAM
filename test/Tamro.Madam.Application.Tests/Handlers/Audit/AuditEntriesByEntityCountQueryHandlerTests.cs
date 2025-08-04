using Moq;
using NUnit.Framework;
using Tamro.Madam.Application.Handlers.Audit;
using Tamro.Madam.Repository.Repositories.Audit;

namespace Tamro.Madam.Application.Tests.Handlers.Audit;

[TestFixture]
public class AuditEntriesByEntityCountQueryHandlerTests
{
    private MockRepository _mockRepository;
    private Mock<IAuditRepository> _auditRepository;
    private AuditEntriesByEntityCountQueryHandler _auditEntriesByEntityCountQueryHandler;

    [SetUp]
    public void SetUp()
    {
        _mockRepository = new MockRepository(MockBehavior.Loose);
        _auditRepository = _mockRepository.Create<IAuditRepository>();
        _auditEntriesByEntityCountQueryHandler = new AuditEntriesByEntityCountQueryHandler(_auditRepository.Object);
    }

    [Test]
    public async Task Handle_Gets_AuditEntriesByEntityCountModel()
    {
        // Act
        await _auditEntriesByEntityCountQueryHandler.Handle(null, CancellationToken.None);

        // Assert
        _auditRepository.Verify(x => x.GetAuditEntriesCountByEntityType(CancellationToken.None), Times.Once);
    }
}